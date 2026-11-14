using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Participant"; // Default
    }

    public class CreateEventViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(7);

        [Required]
        public string Location { get; set; } = string.Empty;

        public IFormFile? EventImage { get; set; } 
    }

    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int AgeFrom { get; set; }
        [Required]
        public int AgeTo { get; set; }
    }

    public class CaptureResultViewModel
    {
        public int EnrolmentID { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string FinishTime { get; set; } = string.Empty; // e.g., "01:30:00"
        public int Position { get; set; }
    }
}