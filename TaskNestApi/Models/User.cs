namespace TaskNestApi.Models
{
    public class User
    {
        public required int Id {get; set;} 
        public required string Name {get; set;}
        public required string Password {get; set;}
        public required bool IsAdmin {get; set;} = false;
    }
}