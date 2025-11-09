using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Data
{
    public class BillingContext: DbContext
    {
        public BillingContext(DbContextOptions<BillingContext> options) : base(options)
        {
        }
        public DbSet<Entities.Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
