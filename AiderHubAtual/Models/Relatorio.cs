using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiderHubAtual.Models
{
    //[Table("relatorio")]
    public class Relatorio
    {
        //[Key]
        //[Column("id")]
        public int Id { get; set; }
        //[Column("id_evento")]
        public int id_Evento { get; set; }
        //[Column("id_voluntario")]
        public int id_Voluntario { get; set; }
        //[Column("nome_voluntario")]
        public string NomeVoluntario { get; set; }
        //[Column("nome_ong")]
        public string NomeONG { get; set; }
        //[Column("data_evento")]
        public DateTime DataEvento { get; set; }
        //[Column("carga_horaria")]
        public TimeSpan CargaHoraria { get; set; }
        public string Email { get; set; }


        public Relatorio() { }
        public Relatorio(int id, int id_evento, int id_voluntario, string nome_voluntario, string nome_ong, DateTime data_evento, TimeSpan carga_horaria, string email)
        {
            Id = id;
            id_Evento = id_evento;
            id_Voluntario = id_voluntario;
            NomeVoluntario = nome_voluntario;
            NomeONG = nome_ong;
            DataEvento = data_evento;
            CargaHoraria = carga_horaria;
            Email = email;
        }
    }

}
