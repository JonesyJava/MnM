using System.ComponentModel.DataAnnotations;

namespace MnM.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string Specialty { get; set; }
        // CreatorId ONLY NEEDED with Auth0
        public string CreatorId { get; set; }
        //  Creator ONLY NEEDED with AUTH0
        public Profile Creator { get; set; }
    }

    public class AppointmentViewModel : Patient
    {
        public int AppointmentId { get; set; }
    }
}