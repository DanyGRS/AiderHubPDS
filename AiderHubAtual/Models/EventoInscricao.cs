using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Models
{
    [Table("eventoinscricao")]
    public class EventoInscricao
    {
        [Key]
        [Column("id_ei")]
        public int id_EI { get; set; }

        [Column("id_evento")]
        public int id_Evento { get; set; }

        [Column("id_inscricao")]
        [ForeignKey("id_inscricao")]
        public int InscricaoId { get; set; }

        [ForeignKey("id_Evento")]
        public virtual Evento evento { get; set; }
        public virtual Inscricao inscricao { get; set; }

        public EventoInscricao()
        {

        }
        public EventoInscricao(int id_ei, int id_evento, int id_inscricao)
        {
            id_EI = id_ei;
            id_Evento = id_evento;
            InscricaoId = id_inscricao;
        }
    }
}
