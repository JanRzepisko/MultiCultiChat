using Shared.BaseModels.BaseEntities;

namespace Signal.App.Domain.Entities;

public class User : Entity
{
   public string Username { get; set; }
   public string Email { get; set; }
   public string Password { get; set; }
   public ICollection<ChatUser> ChatUsers { get; set; }
   public ICollection<Message> Messages { get; set; }
}