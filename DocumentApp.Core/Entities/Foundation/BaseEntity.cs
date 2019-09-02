using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentApp.Core.Entities.Foundation
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
