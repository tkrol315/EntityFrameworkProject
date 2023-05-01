using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using StackOverflowAPI.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
app.MapPost("/createQuestion", async (StackOverflowDbContext db) =>
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
    return question;
});
app.MapPost("/likeQuestion", async (StackOverflowDbContext db) =>
{
    var user = db.Users.Include(u => u.Ratings).First(u => u.Id == Guid.Parse("06064D9F-D1E1-43F6-B2F2-08DB4A8EEA77"));
    var question = db.Questions.First(q => q.Id == Guid.Parse("7A270758-AEFF-4FF8-A922-08DB4A8EEA64"));
    var userRating = question.Ratings.Where(r => r.UserId == user.Id).FirstOrDefault();
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

        await db.SaveChangesAsync();
        return question;
    }
    return null;
}
);
app.MapPost("/dislikeQuestion", async (StackOverflowDbContext db) =>
{
    var user = db.Users.Include(u => u.Ratings).First(u => u.Id == Guid.Parse("06064D9F-D1E1-43F6-B2F2-08DB4A8EEA77"));
    var question = db.Questions.First(q => q.Id == Guid.Parse("7A270758-AEFF-4FF8-A922-08DB4A8EEA64"));
    var userRating = question.Ratings.Where(r => r.UserId == user.Id).FirstOrDefault();
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

        await db.SaveChangesAsync();
        return question;
    }
    return null;
}
);
app.MapPost("/undoRatings", async (StackOverflowDbContext db) =>
{
    var user = db.Users.Include(u => u.Ratings)
    .First(u => u.Id == Guid.Parse("06064D9F-D1E1-43F6-B2F2-08DB4A8EEA77"));
    var question = db.Questions
    .First(q => q.Id == Guid.Parse("7A270758-AEFF-4FF8-A922-08DB4A8EEA64"));
    var userRating = question.Ratings.Where(r => r.UserId == user.Id).FirstOrDefault();
});
app.Run();