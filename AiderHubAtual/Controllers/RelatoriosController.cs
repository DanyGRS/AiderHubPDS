using AiderHubAtual.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly Context _context;

        public RelatoriosController(Context context)
        {
            _context = context;
        }
        // GET: RelatoriosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RelatoriosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RelatoriosController/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: RelatoriosController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, id_Evento, id_Voluntario, NomeVoluntario, NomeONG, DataEvento, CargaHoraria")] Relatorio relatorio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relatorio);
                await _context.SaveChangesAsync();
                return RedirectToAction("Validar", "Home", new { result = ViewBag.resultado, coordinate = ViewBag.coordenadas, distance = ViewBag.distancia });

            }

            return View(relatorio);
        }

        // GET: RelatoriosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RelatoriosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RelatoriosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RelatoriosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
