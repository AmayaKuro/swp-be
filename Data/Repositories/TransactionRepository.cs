using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Models;


namespace swp_be.Data.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction> 
    {
        public TransactionRepository(ApplicationDBContext context) : base(context)
        {
        }

        public Transaction GetTransactionByID(int id)
        {
            return _context.Transactions
                .Include(t => t.Order)
                .Include(t => t.Consignment)
                .Include(t => t.PaymentMethod)
                .FirstOrDefault(u => u.TransactionID == id);
        }
    }
}
