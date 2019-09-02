using DocumentApp.Core.Entities.Foundation;

namespace DocumentApp.Core.Entities
{

    public class DocumentType : AuditableEntity
    {
        public string DocumentTypeName { get; set; }
        public string Remark { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
    }
}
