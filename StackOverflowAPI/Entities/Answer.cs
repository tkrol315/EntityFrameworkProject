﻿using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Author Author { get; set; }

        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }

        public Question Question { get; set; }

        public Guid QuestionId { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}