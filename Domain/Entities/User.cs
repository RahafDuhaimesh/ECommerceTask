namespace ECommerceTask.Domain.Entities
{
    public class User
    {

            public int Id { get; set; }
            public required string FullName { get; set; }
            public required string Email { get; set; }
            public required string Username { get; set; }
            public  string Role { get; set; } = "User";

             public byte[]? PasswordHash { get; set; }

            public byte[]? PasswordSalt { get; set; }

            public string? Password { get; set; }
            public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();


    }
}


