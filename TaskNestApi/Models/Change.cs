using System.ComponentModel.DataAnnotations.Schema;

namespace TaskNestApi.Models
{
    public class Change
    {
        public required int Id {get; set;} 
        public required int TicketId {get; set;}
        public required DateTime Date {get; set;}
        public required string PrevName {get; set;}
        public required string PrevStatus {get; set;}
        public required int PrevUser {get; set;}
        public required string PrevDescription {get; set;}

        // [ForeignKey("TicketId")]
        // public required Ticket Ticket { get; set; }

        // [ForeignKey("PrevUser")]
        // public User? User { get; set; }
    }
}
