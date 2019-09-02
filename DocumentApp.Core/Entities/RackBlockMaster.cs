using DocumentApp.Core.Entities.Foundation;
using Newtonsoft.Json;
using System;

namespace DocumentApp.Core.Entities
{
    public class RackBlockMaster : AuditableEntity
    {
        public string BlockNumber { get; set; }
        public string Remark { get; set; }
        public Guid RackId { get; set; }

        public string Description { get; set; }
        public RackMaster Rack { get; set; }
    }
}