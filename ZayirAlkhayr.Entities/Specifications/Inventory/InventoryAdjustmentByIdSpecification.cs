using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Inventory
{
    public class InventoryAdjustmentByIdSpecification:BaseSpecification<InventoryAdjustment>
    {
        public InventoryAdjustmentByIdSpecification(int InventoryAdjustmentId) : base(i => i.InventoryAdjustmentId == InventoryAdjustmentId)
        {
            AddInclude(i => i.Details);
            AddInclude("Details.InventoryItem");
        }
    }
}
