using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Models;

// 2. Write Dbcontext
namespace ShoppingCart.Database
{
    public class ShoppingCartContext : DbContext
    {
        protected IConfiguration configuration;

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            // // unique name within a column
            // model.Entity<Cinema>().HasIndex(tbl => tbl.Name).IsUnique();

            // // composite keys
            // model.Entity<Seat>().HasAlternateKey(tbl =>
            //     new { tbl.ScreeningId, tbl.Row, tbl.Col }
            // );
        }

        public DbSet<User> Users { get; set; }
        // public DbSet<BookedSeat> BookedSeats { get; set; }
        // public DbSet<ReservedSeat> ReservedSeats { get; set; }

        // public DbSet<Movie> Movies { get; set; }
        // public DbSet<Screening> Screenings { get; set; }
        // public DbSet<Cinema> Cinemas { get; set; }
        // public DbSet<Booking> Bookings { get; set; }
    }
}
