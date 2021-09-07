namespace MnM.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BirthDate { get; set; }
        public string CreatorId { get; set; }
        public Profile Creator { get; set; }
    }
}