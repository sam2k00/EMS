namespace EMS.WebMvc.Models
{
	public class Voter
	{
		public int VoterId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Photo { get; set; }
		public bool Status { get; set; }		
		public int StateId { get; set;}
		
	}
}
