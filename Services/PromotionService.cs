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
    }
}
