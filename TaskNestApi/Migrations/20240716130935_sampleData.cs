using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNestApi.Migrations
{
    /// <inheritdoc />
    public partial class sampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql
            (
            "INSERT INTO users (name, password, isAdmin) " +
            "SELECT 'admin', 'password123', 1 " +
            "WHERE NOT EXISTS (SELECT 1 FROM users) " +
            "UNION ALL " +
            "SELECT 'user1', 'password456', 0 " +
            "WHERE NOT EXISTS (SELECT 1 FROM users);" +

            "INSERT INTO tickets (name, status, user, description) " +
            "SELECT 'Ticket 1', 'new', (SELECT id FROM users WHERE name = 'admin'), 'This is a sample ticket' " +
            "WHERE NOT EXISTS (SELECT 1 FROM tickets);" +

            "INSERT INTO changes (ticketId, date, prevName, prevStatus, prevDescription, prevUser) " +
            "SELECT (SELECT id FROM tickets WHERE name = 'Ticket 1'), '2023-11-24', 'Initial Name', 'Initial Status', 'Initial Description', (SELECT id FROM users WHERE name = 'admin') " +
            "WHERE NOT EXISTS (SELECT 1 FROM changes) " +
            "UNION ALL " +
            "SELECT (SELECT id FROM tickets WHERE name = 'Ticket 1'), '2023-11-25', 'Changed Name', 'In Progress', 'Changed Description', (SELECT id FROM users WHERE name = 'user1') " +
            "WHERE NOT EXISTS (SELECT 1 FROM changes) " +
            "UNION ALL " +
            "SELECT (SELECT id FROM tickets WHERE name = 'Ticket 1'), '2023-11-26', 'Final Name', 'Finished', 'Final Description', (SELECT id FROM users WHERE name = 'admin') " +
            "WHERE NOT EXISTS (SELECT 1 FROM changes);"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
