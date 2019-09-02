using DocumentApp.Core.Entities.Foundation;
using Newtonsoft.Json;
using System;

namespace DocumentApp.Core.Entities
{
    public class FileAllocation : AuditableEntity
    {
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public string Remark { get; set; }
        public Guid RackBlockId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string Description { get; set; }
        public RackBlockMaster RackBlock { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
