using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace blogpessoal.model
{
    public class Tema 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonIgnore]
        [InverseProperty("Tema")]
       
        public virtual ICollection<Postagem>? Postagem { get; set; }
    }
}
