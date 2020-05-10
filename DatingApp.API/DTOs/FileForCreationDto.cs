using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.DTOs
{
    public class FileForCreationDto
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        public DateTime DateAdded { get; set; }

        public FileForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}