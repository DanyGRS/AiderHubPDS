using AiderHubAtual.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class HomeController : Controller
    {
        public bool VoluntarioLogado { get; set; }
        private readonly OpenStreetMapService _openStreetMapService;
        private readonly Context _context;
        public HomeController(Context context)
        {
            _openStreetMapService = new OpenStreetMapService();
            _context = context;
        }

        public ActionResult Inicial()
        {
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string userTipo = HttpContext.Session.GetString("IdTipo");

            if (idUser.HasValue && !string.IsNullOrEmpty(userTipo))
            {
                return RedirectToAction("Index", new { id = idUser.Value, tipo = userTipo });
            }

            return RedirectToAction("LoginPage", "Usuarios");
        }
        public ActionResult Index(int id, string tipo)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id && u.Tipo == tipo);

            if (usuario.Tipo == "V")
            {
                bool voluntarioLogado = (usuario != null && usuario.Tipo == "V");

                ViewBag.VoluntarioLogado = voluntarioLogado;
            }
            else
            {
                bool voluntarioLogado = false;
                ViewBag.VoluntarioLogado = voluntarioLogado;
            }
            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public IActionResult GetEndereco(int id_Evento)
        {
            var evento = _context.Eventos.FirstOrDefault(e => e.id_Evento == id_Evento);

            if (evento == null)
            {
                return NotFound(); // Evento não encontrado
            }

            var address = evento.Logradouro + "  " + evento.Numero + "  " + evento.Bairro + "  " + evento.CEP + "  " + evento.Cidade + "  " + evento.UF;

            return Ok(address);
        }

        public ActionResult Endereco(string address, string deviceLatitude, string deviceLongitude, int id_Evento)
        {
            Coordinates coordinates = _openStreetMapService.GetCoordinates(address);

            ViewBag.Address = address;

            if (coordinates != null)
            {
                ViewBag.Latitude = coordinates.Latitude;
                ViewBag.Longitude = coordinates.Longitude;

                return RedirectToAction("Resultado", new
                {
                    databaseLatitude = ViewBag.Latitude,
                    databaseLongitude = ViewBag.Longitude,
                    deviceLatitude,
                    deviceLongitude,
                    id_Evento
                });
                //return View();
                // return RedirectToAction("Device", new { databaseLat = ViewBag.Latitude, databaseLone = ViewBag.Longitude });
            }
            else
            {
                // não achou / deu erro
                ViewBag.ErrorMessage = "Não foi possivel realizar check-in";
            }
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(int id_Evento, string address, string deviceLatitude, string deviceLongitude)
        {
            return RedirectToAction("Endereco", new { address, deviceLatitude, deviceLongitude, id_Evento });
        }

        [HttpGet]
        [HttpPost]
        public ActionResult Resultado(string databaseLatitude, string databaseLongitude, string deviceLatitude, string deviceLongitude, int id_Evento)
        {
            int idUser = HttpContext.Session.GetInt32("IdUser") ?? 0;
            double parsedDeviceLatitude = double.Parse(deviceLatitude, CultureInfo.InvariantCulture);
            double parsedDeviceLongitude = double.Parse(deviceLongitude, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(databaseLatitude) || string.IsNullOrEmpty(databaseLongitude))
            {
                return View("Eventos/Index");
            }

            double parsedDataBaselatitude = double.Parse(databaseLatitude, CultureInfo.InvariantCulture);
            double parsedDataBaselongitude = double.Parse(databaseLongitude, CultureInfo.InvariantCulture);

            double distanceInMeters = CalculateDistance(parsedDeviceLatitude, parsedDeviceLongitude, parsedDataBaselatitude, parsedDataBaselongitude);

            if (distanceInMeters <= 4000)
            {
                ViewBag.resultado = "DENTRO DO RAIO, CHECK-IN REALIZADO COM SUCESSO!";
                //ViewBag.coordenadas = $"{parsedDeviceLatitude}, {parsedDeviceLongitude}";
                //ViewBag.distancia = distanceInMeters;

                var inscricao = _context.Inscricoes.FirstOrDefault(i => i.id_Evento == id_Evento && i.id_Voluntario == idUser);

                if (inscricao != null)
                {
                    if (inscricao.Confirmacao == true)
                    {
                        ViewBag.Mensagem = "Você já fez check-in nesse evento!";
                        return RedirectToAction("Inscricao", "Eventos", new { result = ViewBag.Mensagem });
                    }
                    else
                    {
                        inscricao.Confirmacao = true;
                        _context.SaveChanges();
                    }
                }

                return RedirectToAction("Validar", new { result = ViewBag.resultado});
            }
            else
            {
                ViewBag.resultado = "FORA DO RAIO, CHECK-IN INVÁLIDO!";
                ViewBag.resultados = "Tente novamente quando estiver no local do evento";

                return RedirectToAction("Validar", new { result = ViewBag.resultado, results = ViewBag.resultados }) ;
            }
        }

        public ActionResult Validar(string result, string coordinate, double distance)
        {
            ViewBag.Result = result;
            ViewBag.Coordinate = coordinate;
            ViewBag.Distance = distance;
            return View();
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadius = 6371000; // in meters

            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadius * c;

            return distance;
        }

        public ActionResult Saindo()
        {
            HttpContext.Session.Clear();

            // Redireciona para a página de login
            return RedirectToAction("LoginPage", "Usuarios");
        }

    }
}
