using swp_be.Data.Repositories;
using swp_be.Services;

namespace swp_be.Data
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDBContext _context;
        private KoiRepository _koiRepository;

        private bool disposed = false;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            _koiRepository = new KoiRepository(_context);
        }

        public KoiRepository KoiRepository
        {
            get
            {
                return _koiRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
