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

        public async Task<Promotion> RedeemPromotion(int customerId, int loyaltyPointsToRedeem)
        {
            var customer = await unitOfWork.CustomerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            if (customer.LoyaltyPoints < loyaltyPointsToRedeem)
            {
                throw new InvalidOperationException("Not enough Loyalty Points to redeem promotion");
            }

            decimal discountRate = 0;
            if(loyaltyPointsToRedeem==10)
            {
                discountRate = 10;
            } else if (loyaltyPointsToRedeem==50)
            { 
                discountRate = 15; 
            } else if (loyaltyPointsToRedeem == 100)
            {
                discountRate = 20;
            } else if (loyaltyPointsToRedeem ==200)
            {
                discountRate = 30;
            } else
            {
                throw new InvalidOperationException("Loyalty points are not valid for redeeming promotions.");
            }

            var promotion = new Promotion
            {
                Code = $"PROMO-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                Description = "'s Loyalty Points Promotion - " + discountRate + "% Discount",
                DiscountRate = discountRate,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1), 
                RemainingRedeem = 1,
                CustomerID = customerId
            };

            customer.LoyaltyPoints -= loyaltyPointsToRedeem;

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
