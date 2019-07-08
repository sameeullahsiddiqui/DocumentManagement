using DocumentManagement.Core.Interfaces;
using DocumentManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DocumentManagement.Infrastructure.Repository
{
    public class DocumentManagementContext : DbContext
    {
        public DocumentManagementContext() : base("name=DocumentManagementContext")
        {
        }

        public DbSet<RackMaster> RackMasters { get; set; }
        public DbSet<RackBlockMaster> RackBlockMasters { get; set; }
        public DbSet<DocumentsType> DocumentTypes { get; set; }
        public DbSet<FileAllocation> FileAllocations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            //modelBuilder.Entity<Member>()
            //    .HasMany<FamilyMember>(g => g.FamilyMembers)
            //    .WithRequired(s => s.Member)
            //    .HasForeignKey<Guid>(s => s.MemberId);

            //modelBuilder.Entity<Member>()
            //    .HasMany<DonationDetail>(g => g.Donations)
            //    .WithRequired(s => s.Member)
            //    .HasForeignKey<Guid>(s => s.MemberId);
        }

    public override async Task<int> SaveChangesAsync()
        {
            var modifiedEntries = ChangeTracker.Entries()
                                               .Where(x => x.Entity is IAuditableEntity && 
                                                           (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is IAuditableEntity entity)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        (entry.Entity as IEntity).Id = Guid.NewGuid();
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
