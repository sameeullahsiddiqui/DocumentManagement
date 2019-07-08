using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Core.Models
{
    public class RackMaster : AuditableEntity
    {
        public string RackNumber { get; set; }
        public string Remark { get; set; }
        public virtual ICollection<RackBlockMaster> RackBlocks { get; set; }
    }
}
