using System.ComponentModel.DataAnnotations;

namespace ExpenseAnalyzer.Models
{
    internal class BOC_CC_Payment : Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string InternetBankingReferenceNumber { get; set; } = string.Empty;

        [Required]
        public string HostReferenceNumber { get; set; } = string.Empty;

        [Required]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = string.Empty;

        public string? Status { get; set; }

        [Required]
        public DateOnly TransactionDate { get; set; }

        [Required]
        public TimeOnly TransactionTime { get; set; }
    }
}
