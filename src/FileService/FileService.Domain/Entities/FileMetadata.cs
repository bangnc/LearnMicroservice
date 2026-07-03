using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.Entities
{
    public class FileMetadata
    {
        public Guid Id { get; set; }

        // Tên file sau khi đổi (GUID)
        public string FileName { get; set; } = string.Empty;

        // Tên gốc người dùng upload
        public string OriginalFileName { get; set; } = string.Empty;

        // jpg, png, pdf...
        public string Extension { get; set; } = string.Empty;

        // image/png...
        public string ContentType { get; set; } = string.Empty;

        // Kích thước (bytes)
        public long Size { get; set; }

        // Đường dẫn lưu
        public string FilePath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
