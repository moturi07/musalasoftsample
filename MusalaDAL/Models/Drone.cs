using System;
using System.ComponentModel.DataAnnotations;

namespace MusalaDAL.Models
{
    public class Drone
    {
        [Key]
        public int ID { get; set; }
        public bool Active { get; set; }
        [StringLength(150)]
        public string serialNumber { get; set; }
        public modelType model { get; set; } = modelType.Cruiserweight;
        [Range(0.0, 500, ErrorMessage = "The Weight Limit must be between 0 to 500 grams.")]
        public int weightLimit { get; set; }
        [Range(0.0, 100, ErrorMessage = "The percentage must be between 0 to 100.")]
        public int batteryCapacity { get; set; }

        public stateType state { get; set; } = stateType.IDLE;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public enum modelType{Lightweight=1, Middleweight=2, Cruiserweight=3, Heavyweight=3}
        public enum stateType{ IDLE=1, LOADING=2, LOADED=3, DELIVERING=4, DELIVERED=5, RETURNING=6 }
    }
}
