using EShop.DAL.Entities;
using EShop.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }
        public async Task AddReviewAsync(int productId, int customerId, int rating, string comment)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            var hasPurchased = orders.Any(o =>
                o.Status == DAL.Enums.OrderStatus.Completed &&
                o.OrderDetails.Any(od => od.ProductId == productId));

            if (!hasPurchased)
            {
                throw new InvalidOperationException("Bạn chưa mua sản phẩm này hoặc đơn hàng chưa hoàn tất nên không thể đánh giá.");
            }

            var review = new Review
            {
                ProductId = productId,
                CustomerId = customerId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now,
                IsVisible = true
            };
            await _reviewRepository.AddReviewAsync(review);
        }

        public async Task<List<Review>> GetProductReviewsAsync(int productId)
        {
            return await _reviewRepository.GetReviewsByProductIdAsync(productId);
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _reviewRepository.DeleteReviewAsync(id);
        }

        public async Task<List<Review>> GetReviewsBySellerAsync(int sellerId)
        {
            return await _reviewRepository.GetReviewsBySellerAsync(sellerId);
        }

        public async Task ReplyReviewAsync(int reviewId, string reply)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);

            if (review != null)
            {
                review.SellerReply = reply;
                review.ReplyDate = DateTime.Now;

                await _reviewRepository.UpdateReviewAsync(review);
            }
        }
    }
}