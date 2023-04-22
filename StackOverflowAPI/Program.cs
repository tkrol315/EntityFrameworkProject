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
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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
    var authors = new List<Author>
    {
        new Author()
        {
            Name = "Jan",
            Surname = "Kowalski",
            Address = address[0]
        },
        new Author() {
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
        Rating = 13,
        Comments = new List<Comment>() { comments[1] }
    };

    var question = new Question()
    {
        Author = authors[0],
        Content = @"Why my code doesn't work. It should print i?
                    for(let string i = 0;i<15;i++)
                    {
                        ConsoleLog(i)
                    }",
        Comments = new List<Comment>() { comments[1] },
        Answers = new List<Answer>() { answer },
        Tags = new List<Tag>() { tag },
    };

    await db.Questions.AddAsync(question);
    await db.SaveChangesAsync();
    return question;
});

app.Run();