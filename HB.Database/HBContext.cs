using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using HB.Database.DbModels;
using Strateq.Core.Database;

namespace HB.Database
{
    public partial class HBContext : CoreContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<CPSiteDetails> CPSiteDetails { get; set; }
        public DbSet<CPDetails> CPDetails { get; set; }
        public DbSet<CPConnector> CPConnector { get; set; }
        public DbSet<CPBreakdownError> CPBreakdownError { get; set; }
        public DbSet<CPTransaction> CPTransaction { get; set; }
        public DbSet<RunningSequenceNumber> RunningSequenceNumber { get; set; }
        public DbSet<UserAccountAuthorizationToken> UserAccountAuthorizationToken { get; set; }
        public DbSet<CPBreakdownDurationDetails> CPBreakdownDurationDetails { get; set; }
        public DbSet<PriceVaries> PriceVaries { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<PricingPlan> PricingPlan { get; set; }
        public DbSet<PricingPlanType> PricingPlanType { get; set; }
        public DbSet<MeterValue> MeterValue { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<GuestDetails> GuestDetails { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public HBContext(DbContextOptions<HBContext> options) : base(options)
        {
        }
    }
}
