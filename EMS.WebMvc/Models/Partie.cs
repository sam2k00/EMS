namespace EMS.WebMvc.Models
{
    public class Partie
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int SymbolId { get; set;}
        public Symbol PartiSymbol { get; set; }
    }
}
