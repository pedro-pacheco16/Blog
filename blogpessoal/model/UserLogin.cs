using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace blogpessoal.model
{
    public class UserLogin
    {
        public long id { get; set; }

        
        public string Nome { get; set; } = string.Empty;

       
        public string Usuario { get; set; } = string.Empty;

      
        public string Senha { get; set; } = string.Empty;

       
        public string? Foto { get; set; } = string.Empty;

       
        public string Token { get; set; } = string.Empty;


    }
}

