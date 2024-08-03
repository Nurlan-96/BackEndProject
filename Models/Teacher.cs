﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Teacher:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string Faculty { get; set; }
        public string Degree { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int Number { get; set; }
        public int Experience { get; set; }
        public string Social { get; set; }
        public string ImageUrl { get; set; }
        public string Hobbies { get; set; }
    }
}
