using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaDAL.ViewModels
{
    public class MedicationViewModel
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public string name { get; set; }
        public int weight { get; set; }
        public string code { get; set; }
        public IFormFile image { get; set; }
    }
}
