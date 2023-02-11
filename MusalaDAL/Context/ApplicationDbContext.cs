using MusalaDAL.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusalaDAL.Models;
using MusalaDAL.ViewModels;
using System.Data.Entity.Infrastructure;

namespace MusalaDAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Medication> Medication { get; set; }
        public DbQuery<DeliveriesViewModel> DeliveriesViewModel { get; set; }
    }
}
