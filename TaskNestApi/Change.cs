namespace TaskNestApi.Models
{
    public class Change
    {
        public required int Id {get; set;} 
        public required int TicketId {get; set;}
        public required DateOnly Date {get; set;}
        public required string PrevName {get; set;}
        public required string PrevStatus {get; set;}
        public required int PrevUser {get; set;}
        public required string PrevDescription {get; set;}
    }
}
