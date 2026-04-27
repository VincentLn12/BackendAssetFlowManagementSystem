using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructure.Services;

public class CartService(StoreContext _context) : ICartService
{
    public async Task<bool> DeleteCartAsync(string key)
    {
        _context.CartStorages.RemoveRange(_context.CartStorages.Where(c => c.Id == key));
        var result = (await _context.SaveChangesAsync()) > 0;

        return result;
    }

    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _context.CartStorages
           .Where(c => c.Id == key)
           .Select(c => c.JsonData)
           .FirstOrDefaultAsync();

        return string.IsNullOrEmpty(data) ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var cartStorage = new CartStorage()
        {
            Id = cart.Id,
            JsonData = JsonSerializer.Serialize(cart)
        };

        var result = await _context.CartStorages.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(cart.Id));

        if (result != null)
        {
            _context.CartStorages.Update(cartStorage);
        }
        else
        {
            await _context.CartStorages.AddAsync(cartStorage);
        }

        var created = (await _context.SaveChangesAsync()) > 0;

        if (!created) return null;

        return await GetCartAsync(cart.Id);
    }
}