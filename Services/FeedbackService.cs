using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public class FeedbackService
    {
        private readonly UnitOfWork _unitOfWork;

        public FeedbackService(ApplicationDBContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacks()
        {
            return await _unitOfWork.FeedbackRepository.GetAllAsync();
        }

        public async Task<Feedback> GetFeedbackById(int id)
        {
            return await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
        }

        public async Task<Feedback> CreateFeedback(Feedback feedback)
        {
            var order = _unitOfWork.OrderRepository.GetById(feedback.OrderID);
            if (order == null || order.Status != OrderStatus.Completed)
            {
                return null;
            }

            _unitOfWork.FeedbackRepository.Create(feedback);
            _unitOfWork.Save();
            return feedback;
        }

        public async Task<Feedback> UpdateFeedback(Feedback feedback)
        {
            _unitOfWork.FeedbackRepository.Update(feedback);
            _unitOfWork.Save();
            return feedback;
        }

        public async Task<bool> DeleteFeedback(int id)
        {
            var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                return false;
            }

            _unitOfWork.FeedbackRepository.Remove(feedback);
            _unitOfWork.Save();
            return true;
        }

        public async Task<IEnumerable<Feedback>> SearchFeedbacks(int? rating, DateTime? dateFb)
        {
            var feedbacks = await GetFeedbacks();

            if (rating.HasValue)
            {
                feedbacks = feedbacks.Where(fb => fb.Rating == rating.Value);
            }

            if (dateFb.HasValue)
            {
                feedbacks = feedbacks.Where(fb => fb.DateFb.Date == dateFb.Value.Date);  
            }

            return feedbacks;
        }
    }
}
