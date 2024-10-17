using swp_be.Controllers;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Models;
using swp_be.Utils;

namespace swp_be.Services
{
    public class TransactionService
    {
        private readonly ApplicationDBContext _context;
        private GenericRepository<Transaction> transactionRepository;

        public TransactionService(ApplicationDBContext context)
        {
            _context = context;
            transactionRepository = new GenericRepository<Transaction>(context);

        }

        /// <summary>
        ///     Return VNPay redirect URL 
        /// </summary>
        public string CreateVNPayTransaction(Order order, string clientIPAddr)
        {
            Transaction transaction = new Transaction();

            transaction.OrderID = order.OrderID;
            transaction.Amount = order.TotalAmount;
            transaction.CreateAt = DateTime.Now;
            transaction.Type = TransactionType.Shopping;
            transaction.Status = TransactionStatus.Pending;
            // Always online regardless of payment method
            transaction.PaymentMethodID = 1;

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
            vnpay.AddRequestData("vnp_Amount", ((int)(order.TotalAmount * 100)).ToString());
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

            Console.WriteLine(url);
            return url;
        }
    }
}
