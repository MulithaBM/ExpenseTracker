using ExpenseAnalyzer.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAnalyzer.Data
{
    internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OwnAccountTransfer>()
                .HasIndex(p => new { p.OnlineBankingReferenceNumber, p.HostReferenceNumber })
                .IsUnique();

            modelBuilder.Entity<BOC_CC_Payment>()
                .HasIndex(p => new { p.InternetBankingReferenceNumber, p.HostReferenceNumber })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        internal DbSet<OwnAccountTransfer> OwnAccountTransfers { get; set; }
        internal DbSet<BOC_CC_Payment> BOC_CC_Payments { get; set; }
    }
}
