using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Core.Models
{
    public class DocumentsType : AuditableEntity
    {
        public string DocumentType { get; set; }
        public string Remark { get; set; }
    }
}
