using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Common
{
    public enum FamilyNeedTypes
    {
        Electric = 0, // الاجهزة الكهربائية
        Furniture = 1, // اثاث
        HomeMaintenance = 2, // صيانة المنزل
        Joinary = 3 // نجارة
    }

    public enum ImageFiles
    {
        SliderImages = 0,
        ActivityImages = 1,
        ActivitySliderImages = 2,
        EventImages = 3,
        ExportFiles = 4,
        PhotoImages = 5,
        PhotoDetailImages = 6
    }

    public enum FamilyStatusTabs
    {
        All = 0,
        Poor = 1,
        Widows = 2,
        Patient = 3,
        Needed = 4
    }
}
