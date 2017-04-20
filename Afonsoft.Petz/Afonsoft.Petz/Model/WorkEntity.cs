using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afonsoft.Petz.Model
{
    public class WorkEntity
    {
        public int WeekId { get; set; }
        public DayOfWeek Week
        {
            get { return (DayOfWeek) Enum.Parse(typeof(DayOfWeek), WeekId.ToString()); }
        }

        public int StartTimeMin { get; set; }
        public int EndTimeMin { get; set; }
        public string StartTime {
            get
            {
                if (StartTimeMin > 0)
                {
                    return new DateTime(2016, 1, 1).AddMinutes(StartTimeMin).ToString("HH:mm");
                }
                return "00:00";
            }
        }
        public string EndTime {
            get
            {
                if (EndTimeMin > 0)
                {
                    return new DateTime(2016, 1, 1).AddMinutes(EndTimeMin).ToString("HH:mm");
                }
                return "00:00";
            }
        }
    }
}