
using App.DTO.Cart;
using App.Interface;
using App.Mappers;
using App.Models;
using Microsoft.EntityFrameworkCore;

public class AddressRepo : IAddressRepo
{
    private readonly AppDbContext _context;

    public AddressRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(string userName)
    {
        var user = await _context.Users
                        .Where(o => o.UserName == userName)
                        .FirstOrDefaultAsync();

        string id = user?.Id; // Use null-conditional operator to handle cases where the user might not be found
        var address = new Address { UserId = id };
        await _context.Address.AddAsync(address);
        user.AddressID = address.Id;
        await _context.SaveChangesAsync();
    }
}