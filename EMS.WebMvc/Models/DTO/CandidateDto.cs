using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebMvc.Models.DTO
{
    public class CandidateDto
    {
        public int CandidateId { get; set; }
        [Required]
        public string Name { get; set; }

        [DisplayName("Party ID")]
        [Required(ErrorMessage = "Please select the Party")]
        public int PartyId { get; set; }
        [Required]
        public int? StateId { get; set; }

        [NotMapped]
        public string? PartyName { get; set; }

        [NotMapped]
        public string? StateName { get; set; }

        public virtual Partie? Partie { get; set; }
        public virtual States? State { get; set; }
    }
}
