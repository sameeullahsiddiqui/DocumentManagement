using System;

namespace DocumentApp.Dto.Dtos
{
    public class RackBlockMasterDto : BaseDto
    {
        public string BlockNumber { get; set; }
        public string RackNumber { get; set; }
        public string Remark { get; set; }
        public Guid RackId { get; set; }
    }
}
