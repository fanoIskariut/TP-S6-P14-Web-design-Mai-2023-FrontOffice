using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AI.Models;
[Table("aitype")]
public class AIType
{
    [Key]
    public int id { get; set; } 
    public string nom { get; set; }
    
    public string images { get; set; }
    public string description { get; set; } 
    public string avantages { get; set; } 
    public string exemples { get; set; } 
    
}