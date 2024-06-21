using ExpenseAnalyzer.Application;
using ExpenseAnalyzer.Data;
using ExpenseAnalyzer.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAnalyzer
{
    internal class Analyzer
    {
        private readonly DataContext _context;
        private readonly string _paymentsFolder;

        internal Analyzer(DataContext context)
        {
            _context = context;
            _paymentsFolder = "D:\\Data\\Expenses - Test\\Own Account Transfers"; // Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
        }

        internal async Task UpdateDatabase()
        {
            HashSet<string> existingTransactions = await ExistingTransactionsInDBAsync();
            // List<string> newFiles = GetNewFiles(existingTransactions);
            List<string> newFiles = GetNewFiles();

            if (newFiles.Count == 0)
            {
                Console.WriteLine("No new files found.");
            }
            else
            {
                List<Transaction> transactions = [];

                foreach (string file in newFiles)
                {
                    Transaction transaction = ExtractTransactionData(file);
                    transactions.Add(transaction);
                }

                Test(transactions);

                await SaveTransactionsAsync(transactions);
            }
        }

        private async Task<HashSet<string>> ExistingTransactionsInDBAsync()
        {
            HashSet<string> transactions = [];
            List<Transaction> existingTransactions = [];

            //try
            //{
            //    await _context.Transactions.ForEachAsync(t => transactions.Add(t.FileName));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //}

            return transactions;
        }

        private List<string> GetNewFiles(HashSet<string> existingTransactions)
        {
            List<string> newFiles = [];

            foreach (string file in Directory.EnumerateFiles(_paymentsFolder, "*.pdf"))
            {
                string fileName = System.IO.Path.GetFileName(file);

                if (!existingTransactions.Contains(fileName))
                {
                    newFiles.Add(file);
                }
            }

            return newFiles;
        }

        private List<string> GetNewFiles()
        {
            List<string> newFiles = [];

            newFiles.AddRange(Directory.EnumerateFiles(_paymentsFolder, "*.pdf", SearchOption.AllDirectories));

            Console.WriteLine($"Found {newFiles.Count} new files.");

            return newFiles;
        }

        private Transaction ExtractTransactionData(string filePath)
        {
            try
            {
                using var fs = File.OpenRead(filePath);

                PdfReader reader = new(fs);
                PdfDocument pdfDoc = new(reader);

                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                string transactionData = PdfTextExtractor.GetTextFromPage(pdfDoc.GetFirstPage(), strategy);

                Transaction transaction = TransactionParser.ParseData(transactionData);
                transaction.FileName = System.IO.Path.GetFileName(filePath);

                return transaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An error occurred while extracting transaction data: {ex.Message}");
            }
        }

        private void Test(List<Transaction> transactions)
        {
            Dictionary<string, string> d1 = new();
            Dictionary<string, string> d2 = new();

            try
            {
                foreach (Transaction transaction in transactions)
                {
                    if (transaction is OwnAccountTransfer transfer)
                    {
                        if (d1.TryGetValue(transfer.OnlineBankingReferenceNumber, out string? value1))
                        {
                            Console.WriteLine($"Duplicate transaction found: {value1}, {transfer.FileName}");
                        }
                        else
                        {
                            d1.Add(transfer.OnlineBankingReferenceNumber, transfer.FileName);
                        }

                        if (d2.TryGetValue(transfer.HostReferenceNumber, out string? value2))
                        {
                            Console.WriteLine($"Duplicate transaction found: {value2}, {transfer.FileName}");
                        }
                        else
                        {
                            d2.Add(transfer.HostReferenceNumber, transfer.FileName);
                        }
                    }
                    else if (transaction is BOC_CC_Payment payment)
                    {
                        if (d1.TryGetValue(payment.InternetBankingReferenceNumber, out string? value1))
                        {
                            Console.WriteLine($"Duplicate transaction found: {value1}, {payment.FileName}");
                        }
                        else
                        {
                            d1.Add(payment.InternetBankingReferenceNumber, payment.FileName);
                        }

                        if (d2.TryGetValue(payment.HostReferenceNumber, out string? value2))
                        {
                            Console.WriteLine($"Duplicate transaction found: {value2}, {payment.FileName}");
                        }
                        else
                        {
                            d2.Add(payment.HostReferenceNumber, payment.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task SaveTransactionsAsync(List<Transaction> transactions)
        {
            HashSet<string> OAT_OBRN = [];
            HashSet<string> OAT_HRN = [];
            HashSet<string> BOC_CC_IBRN = [];
            HashSet<string> BOC_CC_HRN = [];

            foreach (Transaction transaction in transactions)
            {
                if (transaction is OwnAccountTransfer transfer)
                {
                    if (OAT_OBRN.Count == 0) await _context.OwnAccountTransfers.ForEachAsync(t => OAT_OBRN.Add(t.OnlineBankingReferenceNumber));
                    if (OAT_HRN.Count == 0) await _context.OwnAccountTransfers.ForEachAsync(t => OAT_HRN.Add(t.HostReferenceNumber));

                    if (!OAT_OBRN.Contains(transfer.OnlineBankingReferenceNumber) && !OAT_HRN.Contains(transfer.HostReferenceNumber))
                    {
                        _context.OwnAccountTransfers.Add(transfer);
                    }
                }
                else if (transaction is BOC_CC_Payment payment)
                {
                    if (BOC_CC_IBRN.Count == 0) await _context.BOC_CC_Payments.ForEachAsync(t => BOC_CC_IBRN.Add(t.InternetBankingReferenceNumber));
                    if (BOC_CC_HRN.Count == 0) await _context.BOC_CC_Payments.ForEachAsync(t => BOC_CC_HRN.Add(t.HostReferenceNumber));

                    if (!BOC_CC_IBRN.Contains(payment.InternetBankingReferenceNumber) && !BOC_CC_HRN.Contains(payment.HostReferenceNumber))
                    {
                        _context.BOC_CC_Payments.Add(payment);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
