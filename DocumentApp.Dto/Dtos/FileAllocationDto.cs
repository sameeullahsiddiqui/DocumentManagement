using System;

namespace DocumentApp.Dto.Dtos
{
    public class FileAllocationDto : BaseDto
    {
        public FileAllocationDto(){        }

        public string FileName { get; set; }
        public string FolderName { get; set; }
        public string Remark { get; set; }
        public Guid RackBlockId { get; set; }
        public Guid DocumentTypeId { get; set; }
    }
}

