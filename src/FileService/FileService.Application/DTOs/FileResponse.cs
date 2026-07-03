using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Application.DTOs
{
    public class FileResponse
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public long Size { get; set; }

        public string ContentType { get; set; } = string.Empty;
    }
}
