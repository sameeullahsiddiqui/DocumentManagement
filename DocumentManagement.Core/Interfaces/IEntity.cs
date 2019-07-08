using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DocumentManagement.Core.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
