using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TaskNestApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TaskNestApi") ?? "Data Source=TaskNestApi.db";

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddDbContext<TaskNestDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<TaskNestDb>(connectionString);

builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "TaskNest API",
         Description = "TaskNest - app for managing tickets/tasks",
         Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyAllowedOrigins");

// var sampleUsers = Enumerable.Range(0, 5).Select(index => 
//     new User(
//         index,
//         "user" + index.ToString(),
//         "",
//         index > 0 ? false : true
//     )
// ).ToArray();

var sampleNames = new [] {
    "Name", "anotherName", "Ticket123", "Task", "TicketTask",
    "Name123", "anotherTaskName", "Ticket", "TaskSample", "TicketName",
    "Tasky", "anotherNames", "6562137", "NewName", "ToDoTask"
};

var statuses = new [] {
    "new", "finished", "in_progress"
};

// var sampleTickets = Enumerable.Range(0, 15).Select(index =>
//     new Ticket(
//         index,
//         sampleNames[Random.Shared.Next(sampleNames.Length)],
//         statuses[Random.Shared.Next(statuses.Length)],
//         Random.Shared.Next(0, 5),
//         sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)]
//     )
// ).ToArray();

// var sampleTickets = Enumerable.Range(0, 15).Select(index =>
//     new Ticket{
//         Id = index,
//         Name = sampleNames[Random.Shared.Next(sampleNames.Length)],
//         Status = statuses[Random.Shared.Next(statuses.Length)],
//         User = Random.Shared.Next(0, 5),
//         Description = sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)]
//     }
// ).ToArray();



// app.MapGet("/users", () => {
//     return sampleUsers;
// });

// app.MapPost("/login", (User user) => {
// });

// app.MapGet("/tickets", () => {
//     return sampleTickets;
// });

//Users
app.MapGet("/users", async (TaskNestDb db) => await db.Users.ToListAsync());

app.MapPost("/user", async (TaskNestDb db, User user) => 
{
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Created($"/user/{user.Id}", user);
});

app.MapGet("/users/{id}", async (TaskNestDb db, int id) => await db.Users.FindAsync(id));

app.MapDelete("/user/{id}", async (TaskNestDb db, int id) =>
{
   var user = await db.Users.FindAsync(id);
   if (user is null) return Results.NotFound();
   db.Users.Remove(user);
   await db.SaveChangesAsync();
   return Results.Ok();
});

//Tickets
app.MapGet("/tickets", async (TaskNestDb db) => await db.Tickets.ToListAsync());

app.MapGet("/tickets/{id}", async (TaskNestDb db, int id) => await db.Tickets.FindAsync(id));

app.MapPost("/ticket", async (TaskNestDb db, Ticket ticket) => 
{
    await db.Tickets.AddAsync(ticket);
    await db.SaveChangesAsync();
    return Results.Created($"/ticket/{ticket.Id}", ticket);
});

app.MapPut("/ticket/{id}", async (TaskNestDb db, Ticket updateticket, int id) => 
{
    var ticket = await db.Tickets.FindAsync(id);
    if (ticket is null) return Results.NotFound();
    ticket.Name = updateticket.Name;
    ticket.Description = updateticket.Description;
    ticket.Status = updateticket.Status;
    ticket.User = updateticket.User;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/ticket/{id}", async (TaskNestDb db, int id) =>
{
   var ticket = await db.Tickets.FindAsync(id);
   if (ticket is null) return Results.NotFound();
   db.Tickets.Remove(ticket);
   await db.SaveChangesAsync();
   return Results.Ok();
});

//Changes
app.MapGet("/changes", async (TaskNestDb db) => await db.Changes.ToListAsync());
//app.MapGet("/changes/{id}", async (TaskNestDb db) => await db.Changes.ToListAsync());


// app.MapGet("/tickets/{id}", (int id) => {
//     var ticket = sampleTickets.FirstOrDefault(element => element.Id == id);
//     return ticket;
// });

// app.MapGet("/changes/{id}", (int id) => {
//     var sampleChanges = Enumerable.Range(0, Random.Shared.Next(1, 5)).Select(index =>
//         new Change(
//             index,
//             id,
//             DateOnly.FromDateTime(DateTime.Now.AddDays(-index)),
//             sampleNames[Random.Shared.Next(sampleNames.Length)],
//             statuses[Random.Shared.Next(statuses.Length)],
//             sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)],
//             Random.Shared.Next(0, 5)
//         )
//     ).ToArray();

//     return sampleChanges;
// });

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
// record User(int Id, string Name, string Password, bool IsAdmin);

// record Ticket(int Id, string Name, string Status, int? User, string? Description);

// record Change(int Id, int TicketId, DateOnly Date, string PrevName, string PrevStatus, string PrevDescription, int PrevUser);