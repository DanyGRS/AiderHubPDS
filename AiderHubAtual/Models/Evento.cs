using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiderHubAtual.Models
{
    [Table("evento")]
    public class Evento
    {
        [Key]
        [Column("id_evento")]
        public int id_Evento { get; set; }

        [DisplayName("Data do Evento: ")]
        [Column("data_evento")]
        public DateTime Data_Evento { get; set; }

        [DisplayName("Hora do Evento: ")]
        [Column("hora_evento")]
        public TimeSpan Hora_Evento { get; set; }

        [DisplayName("Logradouro: ")]
        [Column("logradouro")]
        public string Logradouro { get; set; }

        [DisplayName("CEP: ")]
        [Column("cep")]
        public string CEP { get; set; }

        [DisplayName("Número: ")]
        [Column("numero")]
        public string Numero { get; set; }

        [DisplayName("UF: ")]
        [Column("uf")]
        public string UF { get; set; }

        [DisplayName("Complemento: ")]
        [Column("complemento")]
        public string Complemento { get; set; }

        [DisplayName("Cidade: ")]
        [Column("cidade")]
        public string Cidade { get; set; }

        [DisplayName("Bairro: ")]
        [Column("bairro")]
        public string Bairro { get; set; }

        [DisplayName("Carga Horária: ")]
        [Column("carga_horaria")]
        public TimeSpan Carga_Horaria { get; set; }

        [DisplayName("Descrição: ")]
        [Column("descricao")]
        public string Descricao { get; set; }

        [DisplayName("Responsável: ")]
        [Column("responsavel")]
        public string Responsavel { get; set; }

        [Column("id_ong")]
        public int IdOng { get; set; }
        [Column("status")]
        public bool Status { get; set; }

        [Column("titulo_evento")]
        public string Titulo { get; set; }

        
        //[Column("id_inscricao")]
        //public int InscricaoId { get; set; }

        //public virtual ICollection<EventoInscricao> EventosInscricoes { get; set; } = new List<EventoInscricao>();
        public Evento() { }

        public Evento(int id_evento, DateTime data_evento, TimeSpan hora_evento, string logradouro, string cep, string numero, string uf, string complemento, string cidade, string bairro, TimeSpan carga_horaria, string descricao, string responsavel, bool status, int id_ong, string titulo_evento)
        {
            id_Evento = id_evento;
            Data_Evento = data_evento;
            Hora_Evento = hora_evento;
            Logradouro = logradouro;
            CEP = cep;
            Numero = numero;
            UF = uf;
            Complemento = complemento;
            Cidade = cidade;
            Bairro = bairro;
            Carga_Horaria = carga_horaria;
            Descricao = descricao;
            Responsavel = responsavel;
            Status = status;
            IdOng = id_ong;
            Titulo = titulo_evento;
            //InscricaoId = id_inscricao;
        }
    }
}
