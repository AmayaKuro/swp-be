using swp_be.data.Repositories;
using swp_be.Data.Repositories;
using swp_be.Models;
using YourNamespace.Models;

namespace swp_be.Data
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDBContext _context;
        private KoiRepository _koiRepository;
        private UserRepository _userRepository;
        private TokenRepository _tokenRepository;
        private GenericRepository<Batch> _batchRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<Consignment> _consignmentRepository;
        private GenericRepository<FosterKoi> _fosterKoiRepository;
        private GenericRepository<Staff> _staffRepository;
        private GenericRepository<FosterBatch> _fosterBatchRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<PaymentMethod> _paymentMethodRepository;
        private GenericRepository<Feedback> _feedbackRepository;
        private GenericRepository<Blog> _blogRepository;

        private bool disposed = false;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            _koiRepository = new KoiRepository(_context);
            _userRepository = new UserRepository(_context);
            _tokenRepository = new TokenRepository(_context);
            _batchRepository = new GenericRepository<Batch>(_context);
            _orderRepository = new GenericRepository<Order>(_context);
            _consignmentRepository = new GenericRepository<Consignment>(_context);
            _fosterKoiRepository = new GenericRepository<FosterKoi>(_context);
            _staffRepository = new GenericRepository<Staff>(_context);
            _fosterBatchRepository = new GenericRepository<FosterBatch>(_context);
            _customerRepository = new GenericRepository<Customer>(_context);
            _paymentMethodRepository = new GenericRepository<PaymentMethod>(_context);
            _feedbackRepository = new GenericRepository<Feedback>(_context);
            _blogRepository =new GenericRepository<Blog>(_context); 
        }

        public KoiRepository KoiRepository
        {
            get
            {
                return _koiRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public TokenRepository TokenRepository
        {
            get
            {
                return _tokenRepository;
            }
        }

        public GenericRepository<Batch> BatchRepository
        {
            get
            {
                return _batchRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                return _orderRepository;
            }
        }

        public GenericRepository<Consignment> ConsignmentRepository
        {
            get
            {
                return _consignmentRepository;
            }
        }

        public GenericRepository<FosterKoi> FosterKoiRepository
        {
            get
            {
                return _fosterKoiRepository;
            }
        }

        public GenericRepository<Staff> StaffRepository
        {
            get
            {
                return _staffRepository;
            }
        }

        public GenericRepository<FosterBatch> FosterBatchRepository
        {
            get
            {
                return _fosterBatchRepository;
            }
        }

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                return _customerRepository;
            }
        }

        public GenericRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                return _paymentMethodRepository;
            }
        }

        public GenericRepository<Feedback> FeedbackRepository
        {
            get
            {
                return _feedbackRepository;
            }
        }
        public GenericRepository<Blog> BlogRepository
        {
            get
            {
                return _blogRepository;
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
