using System.Drawing;
using App.DTO.Product;
using App.Helpers;
using App.Interface;
using App.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace App.ReviewRepository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateAsync(int product_id, string user_id, string content)
        {
            var review = new Review
            {
                CreateDate = DateTime.Now,
                UserId = user_id,
                ProductID = product_id,
                Content = content,
                Like = 0
            };
            await _context.Review.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        Task<Review> IReviewRepository.DeledeAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Review>> IReviewRepository.GetAllProduct(int id)
        {
            var reviews = _context.Review.Include(o=>o.User).Where(o => o.ProductID == id).ToListAsync();
            return reviews;
        }

        Task<List<Review>> IReviewRepository.GetAlltAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<Review>> IReviewRepository.GetAll_UserAsync(string id)
        {
            var reviews = _context.Review.Include(o=>o.User).Where(o => o.UserId == id).ToListAsync();
            return reviews;
        }

        public async Task like(string id, string user_id)
        {
            var resutl = await _context.Review.FirstOrDefaultAsync(o => o.Id == id);
            if (resutl.user_liked.Count() == 0)
            {
                resutl.Like += 1;
                resutl.user_liked.Add(user_id);
            }
            else
            {
                bool check = true;
                foreach (var item in resutl.user_liked)
                { if (item == user_id) check = false; }

                if (check)
                {
                    resutl.Like += 1;
                    resutl.user_liked.Add(user_id);
                }
                else return;
            }
            await _context.SaveChangesAsync();
        }
    }


}