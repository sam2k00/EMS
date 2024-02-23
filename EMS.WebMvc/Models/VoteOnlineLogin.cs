using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.WebMvc.Models
{
    public class VoteOnlineLogin
    {
        [Required]
        public int Id { get; set; }

        public string? UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
