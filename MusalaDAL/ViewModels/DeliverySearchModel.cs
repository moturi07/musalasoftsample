using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaDAL.ViewModels
{
    public class DeliverySearchModel
    {
        public int ID { get; set; }
        public int DroneID { get; set; }
        public int MedicationID { get; set; }
        public int BatchID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}
