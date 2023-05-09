using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using StackOverflowAPI.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackOverflowAPI.Migrations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using StackOverflowAPI.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StackOverflowDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("StackOverflowConnectionString")));
//I don't know why but it doesn't work so I had to use [JsonIgnore]
//=====================================
//builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.Configure<JsonOptions>(option =>
//{
//    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//});
//=====================================
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<StackOverflowDbContext>();
var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
    dbContext.Database.Migrate();

app.UseHttpsRedirection();

Question sampleQuestion = dbContext.Questions.Include(q => q.Answers).FirstOrDefault();
Guid QuestionAuthorId = new Guid();
Guid AnswerAuthorId = new Guid();
Guid AnswerId = new Guid();
Guid QuestionId = new Guid();
if (sampleQuestion == null)
{
    CreateSampleData(dbContext);
}
else
{
    QuestionId = sampleQuestion.Id;
    QuestionAuthorId = sampleQuestion.AuthorId;
    Answer sampleAnswer = sampleQuestion.Answers.FirstOrDefault();
    AnswerId = sampleAnswer.Id;
    AnswerAuthorId = sampleAnswer.AuthorId;
}

async void CreateSampleData(StackOverflowDbContext db)
{
    var address = new List<Address>()
    {
      new Address()
      {
          Street = "krótka",
          City = "Bielsko-Bia³a",
          Postcode = "43-300"
      },
      new Address()
      {
          Street = "d³uga",
          City = "Warszawa",
          Postcode = "00-015"
      }
    };
    var authors = new List<User>
    {
        new User()
        {
            Name = "Jan",
            Surname = "Kowalski",
            Address = address[0]
        },
        new User() {
            Name = "Adam",
            Surname = "Nowak",
            Address = address[1]
        }
    };

    var tag = new Tag()
    {
        Name = "JavaScript"
    };

    var comments = new List<Comment>()
    {
        new Comment()
        {
            Author = authors[1],
            Content = "Question comment ehhh...."
        },
        new Comment()
        {
            Author = authors[0],
            Content = "Thanks, it worked"
        }
    };

    var answer = new Answer()
    {
        Author = authors[1],
        Content = "Try to change string to int",
        Comments = new List<Comment>() { comments[1] }
    };

    var question = new Question()
    {
        Author = authors[0],
        Content = @"Why my code doesn't work. It should print i?
                    for(let string i = 0;i<15;i++)
                    {
                        console.log(i)
                    }",
        Comments = new List<Comment>() { comments[1] },
        Answers = new List<Answer>() { answer },
        Tags = new List<Tag>() { tag },
    };

    await db.Questions.AddAsync(question);
    await db.SaveChangesAsync();
    QuestionAuthorId = authors[0].Id;
    AnswerAuthorId = authors[1].Id;
    QuestionId = question.Id;
    AnswerId = answer.Id;
};

app.MapPost("rateQuestion", async (StackOverflowDbContext db, RatingType ratingType) =>
{
    var user = db.Users.Include(u => u.Ratings).FirstOrDefault(u => u.Id == AnswerAuthorId);
    var question = db.Questions.FirstOrDefault(q => q.Id == QuestionId);
    if (user == null || question == null)
        return null;
    var userRating = user.Ratings.FirstOrDefault(r => r.Question.Id == question.Id);
    if (ratingType == RatingType.Like)
    {
        if (userRating == null || userRating.Value == 0 || userRating.Value == -1)
        {
            if (userRating == null)
            {
                userRating = new Rating { User = user };
                question.Ratings.Add(userRating);
                question.RatingSum += 1;
            }
            else if (userRating.Value == 0)
            {
                question.RatingSum += 1;
            }
            else
            {
                question.RatingSum += 2;
            }
            userRating.Value = 1;
        }
        else return null;
    }
    else if (ratingType == RatingType.Dislike)
    {
        if (userRating == null || userRating.Value == 0 || userRating.Value == 1)
        {
            if (userRating == null)
            {
                userRating = new Rating { User = user };
                question.Ratings.Add(userRating);
                question.RatingSum -= 1;
            }
            else if (userRating.Value == 0)
            {
                question.RatingSum -= 1;
            }
            else
            {
                question.RatingSum -= 2;
            }
            userRating.Value = -1;
        }
        else return null;
    }
    else if (ratingType == RatingType.UndoRating)
    {
        if (userRating != null && userRating.Value != 0)
        {
            if (userRating.Value == 1)
            {
                question.RatingSum -= 1;
            }
            else
            {
                question.RatingSum += 1;
            }
            userRating.Value = 0;
        }
        else return null;
    }
    else
    {
        return null;
    }
    await db.SaveChangesAsync();
    return question;
});

