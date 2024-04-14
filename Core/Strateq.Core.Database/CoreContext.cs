using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Strateq.Core.Database.DbModel;
using Strateq.Core.Database.Helper;

namespace Strateq.Core.Database
{
    public partial class CoreContext : DbContext
    {
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<AuditLogDetail> AuditLogDetail { get; set; }

        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {

        }

        public CoreContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cpms");

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                            .SelectMany(t => t.GetForeignKeys())
                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveAuditedChanges()
        {
            var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();

            var entries = ChangeTracker.Entries();
            List<AuditLogEntry> auditEntries = new();

            //log modified entries 
            foreach (var entry in modifiedEntries)
            {
                var log = AuditHelper.AddAuditLogUpdate(entry);
                auditEntries.Add(log);
            }

            //Save changes to insert data and get Ids for new entries 
            var result = await base.SaveChangesAsync();

            //log new inserted record entries 
            foreach (var entry in addedEntries)
            {
                var log = AuditHelper.AddAuditLogAdded(entry);
                auditEntries.Add(log);
            }

            auditEntries.RemoveAll(item => item == null);

            foreach (var auditEntry in auditEntries)
            {
                AuditLog.Add(auditEntry.Auditlog);
                SaveChanges();
                var id = auditEntry.Auditlog.Id;
                foreach (var detail in auditEntry.Details)
                {
                    detail.AuditLogId = id;
                    AuditLogDetail.Add(detail);
                    SaveChanges();
                }
            }

            return result;
        }
    }
}
