
using App.Models;

namespace App.Interface
{
    public interface IAddressRepo
    {
        Task CreateAsync(string userName);

    }

}
