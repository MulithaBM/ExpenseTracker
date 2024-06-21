using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseAnalyzer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOC_CC_Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InternetBankingReferenceNumber = table.Column<string>(type: "text", nullable: false),
                    HostReferenceNumber = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TransactionTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOC_CC_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnAccountTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OnlineBankingReferenceNumber = table.Column<string>(type: "text", nullable: false),
                    HostReferenceNumber = table.Column<string>(type: "text", nullable: false),
                    DebitAccountNumber = table.Column<string>(type: "text", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    CreditAccountNumber = table.Column<string>(type: "text", nullable: false),
                    TransferAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    FundsTransferMethod = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TransactionTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnAccountTransfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOC_CC_Payments_InternetBankingReferenceNumber_HostReferenc~",
                table: "BOC_CC_Payments",
                columns: new[] { "InternetBankingReferenceNumber", "HostReferenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnAccountTransfers_OnlineBankingReferenceNumber_HostRefere~",
                table: "OwnAccountTransfers",
                columns: new[] { "OnlineBankingReferenceNumber", "HostReferenceNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOC_CC_Payments");

            migrationBuilder.DropTable(
                name: "OwnAccountTransfers");
        }
    }
}
