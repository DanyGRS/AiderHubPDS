using AiderHubAtual.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class InscricoesController : Controller
    {
        private readonly Context _context;

        public InscricoesController(Context context)
        {
            _context = context;
        }

        // GET: Inscricoes
        public async Task<IActionResult> Index()
        {
            int idUser = HttpContext.Session.GetInt32("IdUser") ?? 0;

            // Filtrar as inscrições pelo valor de id_Voluntario
            // Parei Aqui
            var inscricoes = await _context.Inscricoes
                .Where(i => i.id_Voluntario == idUser)
                .Include(i => i.EventosInscricoes)
                .ThenInclude(navigationPropertyPath: p => p.evento)
                .ToListAsync();
    
                //.ThenInclude(o => o.evento)

             //   .Include(i => i.Eventos).ToListAsync();


            return View(inscricoes);
        }


        // GET: Inscricoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricoes
                .Include(i => i.EventosInscricoes)
                .ThenInclude(navigationPropertyPath: p => p.evento)
                .FirstOrDefaultAsync(m => m.InscricaoId == id);
            if (inscricao == null)
            {
                return NotFound();
            }

            return View(inscricao);
        }

        // GET: Inscricoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inscricoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InscricaoId,id_Evento,id_Voluntario,Status,Tipo,Confirmacao,DataInscricao")] Inscricao inscricao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inscricao);
                await _context.SaveChangesAsync();

                EventoInscricao ins = new EventoInscricao
                {
                    id_Evento = (int)inscricao.id_Evento,
                    InscricaoId = inscricao.InscricaoId,
                };
                _context.Add(ins);
                await _context.SaveChangesAsync();
            }
            return View(inscricao);
        }

        // GET: Inscricoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound();
            }
            return View(inscricao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InscricaoId,id_Evento,id_Voluntario,Status,Tipo,Confirmacao,DataInscricao")] Inscricao inscricao)
        {

            if (id != inscricao.InscricaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscricao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscricaoExists(inscricao.InscricaoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inscricao);
        }


        public async Task<IActionResult> DeletarInscricao(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                     
            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound();
            }
            foreach (var eventoins in _context.EventosInscricoes.Where(x => x.id_Evento == inscricao.id_Evento))
            {
                _context.EventosInscricoes.Remove(eventoins);
            }
            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscricaoExists(int id)
        {
            return _context.Inscricoes.Any(e => e.InscricaoId == id);
        }


    }
}
