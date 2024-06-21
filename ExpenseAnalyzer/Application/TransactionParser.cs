using ExpenseAnalyzer.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ExpenseAnalyzer.Application
{
    internal static partial class TransactionParser
    {
        private static readonly Dictionary<string, Func<string, Transaction>> _parsers;

        static TransactionParser()
        {
            _parsers = new()
            {
                { "OwnAccountTransfer", ParseOwnAccountTransferData },
                { "BOC_CC_Payment", ParseBOC_CC_PaymentData }
            };
        }

        internal static Transaction ParseData(string transactionData)
        {
            string transactionType = "";

            if (OwnAccountTransferPattern().IsMatch(transactionData)) transactionType = "OwnAccountTransfer";
            else if (BOC_CC_Pattern().IsMatch(transactionData)) transactionType = "BOC_CC_Payment";

            if (_parsers.TryGetValue(transactionType, out Func<string, Transaction>? value))
            {
                return value(transactionData);
            }
            else
            {
                throw new Exception($"Transaction type '{transactionType}' is not supported.");
            }
        }

        private static OwnAccountTransfer ParseOwnAccountTransferData(string transactionData)
        {
            string onlineBankingRefPattern = @"online\s*banking\s*reference\s*number\s*[-:]?\s*(\d+)";
            string hostRefPattern = @"host\s*reference\s*number\s*[-:]?\s*(\d+)";
            string debitAccNumberPattern = @"debit\s*account\s*number\s*[-:]?\s*(\w+)";
            string customerNamePattern = @"customer\s*name\s*[-:]?\s*([\w\.\s]+?)\s*credit\s*account\s*number";
            string creditAccNumberPattern = @"credit\s*account\s*number\s*[-:]?\s*(\w+)";
            string transferAmountPattern = @"transfer\s*amount\s*[-:]?\s*([\d,]*\.\d*)";
            string currencyPattern = @"transfer\s*amount\s*[-:]?\s*[\d,]*\.\d*\s*(\w*)";
            string fundsTransferMethodPattern = @"funds\s*transfer\s*method\s*[-:]?\s*(\w*)";
            string statusPattern = @"status\s*[-:]?\s*(\w*\s\w*)\s*";
            string transactionDatePattern = @"transaction\s*date\s*and\s*time\s*[-:]?\s*(\d{2}-\d{2}-\d{4})";
            string transactionTimePattern = @"transaction\s*date\s*and\s*time\s*[-:]?\s*\d{2}-\d{2}-\d{4}\s*(\d{2}:\d{2}:\d{2})";
            string descriptionPattern = @"description\s*[-:]?\s*([\w\s\.\,]+)";

            try
            {
                string onlineBankingRef = (Regex.Match(transactionData, onlineBankingRefPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string hostRef = (Regex.Match(transactionData, hostRefPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string debitAccNumber = (Regex.Match(transactionData, debitAccNumberPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string customerName = (Regex.Match(transactionData, customerNamePattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string creditAccNumber = (Regex.Match(transactionData, creditAccNumberPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                decimal transferAmount = decimal.Parse((Regex.Match(transactionData, transferAmountPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim());
                string currency = (Regex.Match(transactionData, currencyPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string fundsTransferMethod = (Regex.Match(transactionData, fundsTransferMethodPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string status = (Regex.Match(transactionData, statusPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                DateOnly transactionDate = DateOnly.ParseExact(Regex.Match(transactionData, transactionDatePattern, RegexOptions.IgnoreCase).Groups[1].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                TimeOnly transactionTime = TimeOnly.Parse(Regex.Match(transactionData, transactionTimePattern, RegexOptions.IgnoreCase).Groups[1].Value);
                string description = (Regex.Match(transactionData, descriptionPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();

                OwnAccountTransfer transfer = new()
                {
                    OnlineBankingReferenceNumber = onlineBankingRef,
                    HostReferenceNumber = hostRef,
                    DebitAccountNumber = debitAccNumber,
                    CustomerName = customerName,
                    CreditAccountNumber = creditAccNumber,
                    TransferAmount = transferAmount,
                    Currency = currency,
                    FundsTransferMethod = fundsTransferMethod,
                    Status = status,
                    TransactionDate = transactionDate,
                    TransactionTime = transactionTime,
                    Description = description
                };

                return transfer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An error occurred while parsing transaction data: {ex.Message}");
            }
        }

        private static BOC_CC_Payment ParseBOC_CC_PaymentData(string transactionData)
        {
            string internetBankingRefPattern = @"internet\s*banking\s*reference\s*number\s*[-:]?\s*(\d+)";
            string hostRefPattern = @"host\s*reference\s*number\s*[-:]?\s*(\d+)";
            string fromAccountPattern = @"from\s*account\s*[-:]?\s*(\w+)";
            string CardNumberPattern = @"card\s*number\s*[-:]?\s*(\w+)";
            string transferAmountPattern = @"amount\s*[-:]?\s*([\d,]*\.\d*)";
            string currencyPattern = @"amount\s*[-:]?\s*[\d,]*\.\d*\s*(\w*)";
            string statusPattern = @"status\s*[-:]?\s*(\w*\s\w*)\s*";
            string transactionDatePattern = @"transaction\s*date\s*and\s*time\s*[-:]?\s*(\d{2}-\d{2}-\d{4})";
            string transactionTimePattern = @"transaction\s*date\s*and\s*time\s*[-:]?\s*\d{2}-\d{2}-\d{4}\s*(\d{2}:\d{2}:\d{2})";

            try
            {
                string internetBankingRef = (Regex.Match(transactionData, internetBankingRefPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string hostRef = (Regex.Match(transactionData, hostRefPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string fromAccount =( Regex.Match(transactionData, fromAccountPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string cardNumber = (Regex.Match(transactionData, CardNumberPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                decimal transferAmount = decimal.Parse((Regex.Match(transactionData, transferAmountPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim());
                string currency = (Regex.Match(transactionData, currencyPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                string status = (Regex.Match(transactionData, statusPattern, RegexOptions.IgnoreCase).Groups[1].Value).Trim();
                DateOnly transactionDate = DateOnly.ParseExact(Regex.Match(transactionData, transactionDatePattern, RegexOptions.IgnoreCase).Groups[1].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                TimeOnly transactionTime = TimeOnly.Parse(Regex.Match(transactionData, transactionTimePattern, RegexOptions.IgnoreCase).Groups[1].Value);

                BOC_CC_Payment payment = new()
                {
                    InternetBankingReferenceNumber = internetBankingRef,
                    HostReferenceNumber = hostRef,
                    CardNumber = cardNumber,
                    Amount = transferAmount,
                    Currency = currency,
                    Status = status,
                    TransactionDate = transactionDate,
                    TransactionTime = transactionTime
                };

                return payment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An error occurred while parsing transaction data: {ex.Message}");
            }
        }

        private static ThirdParty_BOC_ACC_Transfer ParseThirdPartyBOC_ACC_TransferData(string transactionData)
        {
            throw new NotImplementedException();
        }

        [GeneratedRegex("own account transfer", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex OwnAccountTransferPattern();

        [GeneratedRegex("credit card payment", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex BOC_CC_Pattern();
    }
}
