using System;
using Microsoft.AspNetCore.Http;

namespace AprioriApp.API.DTOs
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