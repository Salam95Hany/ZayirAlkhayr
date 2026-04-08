using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Items
{
    public class ItemSpecification : BaseSpecification<Item>
    {
        public ItemSpecification(PagingFilterModel filterModel, bool applyPaging = true) : base()
        {
            var searchText = filterModel.FilterList.FirstOrDefault(f => f.CategoryName == "SearchText")?.ItemId;

            if (!string.IsNullOrEmpty(searchText))
                AddCriteria(fc => fc.Name.Contains(searchText));

            AddInclude(i => i.Category);

            if (applyPaging)
                ApplyPaging((filterModel.Currentpage - 1) * filterModel.Pagesize, filterModel.Pagesize);
        }
    }
}
