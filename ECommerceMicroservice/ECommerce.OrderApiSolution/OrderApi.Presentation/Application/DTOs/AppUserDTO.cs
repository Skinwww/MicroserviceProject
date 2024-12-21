using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public class AppUserDTO
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required, RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Номер телефона должен содержать от 10 до 15 цифр и может начинаться с +.")] public string TelephoneNumber { get; set; }
        [Required] public string Address { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required, RegularExpression("^(Admin|User)$", ErrorMessage = "Роль должна быть 'Admin' или 'User'.")] public string Role { get; set; }

        public AppUserDTO(int id, string name, string telephoneNumber, string address, string email, string password, string role)
        {
            Id = id;
            Name = name;
            TelephoneNumber = telephoneNumber;
            Address = address;
            Email = email;
            Password = password;
            Role = role;
        }
    };
}
