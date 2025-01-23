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
        private readonly PasswordHasherService _password;

        public UsuariosController(Context context, PasswordHasherService password)
        {
            _context = context;
            _password = password;
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

        public IActionResult Create()
        {
            return View();
        }

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
            bool loginValido = VerificarCredenciais(email, senha);

            if (loginValido)
            {
                int userId = ObterUserId(email);
                string userTipo = ObterTipo(email);
                HttpContext.Session.SetInt32("IdUser", userId);
                HttpContext.Session.SetString("IdTipo", userTipo);
                return RedirectToAction("Index", "Home", new { id = userId, tipo = userTipo });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email ou senha inválidos");
                ViewBag.Mensagem = "Senha ou Email estão invalidos";
                return View("LoginPage");
            }
        }

        private bool VerificarCredenciais(string email, string senha)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            var senhaDescriptografada = _password.VerifyPassword(usuario.Senha, senha);
            return senhaDescriptografada;
        }

        private int ObterUserId(string email)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                return usuario.Id;
            }

            return 0;
        }
        private string ObterTipo(string email)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                return usuario.Tipo;
            }
            return " ";
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
