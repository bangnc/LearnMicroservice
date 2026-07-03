using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveAsync(IFormFile file, string fileName);

        Task DeleteAsync(string path);
    }
}
