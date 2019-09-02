using DocumentApp.Core.Entities.Foundation;
using System.Collections.Generic;

namespace DocumentApp.Core.Entities
{
    public class RackMaster : AuditableEntity
    {
        public string RackNumber { get; set; }
        public string Remark { get; set; }

        public string Description { get; set; }
        public virtual ICollection<RackBlockMaster> RackBlocks { get; set; }
    }
}
