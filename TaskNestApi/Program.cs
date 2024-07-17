//By Mikołaj Poznański, 2024

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

//Users
app.MapGet("/users", async (TaskNestDb db) => await db.Users.Select(u => new {u.Id, u.Name, u.IsAdmin}).ToListAsync());

app.MapPost("/login", async (TaskNestDb db, User user) => 
{
    var result = await db.Users.Where(u => u.Name == user.Name).SingleAsync();//.Select(u => u);//.SingleAsync(u => u);
    // var result = await (from u in db.Users
    //                     where u.Name == user.Name
    //                     select u
    // ).SingleAsync();

    if (result is null) return Results.NotFound();
    if (result.Password != user.Password) return Results.NotFound();
    return Results.Ok(new {result.Id, result.Name, result.IsAdmin});
});

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
//app.MapGet("/simpletickets", async (TaskNestDb db) => await db.Tickets.ToListAsync());
app.MapGet("/tickets", async (TaskNestDb db) => {
    var tickets = await (from ticket in db.Tickets
                        join user in db.Users on ticket.User equals user.Id
                        select new
                        {
                            ticket.Id,
                            ticket.Name,
                            ticket.Description,
                            ticket.Status,
                            ticket.User,
                            userName = user.Name,
                        }).ToListAsync();
    return tickets;
});

app.MapGet("/tickets/{id}", async (TaskNestDb db, int id) => {
    var result = await (from ticket in db.Tickets
                        where ticket.Id == id
                        join user in db.Users on ticket.User equals user.Id
                        select new
                        {
                            ticket.Id,
                            ticket.Name,
                            ticket.Description,
                            ticket.Status,
                            ticket.User,
                            userName = user.Name,
                        }).SingleAsync();
    return result;
});
//app.MapGet("/tickets/{id}", async (TaskNestDb db, int id) => await db.Tickets.FindAsync(id));

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

    //Potentially replace with trigger in database?
    //I planned to make trigger that would add Changes when updating tickets, but it seems EF Core doesn't support triggers?
    Change change = new Change
    {
        Id = 0,
        PrevName = "",
        PrevDescription = "",
        PrevStatus = "",
        PrevUser = 0,
        TicketId = 0,
        Date = DateTime.Now
    };
    change.PrevName = ticket.Name;
    change.PrevDescription = ticket.Description is null ? "" : ticket.Description;
    change.PrevStatus = ticket.Status;
    change.PrevUser = (int)ticket.User;
    change.TicketId = ticket.Id;
    change.Date = DateTime.Now;

    await db.Changes.AddAsync(change);
    //

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
//app.MapGet("/changes/{id}", async (TaskNestDb db, int id) => await db.Changes.Where(c => c.TicketId == id).ToListAsync());
app.MapGet("/changes/{id}", async (TaskNestDb db, int id) => {
    var changes = await (from change in db.Changes
                        where change.TicketId == id
                        join user in db.Users on change.PrevUser equals user.Id
                        select new
                        {
                            change.Id,
                            change.Date,
                            change.PrevName,
                            change.PrevStatus,
                            change.PrevUser,
                            change.PrevDescription,
                            prevUserName = user.Name
                        }).ToListAsync();
    return changes;
});

app.Run();