using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.DTO
{
    public class FrameFilterDTO
    {
        public string SearchText { get; set; }
        public double Price { get; set; }
        public int FrameStatus { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
    }
}
