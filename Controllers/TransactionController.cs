using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using swp_be.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        TransactionService transactionService;
        OrderService orderService;

        public TransactionController(ApplicationDBContext context)
        {
            _context = context;
            transactionService = new TransactionService(context);
            orderService = new OrderService(context);
        }

        // GET: api/Transactions
        [HttpGet]
        [Authorize("all")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        [Authorize("all")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("all")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("all")]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionID }, transaction);
        }

        [HttpPost]
        [Route("createOffTransaction")]
        [Authorize("staff, admin")]
        public async Task<ActionResult<Transaction>> CreateOffTransaction([FromBody] int orderID)
        {
            var order = await transactionService.CreateOffTransaction(orderID);

            // Kiểm tra kết quả từ service
            if (order == null)
            {
                return NotFound("Order not found");
            }

            // Cập nhật trạng thái của order
            transactionService.UpdateStatus(order, TransactionStatus.Completed);

            // Trả về transaction đã tạo
            return CreatedAtAction("GetTransaction", new { id = order.TransactionID }, order);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionID == id);
        }

        [HttpGet]
        [Route("vnpay_return")]
        public async Task<ActionResult> VnpayTransaction_return()
        {
            // Query will be retrieved all as once
            IQueryCollection queries = HttpContext.Request.Query;
            var feRedirectQuery = new Dictionary<string, string>();

            Console.WriteLine("Begin VNPAY Return, URL={0}", queries);
            if (queries.Count > 0)
            {
                string vnp_HashSecret = Configuration.GetConfiguration()["VNPay:vnp_HashSecret"]; //Chuoi bi mat
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (var query in queries)
                {
                    Console.WriteLine($"{query.Key}: {query.Value}.");
                    //get all querystring data
                    if (!string.IsNullOrEmpty(query.Key) && query.Key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(query.Key, query.Value);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                int transactionID = Convert.ToInt32(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = queries["vnp_SecureHash"];
                string TerminalID = queries["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                string bankCode = queries["vnp_BankCode"];

                // Pre-add to FE redirect URL
                feRedirectQuery.Add("transactionID", transactionID.ToString());
                feRedirectQuery.Add("amount", vnp_Amount.ToString());
                feRedirectQuery.Add("bankCode", vnp_ResponseCode);
                feRedirectQuery.Add("bank", vnpayTranId.ToString());
                // "00" for success, other for error
                feRedirectQuery.Add("status_code", vnp_ResponseCode);

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    Transaction transaction = transactionService.GetTransactionByID(transactionID);

                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        transactionService.UpdateStatus(transaction, TransactionStatus.Completed, vnpayTranId.ToString());

                        feRedirectQuery.Add("status_text", "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ");
                        Console.WriteLine("Thanh toan thanh cong, TransactionID={0}, VNPAY TranId={1}", transactionID, vnpayTranId);
                    }
                    // User cancel order
                    else if (vnp_ResponseCode == "24")
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        transactionService.UpdateStatus(transaction, TransactionStatus.Cancelled, vnpayTranId.ToString());

                        feRedirectQuery.Add("status_text", "Giao dịch bị hủy bởi người dùng");
                        Console.WriteLine("Thanh toan khong thanh cong, nguoi dung huy Transaction, TransactionID={0}, VNPAY TranId={1},ResponseCode={2}", transactionID, vnpayTranId, vnp_ResponseCode);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        transactionService.UpdateStatus(transaction, TransactionStatus.Failed, vnpayTranId.ToString());

                        feRedirectQuery.Add("status_text", "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode);
                        Console.WriteLine("Thanh toan loi, TransactionID={0}, VNPAY TranId={1},ResponseCode={2}", transactionID, vnpayTranId, vnp_ResponseCode);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid signature, InputData={0}", queries);
                    feRedirectQuery.Add("status_text", "Có lỗi xảy ra trong quá trình xử lý");
                }
            }

            // Build params for FE redirection
            string feQueryString = "?";

            foreach (var param in feRedirectQuery)
            {
                if (!string.IsNullOrEmpty(param.Key))
                {
                    feQueryString += param.Key + "=" + System.Net.WebUtility.UrlEncode(param.Value) + "&";
                }
            }

            // Redirect to FE /paymentSuccess if success (status_code == 00) else redirect to /paymentFail
            return Redirect(Configuration.GetConfiguration()["FEUrl"] + feRedirectQuery["status_code"] != "00" ? "/paymentFail" : "/paymentSuccess" + feQueryString);
        }
    }
}