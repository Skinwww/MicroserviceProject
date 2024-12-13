using System.ComponentModel.DataAnnotations;
using ProductApi.Presentation.Domain.Entities;
namespace ProductApi.Application.DTOs
{
    public record ProductDTO
        (
        [Required] int Id,
        [Required] string Name,
        [Required, Range (1, int.MaxValue)] int Quantity,
        [Required, DataType(DataType.Currency)] decimal Price,
        [Required] string Type,
        [Required] string Volume
        );
}
