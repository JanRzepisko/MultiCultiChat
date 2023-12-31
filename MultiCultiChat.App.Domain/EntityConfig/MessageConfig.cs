﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiCultiChat.App.Domain.Entities;

namespace MultiCultiChat.App.Domain.EntityConfig;

public class MessageConfig: IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(c => c.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(c => c.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}