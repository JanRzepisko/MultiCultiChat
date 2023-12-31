﻿using FluentValidation;
using MediatR;
using MultiCultiChat.App.Application.DataAccess;
using MultiCultiChat.App.Domain.Entities;
using Shared.Service.Interfaces;

namespace MultiCultiChat.App.Application.Actions.Chat;

public static class CreateChat
{
    public sealed record Command(List<Guid> users, string chatName) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProvider _userProvider;

        public Handler(IUnitOfWork unitOfWork, IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistsAsync(_userProvider.Id, cancellationToken))
                throw new Exception("User not found");
            
            Guid chatId = Guid.NewGuid();
            
            var chat = new Domain.Entities.Chat
            {
                ChatName = request.chatName,
                PhotoPath = "",
                Id = chatId,
                ChatUsers = new List<ChatUser>(),
            };
            
            request.users.Add(_userProvider.Id);
            
            await _unitOfWork.Chats.AddAsync(chat, cancellationToken);
            foreach (var userId in request.users)
            {
                if(!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
                    continue;
                chat.ChatUsers.Add(new ChatUser
                {
                    ChatId = chatId,
                    UserId = userId
                });                
            }

            await _unitOfWork.Messages.AddAsync(new Message()
            {
                ChatId = chatId,
                Content = $"Chat {request.chatName} created",
                SenderId = _userProvider.Id
            });
            
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

            }
        }
    }
}