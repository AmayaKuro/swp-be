using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    /// <summary>
    /// Represents the status of an Transaction.
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// The transaction is pending for payment confirmation from paymentMethod.
        /// </summary>
        Pending,

        /// <summary>
        /// The transaction is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The transaction failed to complete.
        /// </summary>
        /// TODO:this one might need consideration when having more information about the payment method
        Failed,

        /// <summary>
        /// The transaction is cancelled.
        /// </summary>
        Cancelled,
    }

    /// <summary>
    /// Represents the type of an Transaction.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// The transaction is for Consignment for fostering.
        /// </summary>
        Consignment,

        /// <summary>
        /// The transaction is for Shopping.
        /// </summary>
        Shopping,

        Offline,
    }

    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        public int? OrderID { get; set; }

        public int? ConsignmentID { get; set; }

        // Token from payment gateway (VNPay,...)
        public string? Token { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public int PaymentMethodID { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        public DateTime? EndAt{ get; set; }

        [Required]
        public long Amount { get; set; }

        [Required, MaxLength(50)]
        public TransactionStatus Status { get; set; }

        public Order Order { get; set; }
        public Consignment Consignment { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
