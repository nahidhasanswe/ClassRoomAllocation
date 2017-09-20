using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Some_Logic
{
    public static class Date
    {
        public static DateTime GetLocalZoneDate(DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date,TimeZoneInfo.Local).Date;
        }
    }
}
