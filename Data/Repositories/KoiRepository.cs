using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Models;


namespace swp_be.Data.Repositories
{
    public class KoiRepository: GenericRepository<Koi> 
    {
        public KoiRepository(ApplicationDBContext context) : base(context)
        {
        }

    }
}
