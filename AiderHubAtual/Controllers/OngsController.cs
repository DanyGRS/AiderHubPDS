using AiderHubAtual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class OngsController : Controller
    {
        private readonly Context _context;

        public OngsController(Context context)
        {
            _context = context;
        }

        // GET: Ongs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ongs.ToListAsync());
        }

        // GET: Ongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ong = await _context.Ongs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ong == null)
            {
                return NotFound();
            }

            return View(ong);
        }

        // GET: Ongs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RazaoSocial,NomeFantasia,Cnpj,Email,Senha,AssinaturaDigital,Telefone,Endereco,CEP,Numero,UF,Cidade,Complemento,Bairro,FotoLogo,Tipo")] Ong ong)
        {
            if (ModelState.IsValid)
            {
                // Verificar se já existe um registro com o mesmo e-mail no banco de dados
                var existingUser = _context.Usuarios.Any(u => u.Email == ong.Email);

                if (existingUser != false)
                {
                    ModelState.AddModelError("Email", "Este e-mail já está sendo usado por outro usuário.");
                    return View(ong);
                }

                ong.Tipo = "O";
                _context.Add(ong);
                await _context.SaveChangesAsync();

                Usuario usuario = new Usuario
                {
                    Id = ong.Id,
                    Email = ong.Email,
                    Senha = ong.Senha,
                    Tipo = "O",
                    Status = true
                };

                var usuarioController = new UsuariosController(_context);
                await usuarioController.Create(usuario);

                return RedirectToAction("Inicial", "Home");
            }

            return View("Inicial", "Home");

        }

        // GET: Ongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ong = await _context.Ongs.FindAsync(id);
            if (ong == null)
            {
                return NotFound();
            }
            return View(ong);
        }

        // POST: Ongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RazaoSocial,NomeFantasia,Cnpj,Email,Senha,AssinaturaDigital,Telefone,Endereco,CEP,Numero,UF,Cidade,Complemento,Bairro,FotoLogo,Tipo")] Ong ong)
        {
            if (id != ong.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OngExists(ong.Id))
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
            return View(ong);
        }

        // GET: Ongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ong = await _context.Ongs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ong == null)
            {
                return NotFound();
            }

            return View(ong);
        }

        // POST: Ongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ong = await _context.Ongs.FindAsync(id);
            _context.Ongs.Remove(ong);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OngExists(int id)
        {
            return _context.Ongs.Any(e => e.Id == id);
        }
    }
}

