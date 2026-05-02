using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "SupplierPayments", Schema = "Inv")]
    public class SupplierPayment
    {
        public int SupplierPaymentId { get; set; }
        // المورد
        public int SupplierId { get; set; }
        // الفاتورة المرتبطة
        public int PurchaseId { get; set; }
        // رقم سند الدفع
        public string PaymentNumber { get; set; }
        // المبلغ المدفوع
        public double AmountPaid { get; set; }
        // تاريخ الدفع
        public DateTime PaymentDate { get; set; }
        // Cash / Bank Transfer / Visa
        public string PaymentMethod { get; set; }
        // ملاحظات
        public string Notes { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
