using DocumentManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.Core.Models
{
    public abstract class BaseEntity
    {
    }

    public abstract class Entity : BaseEntity, IEntity
    {
        public virtual Guid Id { get; set; }
    }
}
