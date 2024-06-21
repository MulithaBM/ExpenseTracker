using System.ComponentModel.DataAnnotations;

namespace ExpenseAnalyzer.Models
{
    internal class OwnAccountTransfer : Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string OnlineBankingReferenceNumber { get; set; } = string.Empty;

        [Required]
        public string HostReferenceNumber { get; set; } = string.Empty;

        [Required]
        public string DebitAccountNumber { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string CreditAccountNumber { get; set; } = string.Empty;

        [Required]
        public decimal TransferAmount { get; set; }

        [Required]
        public string Currency { get; set; } = string.Empty;

        public string? FundsTransferMethod { get; set; }

        public string? Status { get; set; }

        [Required]
        public DateOnly TransactionDate { get; set; }

        [Required]
        public TimeOnly TransactionTime { get; set; }

        public string? Description { get; set; }
    }
}
