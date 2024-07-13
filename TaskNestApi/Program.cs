var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

var sampleUsers = Enumerable.Range(0, 5).Select(index => 
    new User(
        index,
        "user" + index.ToString(),
        "password",
        index > 0 ? false : true
    )
).ToArray();

var sampleNames = new [] {
    "Name", "anotherName", "Ticket123", "Task", "TicketTask",
    "Name123", "anotherTaskName", "Ticket", "TaskSample", "TicketName",
    "Tasky", "anotherNames", "6562137", "NewName", "ToDoTask"
};

var statuses = new [] {
    "new", "finished", "in_progress"
};

var sampleTickets = Enumerable.Range(0, 15).Select(index =>
    new Ticket(
        index,
        sampleNames[Random.Shared.Next(sampleNames.Length)],
        statuses[Random.Shared.Next(statuses.Length)],
        Random.Shared.Next(0, 5),
        sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)]
    )
).ToArray();

app.MapGet("/users", () => {
    return sampleUsers;
});

app.MapGet("/tickets", () => {
    return sampleTickets;
});

app.MapGet("/changes/{id}", (int id) => {
    var sampleChanges = Enumerable.Range(0, Random.Shared.Next(1, 5)).Select(index =>
        new Change(
            index,
            id,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-index)),
            sampleNames[Random.Shared.Next(sampleNames.Length)],
            statuses[Random.Shared.Next(statuses.Length)],
            sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)] + sampleNames[Random.Shared.Next(sampleNames.Length)],
            Random.Shared.Next(0, 5)
        )
    ).ToArray();

    return sampleChanges;
});

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
record User(int Id, string Name, string Password, bool IsAdmin);

record Ticket(int Id, string Name, string Status, int? User, string? Description);

record Change(int Id, int TicketId, DateOnly Date, string PrevName, string PrevStatus, string PrevDescription, int PrevUser);