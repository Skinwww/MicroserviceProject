using ECommercelib.SharedLibrary.Logs;
using ECommercelib.SharedLibrary.Responses;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Presentation.Domain.Entities;
using System.Linq.Expressions;
using Response = ECommercelib.SharedLibrary.Responses.Response;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context, IPublishEndpoint publishEndpoint) : IProduct
    {
        public async Task<Response> CreateAsync(Product _product)
        {
           try
            {
                var getProduct = await GetByAsync(_ => _.Name!.Equals(_product.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{_product.Name} уже добавлен");
               
                var currentEntity = context.Products.Add(_product).Entity;
                await context.SaveChangesAsync();
                await publishEndpoint.Publish(_product);
                if(currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{_product.Name} успешно добвлен в базу данных");
                else
                    return new Response(false, $"Возникла ошибка при добавлении {_product.Name}");
            }
            catch(Exception ex) 
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Возникла ошибка при добавлении нового продукта");
            }
        }

        public async Task<Response> DeleteAsync(int productId) // Принимаем только ID
        {
            try
            {
                var product = await FindByIdAsync(productId);
                if (product is null)
                    return new Response(false, $"Продукт с ID {productId} не найден");

                context.Products.Remove(product); 
                await context.SaveChangesAsync();
                return new Response(true, $"Продукт с ID {productId} удален успешно"); 
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при удалении");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
              var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Произошла ошибка при получении продукта");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (context == null)
            {
                throw new InvalidOperationException("Контекст базы данных не инициализирован.");
            }

            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products ?? Enumerable.Empty<Product>();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                throw new InvalidOperationException("Произошла ошибка при получении продукта", ex); 
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new NotImplementedException("Произошла ошибка при получении продукта");
            }

        }

        public async Task<Response> UpdateAsync(Product _product)
        {
            try
            {
                var product = await FindByIdAsync(_product.Id);
                if (product is null)
                    return new Response(false, $"{_product.Name} не найден");

                // Обновляем поля существующего продукта
                product.Name = _product.Name;
                product.Quantity = _product.Quantity;
                product.Price = _product.Price;
                product.Type = _product.Type;
                product.Volume = _product.Volume;

                // Сохраняем изменения
                await context.SaveChangesAsync();
                return new Response(true, $"{_product.Name} обновлен успешно");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Произошла ошибка при получении продукта");
            }
        }
    }
}
