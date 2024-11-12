using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using swp_be.Data.Repositories;

namespace swp_be.Services
{
    public class KoiInventoryService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly KoiInventoryRepository koiInventoryrepository;
        private readonly ConsignmentKoiRepository consignmentKoiRepository;
        private readonly ConsignmentKoiService consignmentKoiService;

        public KoiInventoryService(ApplicationDBContext context)
        {
            unitOfWork = new UnitOfWork(context);
            koiInventoryrepository = new KoiInventoryRepository(context);
            consignmentKoiRepository = new ConsignmentKoiRepository(context);
            consignmentKoiService = new ConsignmentKoiService(context);
        }

        public async Task<List<KoiInventory>> GetKoiInventoryAsync()
        {
            return await koiInventoryrepository.GetIKois();
        }

        public async Task<KoiInventory> GetKoiInventoryByIdAsync(int id)
        {
            return await unitOfWork.KoiInventoryRepository.GetByIdAsync(id);
        }

        public async Task<List<KoiInventory>> GetKoiInventoryByUserIdAsync (int userId)
        {
            return await unitOfWork.KoiInventoryRepository.GetKoiInventoryByUserId(userId);
        }

        public async Task<KoiInventory> CreateKoiInventoryFromKoiAsync(int koiId,int customerID, bool isConsignment)
        {
            KoiInventory koiInventory = new KoiInventory();

            if (isConsignment)
            {
                var consignmentKoi = await  consignmentKoiService.GetById(koiId);
                if (consignmentKoi == null) throw new Exception("Consignment Koi not found.");

                // Chuyển thông tin từ ConsignmentKoi sang KoiInventory
                koiInventory.Name = consignmentKoi.Name;
                koiInventory.Gender = consignmentKoi.Gender;
                koiInventory.Age = consignmentKoi.Age;
                koiInventory.Size = consignmentKoi.Size;
                koiInventory.Color = consignmentKoi.Color;
                koiInventory.DailyFeedAmount = consignmentKoi.DailyFeedAmount;
                koiInventory.Price = consignmentKoi.Price;
                koiInventory.Personality = consignmentKoi.Personality;
                koiInventory.Origin = consignmentKoi.Origin;
                koiInventory.SelectionRate = consignmentKoi.SelectionRate;
                koiInventory.Species = consignmentKoi.Species;
                koiInventory.Image = consignmentKoi.Image;
                koiInventory.AddOnId = consignmentKoi.AddOnId;
                koiInventory.CustomerID = customerID;
                koiInventory.StartDate = consignmentKoi.Consignment.StartDate;
                koiInventory.EndDate = consignmentKoi.Consignment.EndDate;
                koiInventory.FosterPrice = consignmentKoi.Consignment.FosterPrice;
                if (consignmentKoi.Consignment.Type == 0)
                {
                    koiInventory.Status = InvenKoiStatus.Sold;
                } else 
                {
                    koiInventory.Status = InvenKoiStatus.Foster;
                }
            }
            else
            {
                var koi = await unitOfWork.KoiRepository.GetByIdAsync(koiId);
                if (koi == null) throw new Exception("Koi not found.");

                // Chuyển thông tin từ Koi sang KoiInventory
                koiInventory.Name = koi.Name;
                koiInventory.Gender = koi.Gender;
                koiInventory.Age = koi.Age;
                koiInventory.Size = koi.Size;
                koiInventory.Color = koi.Color;
                koiInventory.DailyFeedAmount = koi.DailyFeedAmount;
                koiInventory.Price = koi.Price;
                koiInventory.Personality = koi.Personality;
                koiInventory.Origin = koi.Origin;
                koiInventory.SelectionRate = koi.SelectionRate;
                koiInventory.Species = koi.Species;
                koiInventory.Image = koi.Image;
                koiInventory.AddOnId = koi.AddOnId;
                koiInventory.CustomerID = customerID;
                koiInventory.Status = InvenKoiStatus.Bought;
            }

            unitOfWork.KoiInventoryRepository.Create(koiInventory);
            unitOfWork.Save();

            return koiInventory;
        }

        public async Task<KoiInventory> UpdateKoiInventoryAsync(KoiInventory koiInventory)
        {
            unitOfWork.KoiInventoryRepository.Update(koiInventory);
            unitOfWork.Save();
            return koiInventory;
        }

        public async Task DeleteKoiInventoryAsync(KoiInventory koiInventory)
        {
            unitOfWork.KoiInventoryRepository.Remove(koiInventory);
            unitOfWork.Save();
        }
    }
}
