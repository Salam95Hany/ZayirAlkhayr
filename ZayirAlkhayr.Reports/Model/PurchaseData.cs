using System;
using Pro.Tafqeet.Arabic;
using System.Collections.Generic;
using System.Globalization;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using Pro.Tafqeet.Arabic.Enums;
using System.Text;
using System.Text.RegularExpressions;

namespace ZayirAlkhayr.Reports.Model
{
    public class PurchaseData
    {
        public string PurchaseNumber { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public double TotalAmount { get; set; }
        public string TotalAmountAr { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertDateAr { get; set; }
        public string ImageSrc { get; set; }
        public List<PurchaseItemDetailsDto> Items { get; set; } = new List<PurchaseItemDetailsDto>();

        private readonly Random _random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string FormatNumber(double value)
        {
            if (value % 1 == 0)
                return value.ToString("N0");

            return value.ToString("0.##");
        }

        public void HandleData()
        {
            if (InsertDate.HasValue)
                InsertDateAr = InsertDate.Value.ToString("yyyy-MM-dd - hh:mm tt");

            if (TotalAmount > 0)
            {
                var converter = TafqeetConverterFactory.Create(TafqeetLanguage.Arabic);
                TotalAmountAr = converter.Convert((decimal)TotalAmount).Replace("ريال", "ليرة");
            }

            var sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
                sb.Append(chars[_random.Next(chars.Length)]);

            PurchaseNumber = $"PS-{sb}";
        }
    }
}
