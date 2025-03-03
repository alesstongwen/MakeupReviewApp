namespace MakeupReviewApp.Models.ViewModels
{
    public class UserProfile
    {
        public User User { get; set; } 
        public string? ProfilePicture { get; set; } 
        public DateTime JoinDate { get; set; } = DateTime.Now; 
        public List<Review> Reviews { get; set; } = new List<Review>(); 
    }
}
