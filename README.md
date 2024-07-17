# TaskNestApi
This is TaskNest API, a minimal API built with ASP.NET Core for managing tickets/tasks. Made for TaskNest web app [available here](https://github.com/m-poznanski/TaskNest)

## Requirements:
.NET 6 or later

## Setup:
1. Clone or download the repository.
2. Restore dependencies: `dotnet restore`
3. (Optionally) Change the SQLite database connection string in the Program.cs
4. Run the migrations: `dotnet ef database update`
5. Run the project: `dotnet run`

## API Usage:
  
***!!! Account management is very provisional and not secure. There is no authentication, nor encryption. Login system is just a blueprint/placeholder. !!!***  
  
The API uses JSON for request and response bodies.
### Endpoints:
#### Users:
- GET /users: Returns a list of users (filtered to only include Id, Name, and IsAdmin).
- POST /login (placeholder): Placeholder for user login (not implemented securely).

#### Tickets:
- GET /tickets: Returns a list of tickets with joined user information (including userName).
- GET /tickets/{id}: Retrieves a specific ticket by ID with joined user information.
- POST /ticket: Creates a new ticket.
- PUT /ticket/{id}: Updates an existing ticket (including tracking changes).
- DELETE /ticket/{id}: Deletes a ticket.

#### Changes:
- GET /changes/{id}: Retrieves a list of changes made to a specific ticket (includes previous user names using joined user data).

### Swagger:
The API includes Swagger documentation for easy exploration. You can access it at `http://localhost:<port>/swagger/index.html` (replace <port> with the port your application is running on).
