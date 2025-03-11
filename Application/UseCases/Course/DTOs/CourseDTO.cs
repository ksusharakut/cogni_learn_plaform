﻿namespace Application.Use_Cases.Course.DTOs
{
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorEmail { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Categories { get; set; } 
    }
}
