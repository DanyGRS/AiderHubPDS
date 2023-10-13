using AiderHubAtual.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace AiderHubAtual.Controllers
{
    public class EventosController : Controller
    {
        private readonly Context _context;

        public EventosController(Context context)
        {
            _context = context;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventosAtivos = await _context.Eventos.Where(s => s.Status != false).ToListAsync();

            return View(eventosAtivos);
        }


        public async Task<IActionResult> IndexOng()
        {
            int idUser = HttpContext.Session.GetInt32("IdUser") ?? 0;

            // Filtrar as inscrições pelo valor de id_Voluntario
            var eventos = await _context.Eventos
                .Where(e => e.IdOng == idUser)
                .ToListAsync();

            return View(eventos);
        }


        public IActionResult Inscricao(string result)
        {
            ViewBag.Mensagem = result;
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.id_Evento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        public async Task<IActionResult> DetailsVol(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.id_Evento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_Evento,Data_Evento,Hora_Evento,Logradouro,CEP,Numero,UF,Complemento,Cidade,Bairro,Carga_Horaria,Descricao,Responsavel,Status,IdOng,Titulo")] Evento evento)
        {
            int idUser = HttpContext.Session.GetInt32("IdUser") ?? 0;
            if (ModelState.IsValid)
            {
                evento.IdOng = idUser;
                evento.Status = true;
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexOng));
            }
            return View(evento);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_Evento,Data_Evento,Hora_Evento,Logradouro,Cep,Numero,Uf,Complemento,Cidade,Bairro,Carga_Horaria,Descricao,Responsavel,Status,IdOng,Titulo")] Evento evento)
        {
            int idUser = HttpContext.Session.GetInt32("IdUser") ?? 0;
            if (id != evento.id_Evento)
            {
                return NotFound();
            }

            var dadosEvento = _context.Eventos
                .FirstOrDefault(e => e.id_Evento == id);

            _context.Attach(evento);

            if (ModelState.IsValid)
            {
                try
                {
                    evento.IdOng = idUser;

                    // Obtenha a data atual
                    var dataAtual = DateTime.Now;
                    var dataEvento = dadosEvento.Data_Evento.Date + dadosEvento.Hora_Evento;
                    
                    // Verifique se a data do evento está dentro do prazo de dois dias antes do evento
                    if (dataEvento.AddDays(-2) >= dataAtual)
                    {
                        evento.Status = true;
                        try
                        {
                            _context.Update(evento);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));

                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "A edição não ocorreu, entre em contato com o suporte!");
                            return View(evento);
                        }
                    }
                    else
                    {
                        // Data do evento está fora do prazo
                        ModelState.AddModelError(string.Empty, "Você só pode editar o evento até dois dias antes da data do evento.");
                        return View(evento);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.id_Evento))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(evento);
        }


        public async Task<IActionResult> DeletarEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var insc in _context.Inscricoes.Where(x => x.id_Evento == evento.id_Evento))
                {
                    _context.Inscricoes.Remove(insc);
                }
                foreach (var eventoins in _context.EventosInscricoes.Where(x => x.id_Evento == evento.id_Evento))
                {
                    _context.EventosInscricoes.Remove(eventoins);
                }

                _context.Eventos.Remove(evento);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocorreu um erro ao excluir o evento: {ex.Message}";
            }
            return RedirectToAction(nameof(IndexOng));
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.id_Evento == id);
        }

        public async Task<IActionResult> InscreverEvento(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.id_Evento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return RedirectToAction("InscreverConfirmed", new { id });
        }

        public async Task<IActionResult> InscreverConfirmed(int id)
        {
            // Use os valores recebidos para criar a inscrição
            //var idUser = (int)ViewData["IdUser"];
            int idUser = HttpContext.Session.GetInt32("IdUser") ?? 0;

            var existingInscricao = await _context.Inscricoes
            .FirstOrDefaultAsync(i => i.id_Evento == id && i.id_Voluntario == idUser);

            if (existingInscricao != null)
            {
                // Já existe uma inscrição com os mesmos valores, faça o tratamento necessário
                ViewBag.Mensagem = "Você já está inscrito.";
                return View("Inscricao"); // Redireciona para a página desejada
            }

            Inscricao inscricao = new Inscricao
            {
                id_Evento = id,
                id_Voluntario = idUser,
                Status = true,
                Tipo = "V",
                Confirmacao = false,
                DataInscricao = DateTime.Today
            };
            //EventoInscricao eventI = new EventoInscricao
            //{
            //    id_Evento = id,
            //    InscricaoId = inscricao.InscricaoId
            //};

           var inscricaoController = new InscricoesController(_context);
            await inscricaoController.Create(inscricao);
            // Faça o processamento necessário com a inscrição

            ViewBag.Mensagem = "Inscrição Confirmada";

            return View("Inscricao");
        }

        public async Task<IActionResult> Encerrar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.id_Evento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return RedirectToAction("EncerrarEvento", new { id });
        }
        public async Task<IActionResult> EncerrarEvento(int id)
        {

            var evento = await _context.Eventos
            .FirstOrDefaultAsync(e => e.id_Evento == id);

            evento.Status = false;
            _context.Update(evento);
            _context.SaveChanges();

            Comparecimento(id);


            return RedirectToAction("IndexOng", "Eventos");
        }

        //Metodo que recebe todos os voluntarios que compareceram
        public List<Voluntario> Comparecimento(int idEvento)
        {
            var voluntariosInscritos = _context.Inscricoes
                .Where(i => i.id_Evento == idEvento && i.Confirmacao) // Filtrar inscrições para o evento específico
                .Select(i => i.id_Voluntario)// Selecionar os IDs dos voluntários inscritos
                .ToList();

            var voluntariosComparecimento = _context.Voluntarios
                .Where(v => voluntariosInscritos.Contains(v.Id)) // Filtrar voluntários inscritos
                .ToList();

            EnviaDados(voluntariosComparecimento, idEvento);

            return voluntariosComparecimento; // retornou os voluntarios que confirmaram a incricao
        }

        private void EnviaDados(List<Voluntario> voluntarios, int idEvento)
        {
            List<Relatorio> relatorios = new List<Relatorio>();//relatórios a serem criados
            var evento = _context.Eventos.FirstOrDefault(e => e.id_Evento == idEvento); //qual foi o evento
            var ong = _context.Ongs.FirstOrDefault(o => o.Id == evento.IdOng); // a ong que realizou o evento


            foreach (var cadaInscrito in voluntarios)
            {
                Relatorio relatorio = new Relatorio();
                relatorio.NomeVoluntario = cadaInscrito.Nome;//testar com mais voluntarios para ver se os dados permanecem
                relatorio.NomeONG = ong.NomeFantasia;
                relatorio.DataEvento = evento.Data_Evento;
                relatorio.CargaHoraria = evento.Carga_Horaria;
                relatorio.Email = cadaInscrito.Email;

                relatorios.Add(relatorio);
            }

            LimpaExcelRel(relatorios.Count());
            PreencheExcel(relatorios);

        }

        //Metodo para limpar o excel
        protected void LimpaExcelRel(int quantidade)
        {
            string nomeArquivo = "MacroCertificado.xlsm";
            string diretorioAtual = Directory.GetCurrentDirectory();
            string caminho = Path.Combine(diretorioAtual, "Relatorio", nomeArquivo);

            // Inicialize o aplicativo do Excel
            Microsoft.Office.Interop.Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false; // Você pode torná-lo visível se desejar

            // Abra o arquivo Excel
            Excel.Workbook workbook = excelApp.Workbooks.Open(caminho);

            // Acesse a primeira planilha (índice 1)
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            int maxRows = 2; // Número máximo de linhas
            int maxColumns = quantidade; // Número máximo de colunas (coluna J é a 10ª)

            // Limpar o conteúdo das células até a coluna J e 15 linhas
            for (int row = 1; row <= maxRows; row++) // verifique se apaga os cabeçalhos
            {
                for (int col = 1; col <= maxColumns; col++)
                {
                    ((Excel.Range)worksheet.Cells[row, col]).ClearContents();
                }
            }

            // Salvar e fechar o arquivo Excel
            workbook.Save();
            workbook.Close();
            excelApp.Quit();

            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);


        }

        public void PreencheExcel(List<Relatorio> relatorios)
        {
            // Salve o arquivo Excel
            // Salve o arquivo Excel
            string nomeArquivo = "MacroCertificado.xlsm";
            string diretorioAtual = Directory.GetCurrentDirectory();
            string caminho = Path.Combine(diretorioAtual, "Relatorio", nomeArquivo);

            // Inicialize o aplicativo do Excel
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;

            Excel.Workbook workbook = excelApp.Workbooks.Open(caminho);

            // Obtenha a planilha existente pelo nome
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets["relatorio"];

            worksheet.Cells[1, 1].Value = "nome_voluntario";
            worksheet.Cells[1, 2].Value = "nome_ong";
            worksheet.Cells[1, 3].Value = "data_evento";
            worksheet.Cells[1, 4].Value = "carga_horaria";
            worksheet.Cells[1, 5].Value = "email";

            // Adicione os dados aos campos correspondentes
            int linha = 2; // Começa na linha 2 após os cabeçalhos
            foreach (var relatorio in relatorios)
            {
                worksheet.Cells[linha, 1].Value = relatorio.NomeVoluntario;
                worksheet.Cells[linha, 2].Value = relatorio.NomeONG;
                worksheet.Cells[linha, 3].Value = relatorio.DataEvento;
                worksheet.Cells[linha, 4].Value = relatorio.CargaHoraria.Hours;//arrumar o tipo de dado da carga horaria
                worksheet.Cells[linha, 5].Value = relatorio.Email;//arrumar o tipo de dado da carga horaria
                linha++;
            }

            // Salve o arquivo Excel
            workbook.Save();
            workbook.Close();
            // Feche o aplicativo do Excel
            excelApp.Quit();

            // Libere os objetos COM
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);

            GeraCertificados(caminho);
        }

        protected void GeraCertificados(string caminho)
        {
            Application xlApp = new Application();

            if (xlApp == null)
            {
                ViewBag.Mensagem = "Erro ao executar a macro: aplicativo Excel não encontrado.";
                //return View("Relatorio");
            }

            Workbook xlWorkbook = xlApp.Workbooks.Open(caminho, ReadOnly: false);

            try
            {
                xlApp.Visible = false;
                xlApp.Run("GerarCertificado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = "Erro ao executar a macro.";
                //return View("Relatorio");
            }

            xlWorkbook.Close(false);
            xlApp.Application.Quit();
            xlApp.Quit();


            ViewBag.Mensagem = "Arquivo gerado com sucesso!";
            //return View("Relatorio");
        }
    }
}
