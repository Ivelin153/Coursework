using System;

namespace AprioriApp.API.Model
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}