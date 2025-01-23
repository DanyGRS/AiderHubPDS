using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiderHubAtual.Models
{
    [Table("voluntario")]
    public class Voluntario
    {
        [Key]
        [Column("id_voluntario")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("foto_logo")]
        public string Foto { get; set; }

        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        [Column("telefone")]
        public string Telefone { get; set; }

        [Column("logradouro")]
        public string Endereco { get; set; }

        [Column("cep")]
        public string Cep { get; set; }

        [Column("numero")]
        public string Numero { get; set; }

        [Column("uf")]
        public string Uf { get; set; }

        [Column("cidade")]
        public string Cidade { get; set; }

        [Column("bairro")]
        public string Bairro { get; set; }

        [Column("complemento")]
        public string Complemento { get; set; }

        [Column("formacao")]
        public string Formacao { get; set; }

        [Column("sobre")]
        public string Sobre { get; set; }

        [Column("interesse")]
        public string Interesses { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        public string CpfFormatado => string.IsNullOrEmpty(Cpf) ? "" : Convert.ToUInt64(Cpf).ToString(@"000\.###\.###-##");
        public string DataNascimentoFormatada => DataNascimento.ToString("dd/MM/yyyy");
        public Voluntario() { }

        public Voluntario(int id_voluntario, string nome, string foto_logo, DateTime data_nascimento, string cpf, string email, string senha, string telefone, string endereco, string cep, string numero, string uf, string cidade, string bairro, string complemento, string formacao, string sobre, string interesses, string tipo)
        {
            Id = id_voluntario;
            Nome = nome;
            Foto = foto_logo;
            DataNascimento = data_nascimento;
            Cpf = cpf;
            Email = email;
            Senha = senha;
            Telefone = telefone;
            Endereco = endereco;
            Cep = cep;
            Numero = numero;
            Uf = uf;
            Cidade = cidade;
            Bairro = bairro;
            Complemento = complemento;
            Formacao = formacao;
            Sobre = sobre;
            Interesses = interesses;
            Tipo = tipo;
        }
    }
}
