using swp_be.Models;
using swp_be.Data;
using swp_be.Data.Repositories;

namespace swp_be.Services
{
    public class PromotionService
    {
        private readonly UnitOfWork unitOfWork;

        public PromotionService(ApplicationDBContext context)
        {
            this.unitOfWork = new UnitOfWork(context);
        }

        public async Task<IEnumerable<Promotion>> GetPromotions()
        {
            return await unitOfWork.PromotionRepository.GetAllAsync();
        }

        public async Task<Promotion> CreatePromotion(Promotion promotion)
        {

            var existingPromotions = await unitOfWork.PromotionRepository
            .GetPromotionsByDateAsync(promotion.StartDate, promotion.EndDate);

            if (existingPromotions.Any())
            {
                foreach (var existingPromotion in existingPromotions)
                {
                    if (existingPromotion.DiscountRate == promotion.DiscountRate)
                    {
                        throw new InvalidOperationException("Promotion with the same date and discount rate already exists.");
                    }
                }
            }

            unitOfWork.PromotionRepository.Create(promotion);
            unitOfWork.Save();
            return promotion;
        }

        public async Task<Promotion> UpdatePromotion(Promotion promotion)
        {
            unitOfWork.PromotionRepository.Update(promotion);
            unitOfWork.Save();
            return promotion;
        }

        public async Task<Promotion> DeletePromotion(Promotion promotion)
        {
            unitOfWork.PromotionRepository.Remove(promotion);
            unitOfWork.Save();
            return promotion;
        }

        public async Task<IEnumerable<Promotion>> SearchPromotions(string? code, DateTime? startDate, DateTime? endDate)
        {
            var promotions = await unitOfWork.PromotionRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(code))
            {
                promotions = promotions.Where(p => p.Code != null && p.Code.Contains(code, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (startDate.HasValue)
            {
                promotions = promotions.Where(p => p.StartDate >= startDate).ToList();
            }

            if (endDate.HasValue)
            {
                promotions = promotions.Where(p => p.EndDate <= endDate || p.EndDate == null).ToList();
            }

            return promotions;
        }

        public async Task<Promotion> RedeemPromotion(int customerId)
        {
            // Lấy thông tin Customer
            var customer = await unitOfWork.CustomerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            // Kiểm tra điểm LoyaltyPoints
            if (customer.LoyaltyPoints < 10)
            {
                throw new InvalidOperationException("Not enough Loyalty Points to redeem promotion");
            }

            // Tạo Promotion mới
            var promotion = new Promotion
            {
                Code = $"PROMO-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                Description = "Loyalty Points Promotion - 10% Discount",
                DiscountRate = 10,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1), // Promotion có hạn sử dụng 1 tháng
                RemainingRedeem = 1,
                CustomerID = customerId
            };

            // Trừ điểm LoyaltyPoints
            customer.LoyaltyPoints -= 10;

            // Lưu thay đổi
            unitOfWork.PromotionRepository.Create(promotion);
            unitOfWork.CustomerRepository.Update(customer);
            unitOfWork.Save();

            return promotion;
        }

        public async Task<IEnumerable<Promotion>> GetUserAvailablePromotions(int customerId)
        {
            // Gọi trực tiếp repository
            return await unitOfWork.PromotionRepository.GetAvailablePromotionsAsync(customerId);
        }

    }
}
