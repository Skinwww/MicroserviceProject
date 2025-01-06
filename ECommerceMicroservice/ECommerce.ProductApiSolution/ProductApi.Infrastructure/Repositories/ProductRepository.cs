using ECommercelib.SharedLibrary.Logs;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Linq.Expressions; 

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
           try
            {
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} уже добавлен");
               
                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if(currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} успешно добвлен в базу данных");
                else
                    return new Response(false, $"Возникла ошибка при добавлении {entity.Name}");
            }
            catch(Exception ex) 
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Возникла ошибка при добавлении нового продукта");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} не найдет");
                context.Products.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} удален успешно");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Возникла ошбика при удалении");
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
                return products ?? Enumerable.Empty<Product>(); // или throw если никогда не null
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

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is not null)
                    return new Response(false, $"{entity.Name} не найден");
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                context.SaveChanges();
                return new Response(true, $"{entity.Name} обновлен успешно");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

               return new Response(false, "Произошла ошибка при получении продукта");
            }
        }
    }
}
