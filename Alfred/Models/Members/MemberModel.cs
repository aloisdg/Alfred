using Alfred.Shared.Enums;
using Microsoft.Build.Framework;

namespace Alfred.Models.Members
{
    public class MemberModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public CommunityRole Role { get; set; }
    }
}
