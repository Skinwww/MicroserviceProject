using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace AuthApiSolution.Application.DTOs
{
    public record GetUserDTO
    (
     int Id,
    [Required] string Name,
    [Required] string TelephoneNumber,
    [Required] string Address,
    [Required, EmailAddress] string Email,
    [Required] string Role

        );
}
