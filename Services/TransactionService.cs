using swp_be.Controllers;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;
using swp_be.Utils;

namespace swp_be.Services
{
    public class TransactionService
    {
        private readonly ApplicationDBContext _context;
        private TransactionRepository transactionRepository;
        private DeliveryService deliveryService;
        private OrderService orderService;
        private GenericRepository<PaymentMethod> paymentMethodRepository;

        public TransactionService(ApplicationDBContext context)
        {
            _context = context;
            transactionRepository = new TransactionRepository(context);
            deliveryService = new DeliveryService(context);
            orderService = new OrderService(context);
            paymentMethodRepository = new GenericRepository<PaymentMethod>(context);
        }

        /// <summary>
        ///     Return VNPay redirect URL 
        /// </summary>
        public string CreateVNPayTransaction(Order order, string clientIPAddr)
        {

            if (order.Promotion != null && order.PromotionID.HasValue)
            {
                var promotion = order.Promotion;


                if (promotion.StartDate <= DateTime.Now &&
                    (promotion.EndDate == null || promotion.EndDate >= DateTime.Now))
                {
                    decimal discountAmount = order.TotalAmount * (promotion.DiscountRate / 100m);
                    order.TotalAmount -= (long)discountAmount;
                }
                else
                {
                    throw new InvalidOperationException("Promotion code is invalid or expired.");
                }
            }

            Transaction transaction = new Transaction();

            transaction.OrderID = order.OrderID;
            if (order.Type == OrderType.Online)
            {
                transaction.Amount = order.TotalAmount;
            }
            else
            {
                transaction.Amount = order.TotalAmount /= 2;
            }

            transaction.CreateAt = DateTime.Now;
            transaction.Type = TransactionType.Shopping;
            transaction.Status = TransactionStatus.Pending;
            // Always online regardless of payment method
            transaction.PaymentMethod = paymentMethodRepository.GetAll().Find(info => info.MethodName == "VNPay");

            transactionRepository.Create(transaction);
            transactionRepository.Save();

            // Begin vnpay flow
            string vnp_Returnurl = Configuration.GetConfiguration()["VNPay:vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = Configuration.GetConfiguration()["VNPay:vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = Configuration.GetConfiguration()["VNPay:vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = Configuration.GetConfiguration()["VNPay:vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            // Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự
            // tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần
            // nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_Amount", ((long)(order.TotalAmount * 100)).ToString());
            // Mã ngân hàng thanh toán. Ví dụ: VNPAYQR, VNBANK, INTCARD
            //if (bankcode_Vnpayqr.Checked == true)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            //}
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");

            vnpay.AddRequestData("vnp_CreateDate", transaction.CreateAt.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", clientIPAddr);

            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo",
                (order.Type == OrderType.Online
                ? "Thanh toan don hang: "
                : "Dat coc don hang: ")
                + order.OrderID);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss"));
            // transactionID got auto filled when save()
            vnpay.AddRequestData("vnp_TxnRef", transaction.TransactionID.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing
            string url = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return url;
        }

        public string CreateVNPayTransaction(Consignment consignment, string clientIPAddr)
        {
            Transaction transaction = new Transaction();

            transaction.ConsignmentID = consignment.ConsignmentID;
            transaction.Amount = consignment.FosterPrice;
            transaction.CreateAt = DateTime.Now;
            transaction.Type = TransactionType.Consignment;
            transaction.Status = TransactionStatus.Pending;
            // Always online regardless of payment method
            transaction.PaymentMethod = paymentMethodRepository.GetAll().Find(info => info.MethodName == "VNPay");

            transactionRepository.Create(transaction);
            transactionRepository.Save();

            // Begin vnpay flow
            string vnp_Returnurl = Configuration.GetConfiguration()["VNPay:vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = Configuration.GetConfiguration()["VNPay:vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = Configuration.GetConfiguration()["VNPay:vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = Configuration.GetConfiguration()["VNPay:vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            // Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự
            // tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần
            // nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_Amount", ((long)(consignment.FosterPrice * 100)).ToString());
            // Mã ngân hàng thanh toán. Ví dụ: VNPAYQR, VNBANK, INTCARD
            //if (bankcode_Vnpayqr.Checked == true)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            //}
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");

            vnpay.AddRequestData("vnp_CreateDate", transaction.CreateAt.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", clientIPAddr);

            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo",
                (consignment.Type == ConsignmentType.Sell
                ? "Thanh toan ky gui: "
                : " Don hang: ")
                + consignment.ConsignmentID);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss"));
            // transactionID got auto filled when save()
            vnpay.AddRequestData("vnp_TxnRef", transaction.TransactionID.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing
            string url = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return url;
        }

        public async Task<Transaction?> CreateOffTransaction(int orderID)
        {
            Transaction transaction = new Transaction();

            var order = orderService.GetByID(orderID);
            var paymentMethod = paymentMethodRepository.GetAll().Find(info => info.MethodName == "Cash");

            if (order == null)
            {
                return null;
            }

            long depositAmount = order.TotalAmount / 2;

            // Tạo Transaction mới
            transaction.OrderID = orderID;
            transaction.CreateAt = DateTime.Now;
            transaction.Status = TransactionStatus.Pending;
            transaction.Type = TransactionType.Shopping;
            transaction.Amount = depositAmount;
            transaction.CreateAt = DateTime.Now;
            transaction.EndAt = transaction.CreateAt.AddDays(1);
            transaction.PaymentMethod = paymentMethod;


            // Lưu Transaction vào repository
            await transactionRepository.CreateAsync(transaction);

            return transaction;
        }

        public Transaction GetTransactionByID(int id)
        {
            return transactionRepository.GetTransactionByID(id);
        }

        public void UpdateTransactionStatus(Transaction transaction)
        {
            transactionRepository.Update(transaction);
            transactionRepository.Save();
        }

        public void UpdateStatus(Transaction transaction, TransactionStatus transactionStatus, string token = "")
        {
            transaction.Status = transactionStatus;
            transaction.EndAt = DateTime.Now;

            transaction.Token = token;

            // Update order status

            // If success
            if (transactionStatus == TransactionStatus.Completed)
            {
                // Update order status
                if (transaction.Type == TransactionType.Shopping)
                {
                    Order order = transaction.Order;

                    // Final payment for Online order, auto create delivery
                    if (order.Type == OrderType.Online)
                    {
                        deliveryService.CreateDeliveryFromOrder(order);
                    }
                    // Final payment for offline order (cash), order is completed. Optional delivery will be created by staff (CRUD Delivery site)
                    else if (transaction.PaymentMethod.MethodName == "Cash"
                        && order.Type == OrderType.Offline)
                    {
                        orderService.FinishOrder(order.OrderID);
                    }
                }

                // Update consignment status
                else if (transaction.Type == TransactionType.Consignment)
                {
                    Consignment consignment = transaction.Consignment;

                    // If consignment foster fee is paid, set status to available for sell or raising for foster
                    if (transactionStatus == TransactionStatus.Completed && consignment.Status == ConsignmentStatus.pending)
                    {
                        consignment.Status = (consignment.Type == ConsignmentType.Sell)
                            ? ConsignmentStatus.available
                            : ConsignmentStatus.raising;
                    }
                }
            }
            else if (transactionStatus == TransactionStatus.Cancelled)
            {
                transaction.Order.Status = OrderStatus.Cancelled;
                transaction.EndAt = DateTime.Now;

                if (transaction.Type == TransactionType.Shopping)
                {
                    orderService.CancelOrder(transaction.OrderID.Value);
                } else
                {
                    Consignment consignment = transaction.Consignment;
                    consignment.Status = ConsignmentStatus.pending;
                }
            }

            transactionRepository.Update(transaction);
            transactionRepository.Save();
        }
    }
}