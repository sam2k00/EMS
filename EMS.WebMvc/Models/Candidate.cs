using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.WebMvc.Models
{
    public class Candidate
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
        public string? PartyName {  get; set; }

        [NotMapped]
        public string? StateName { get; set; }


        public virtual Partie? Partie { get; set; }
        public virtual States? State { get; set; }

    }
}
