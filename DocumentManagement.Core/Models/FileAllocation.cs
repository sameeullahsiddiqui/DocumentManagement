using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Core.Models
{
    public class FileAllocation : AuditableEntity
    {
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public string Remark { get; set; }
        public Guid RackBlockId { get; set; }
        public Guid DocumentTypeId { get; set; }

        [JsonIgnore]
        public RackBlockMaster RackBlock { get; set; }
        [JsonIgnore]
        public DocumentsType DocumentType { get; set; }
    }
}
