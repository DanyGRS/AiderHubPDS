using System.ComponentModel.DataAnnotations.Schema;

namespace AiderHubAtual.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Column("id_usuario")]
        public int Id { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("senha")]
        public string Senha { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("tipo")]
        public string Tipo { get; set; }

        public Usuario() { }

        public Usuario(int id, string email, string senha, bool status, string tipo)
        {
            Id = id;
            Email = email;
            Senha = senha;
            Status = status;
            Tipo = tipo;
        }
    }
}