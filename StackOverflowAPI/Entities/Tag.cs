﻿namespace StackOverflowAPI.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}