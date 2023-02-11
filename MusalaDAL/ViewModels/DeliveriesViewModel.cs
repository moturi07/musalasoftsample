using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaDAL.ViewModels
{
    public class DeliveriesViewModel
    {
        public string DroneName { get; set; }
        public string Medication { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
