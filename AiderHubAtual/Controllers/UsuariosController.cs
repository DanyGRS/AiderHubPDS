using AiderHubAtual.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Context _context;

        public UsuariosController(Context context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Senha,Status,Tipo")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Senha,Status,Tipo")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha)
        {
            // Verificar se o email e a senha estão corretos
            bool loginValido = VerificarCredenciais(email, senha);

            if (loginValido)
            {
                // Login bem-sucedido, redirecionar para a página principal
                int userId = ObterUserId(email);
                string userTipo = ObterTipo(email);
                HttpContext.Session.SetInt32("IdUser", userId);
                HttpContext.Session.SetString("IdTipo", userTipo);
                return RedirectToAction("Index", "Home", new { id = userId, tipo = userTipo });
            }
            else
            {
                // Credenciais inválidas, exibir mensagem de erro
                ModelState.AddModelError(string.Empty, "Email ou senha inválidos");
                ViewBag.Mensagem = "Senha ou Email estão invalidos";
                return View("LoginPage");
            }
        }

        private bool VerificarCredenciais(string email, string senha)
        {
            // Buscar o registro de usuário com o email informado
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                // Verificar se a senha fornecida corresponde à senha do usuário
                if (usuario.Senha == senha)
                {
                    // Credenciais válidas
                    return true;
                }
            }

            // Credenciais inválidas
            return false;
        }

        private int ObterUserId(string email)
        {
            // Buscar o registro de usuário com o email informado
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                // Retornar o userId
                return usuario.Id;
            }

            return 0; // ou outro valor indicando que o userId não foi encontrado
        }
        private string ObterTipo(string email)
        {
            // Buscar o registro de usuário com o email informado
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                // Retornar o userId
                return usuario.Tipo;
            }

            return " "; // ou outro valor indicando que o userId não foi encontrado
        }

        public IActionResult PaginaInicial()
        {
            return View();
        }        
        public IActionResult SobreNos()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }
        public IActionResult ongVol()
        {
            return View();
        }
    }
}
