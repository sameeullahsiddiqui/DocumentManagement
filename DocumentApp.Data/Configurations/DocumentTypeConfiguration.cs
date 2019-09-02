using DocumentApp.Core.Entities;
using System.Data.Entity.ModelConfiguration;


namespace DocumentApp.Data.Configurations
{
    public class DocumentTypeConfiguration : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeConfiguration()
        {
            Property(c => c.DocumentTypeName).IsRequired().HasMaxLength(100);
            Property(c => c.Remark).HasMaxLength(200);
            Property(c => c.FileName).HasMaxLength(200);
            Property(c => c.Description).HasMaxLength(200);
        }
    }
}