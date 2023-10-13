using AiderHubAtual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class VoluntariosController : Controller
    {
        private readonly Context _context;

        public VoluntariosController(Context context)
        {
            _context = context;
        }

        // GET: Voluntarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Voluntarios.ToListAsync());
        }

        // GET: Voluntarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voluntario = await _context.Voluntarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voluntario == null)
            {
                return NotFound();
            }

            return View(voluntario);
        }

        // GET: Voluntarios/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Foto,DataNascimento,Cpf,Email,Senha,Telefone,Endereco,Cep,Numero,Uf,Cidade,Bairro,Complemento,Formacao,Sobre,Interesses,Tipo")] Voluntario voluntario)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Usuarios.Any(u => u.Email == voluntario.Email);

                if (existingUser != false)
                {
                    ModelState.AddModelError("Email", "Este e-mail já está sendo usado por outro usuário.");
                    return View(voluntario);
                }

                ValidacaoController.ValidateCPF(voluntario.Cpf);
                voluntario.Tipo = "V";
                _context.Add(voluntario);
                await _context.SaveChangesAsync();


                Usuario usuario = new Usuario
                {
                    Id = voluntario.Id,
                    Email = voluntario.Email,
                    Senha = voluntario.Senha,
                    Tipo = "V",
                    Status = true
                };

                //await CreateUsuarioAsync(usuario);
                var usuarioController = new UsuariosController(_context);
                await usuarioController.Create(usuario);

                return RedirectToAction("Inicial", "Home");
            }
            return View("Inicial", "Home");
        }

        public async Task CreateUsuarioAsync(Usuario usuario)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
        }

        // GET: Voluntarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voluntario = await _context.Voluntarios.FindAsync(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            return View(voluntario);
        }

        // POST: Voluntarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Foto,DataNascimento,Cpf,Email,Senha,Telefone,Endereco,Cep,Numero,Uf,Cidade,Bairro,Complemento,Formacao,Sobre,Interesses,Tipo")] Voluntario voluntario)
        {
            if (id != voluntario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voluntario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoluntarioExists(voluntario.Id))
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
            return View(voluntario);
        }

        // GET: Voluntarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voluntario = await _context.Voluntarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voluntario == null)
            {
                return NotFound();
            }

            return View(voluntario);
        }

        // POST: Voluntarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voluntario = await _context.Voluntarios.FindAsync(id);
            _context.Voluntarios.Remove(voluntario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoluntarioExists(int id)
        {
            return _context.Voluntarios.Any(e => e.Id == id);
        }
    }
}
