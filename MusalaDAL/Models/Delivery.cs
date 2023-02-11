using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaDAL.Models
{
    public class Delivery
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public int DroneID { get; set; }
        public int MedicationID { get; set; }
        public int BatchID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Comments { get; set; }
        public DeliveryStatus Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string CancelReason { get; set; }
        public DateTime DeliveredOn { get; set; }

        public enum DeliveryStatus { Pending, Enroute, Delivered, Cancelled, Suspended }
    }
}