app.MapPost("/rateAnswer", async (StackOverflowDbContext db, RatingType ratingType) =>
{
    var user = db.Users.Include(u => u.Ratings)
    .First(u => u.Id == QuestionAuthorId);
    var answer = db.Answers
    .FirstOrDefault(a => a.Id == AnswerId);
    if (user == null || answer == null)
        return null;
    var userRating = user.Ratings.FirstOrDefault(r => r.AnswerId == answer.Id);
    if (ratingType == RatingType.Like)
    {
        if (userRating == null || userRating.Value == 0 || userRating.Value == -1)
        {
            if (userRating == null)
            {
                userRating = new Rating { User = user };
                answer.Ratings.Add(userRating);
                answer.RatingSum += 1;
            }
            else if (userRating.Value == 0)
            {
                answer.RatingSum += 1;
            }
            else
            {
                answer.RatingSum += 2;
            }
            userRating.Value = 1;
        }
        else return null;
    }
    else if (ratingType == RatingType.Dislike)
    {
        if (userRating == null || userRating.Value == 0 || userRating.Value == 1)
        {
            if (userRating == null)
            {
                userRating = new Rating { User = user };
                answer.Ratings.Add(userRating);
                answer.RatingSum -= 1;
            }
            else if (userRating.Value == 0)
            {
                answer.RatingSum -= 1;
            }
            else
            {
                answer.RatingSum -= 2;
            }
            userRating.Value = -1;
        }
    }
    else if (ratingType == RatingType.UndoRating)
    {
        if (userRating != null && userRating.Value != 0)
        {
            if (userRating.Value == 1)
            {
                answer.RatingSum -= 1;
            }
            else
            {
                answer.RatingSum += 1;
            }
            userRating.Value = 0;
        }
    }
    else
    {
        return null;
    }
    await db.SaveChangesAsync();
    return answer;
});

app.MapPost("/addQuestionComment", async (StackOverflowDbContext db) =>
{
    var content = "Good question";
    var user = db.Users.FirstOrDefault(u => u.Id == AnswerAuthorId);
    var question = db.Questions.Include(q => q.Comments)
    .FirstOrDefault(q => q.Id == QuestionId);
    if (user == null || question == null)
        return null;
    var newComment = new Comment { Author = user, Content = content };
    question.Comments.Add(newComment);
    await db.SaveChangesAsync();
    return newComment;
});

app.MapPost("/addAnswerComment", async (StackOverflowDbContext db) =>
{
    var content = "Good answer";
    var user = db.Users.FirstOrDefault(u => u.Id == QuestionAuthorId);
    var answer = db.Answers.Include(a => a.Comments)
    .FirstOrDefault(a => a.Id == AnswerId);
    if (user == null || answer == null)
        return null;
    var newComment = new Comment { Author = user, Content = content };
    answer.Comments.Add(newComment);
    await db.SaveChangesAsync();
    return newComment;
});

app.MapPost("/createTag", async (StackOverflowDbContext db) =>
{
    Tag tag = new Tag()
    {
        Name = "Python"
    };
    await db.Tags.AddAsync(tag);
    await db.SaveChangesAsync();

    return tag;
});

app.Run();