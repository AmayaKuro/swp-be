using swp_be.data.Repositories;
using swp_be.Data.Repositories;
using swp_be.Models;


namespace swp_be.Data
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDBContext _context;
        private KoiRepository _koiRepository;
        private UserRepository _userRepository;
        private TokenRepository _tokenRepository;
        private BatchRepository _batchRepository;
        private OrderRepository _orderRepository;
        private ConsignmentRepository _consignmentRepository;
        private ConsignmentKoiRepository _ConsignmentKoiRepository;
        private GenericRepository<ConsignmentPriceList> _consignmentPriceListRepository;
        private GenericRepository<Staff> _staffRepository;
        private GenericRepository<FosterBatch> _fosterBatchRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<PaymentMethod> _paymentMethodRepository;
        private FeedbackRepository _feedbackRepository;
        private DeliveryRepository _deliveryRepository;
        private BlogRepository _blogRepository;
        private PromotionRepository _promotionRepository;


        private bool disposed = false;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            _koiRepository = new KoiRepository(_context);
            _userRepository = new UserRepository(_context);
            _tokenRepository = new TokenRepository(_context);
            _batchRepository = new BatchRepository(_context);
            _orderRepository = new OrderRepository(_context);
            _consignmentRepository = new ConsignmentRepository(_context);
            _ConsignmentKoiRepository = new ConsignmentKoiRepository(_context);
            _staffRepository = new GenericRepository<Staff>(_context);
            _fosterBatchRepository = new GenericRepository<FosterBatch>(_context);
            _customerRepository = new GenericRepository<Customer>(_context);
            _paymentMethodRepository = new GenericRepository<PaymentMethod>(_context);
            _feedbackRepository = new FeedbackRepository(_context);
            _deliveryRepository = new DeliveryRepository(_context);
            _blogRepository = new BlogRepository(_context);
            _promotionRepository = new PromotionRepository(_context);
            _consignmentPriceListRepository = new GenericRepository<ConsignmentPriceList>(_context);


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

        public BatchRepository BatchRepository
        {
            get
            {
                return _batchRepository;
            }
        }

        public PromotionRepository PromotionRepository
        {
            get
            {
                return _promotionRepository;
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                return _orderRepository;
            }
        }

        public ConsignmentRepository ConsignmentRepository
        {
            get
            {
                return _consignmentRepository;
            }
        }

        public ConsignmentKoiRepository ConsignmentKoiRepository
        {
            get
            {
                return _ConsignmentKoiRepository;
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

        public FeedbackRepository FeedbackRepository
        {
            get
            {
                return _feedbackRepository;
            }
        }

        public DeliveryRepository DeliverRepository
        {
            get
            {
                return _deliveryRepository;
            }
        }
        public BlogRepository BlogRepository
        {
            get
            {
                return _blogRepository;

            }
        }

        public GenericRepository<ConsignmentPriceList> ConsignmentPriceListRepository
        {
            get
            {
                return _consignmentPriceListRepository;
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
