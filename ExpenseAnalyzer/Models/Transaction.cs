using System.ComponentModel.DataAnnotations;

namespace ExpenseAnalyzer.Models
{
    public abstract class Transaction
    {
        [Required]
        public string FileName { get; set; } = string.Empty;
    }
}
