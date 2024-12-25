using App.Models;

namespace App.Interface
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAll_UserAsync(string id);
        Task<List<Review>> GetAlltAsync();
        Task<Review> CreateAsync(int product_id, string user_id, string content);
        Task like(string id, string user_id);
        Task<List<Review>> GetAllProduct(int id);
        Task<Review> DeledeAsync(int id);
    }

}