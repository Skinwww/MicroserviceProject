using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace AuthApiSolution.Application.DTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string TelephoneNumber { get; set; }
        [Required] public string Address { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Role { get; set; }

        public GetUserDTO(int id, string name, string telephoneNumber, string address, string email, string role)
        {
            Id = id;
            Name = name;
            TelephoneNumber = telephoneNumber;
            Address = address;
            Email = email;
            Role = role;
        }
    }
}
