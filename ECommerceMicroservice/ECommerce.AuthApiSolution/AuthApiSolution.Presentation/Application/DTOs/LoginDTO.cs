﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApiSolution.Application.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}