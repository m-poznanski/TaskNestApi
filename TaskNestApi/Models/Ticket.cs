using System.ComponentModel.DataAnnotations.Schema;

namespace TaskNestApi.Models
{
    public class Ticket
    {
        public required int Id {get; set;} 
        public required string Name {get; set;}
        public required string Status {get; set;} = "new";
        public int? User {get; set;}
        public string? Description {get; set;}
    }
}