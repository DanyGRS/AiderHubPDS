using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiderHubAtual.Models
{
    [Table("inscricao")]
    public class Inscricao
    {
        [Key]
        [Column("id_inscricao")]
        public int InscricaoId { get; set; }
        [Column("id_evento")]
        public int? id_Evento { get; set; }
        [Column("id_voluntario")]
        public int id_Voluntario { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("tipo")]
        public string Tipo { get; set; }
        [Column("confirmacao")]
        public bool Confirmacao { get; set; }

        [Column("data_inscricao")]
        public DateTime DataInscricao { get; set; }

        public virtual ICollection<EventoInscricao> EventosInscricoes { get; set; } = new List<EventoInscricao>();
        public Inscricao() { }

        public Inscricao(int id_inscricao, int id_evento, int id_voluntario, bool status, string tipo, bool confirmacao, DateTime data_inscricao)
        {
            InscricaoId = id_inscricao;
            id_Evento = id_evento;
            id_Voluntario = id_voluntario;
            Status = status;
            Tipo = tipo;
            Confirmacao = confirmacao;
            DataInscricao = data_inscricao;
        }


    }
}
