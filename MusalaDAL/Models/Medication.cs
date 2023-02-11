using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusalaDAL.Models
{
    public class Medication
    {
        [Key]
        public int ID { get; set; }
        public bool Active { get; set; }
        [RegularExpression(@"^[a-zA-Z\d-_]+$", ErrorMessage = "allowed only letters, numbers, hyphen and Underscore.")]
        public string name { get; set; }
        public int weight { get; set; }

        [RegularExpression(@"^[A-Z0-9\_\]+$", ErrorMessage = "allowed only upper case letters, underscore and numbers.")]
        public string code { get; set; }
        public byte[] image { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
