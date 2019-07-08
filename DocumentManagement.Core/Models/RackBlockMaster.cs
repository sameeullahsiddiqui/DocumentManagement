using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Core.Models
{
    public class RackBlockMaster : AuditableEntity
    {
        public string BlockNumber { get; set; }
        public string Remark { get; set; }
        public Guid RackId { get; set; }
        [JsonIgnore]
        public RackMaster Rack { get; set; }
    }
}
