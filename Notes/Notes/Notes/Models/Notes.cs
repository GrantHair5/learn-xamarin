using System;

namespace Notes.Models
{
    public class Note
    {
        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsInBasket { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}