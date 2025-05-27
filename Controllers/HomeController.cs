using BarcodeReaderDemo.Models;
using BarcodeReaderDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BarcodeReaderDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BarcodeReaderService _barcodeReaderService;

        public HomeController(ILogger<HomeController> logger, BarcodeReaderService barcodeReaderService)
        {
            _logger = logger;
            _barcodeReaderService = barcodeReaderService;
        }

        public IActionResult Index()
        {
            return View(new BarcodeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(BarcodeViewModel model)
        {
            if (model.UploadedFile == null || model.UploadedFile.Length == 0)
            {
                model.ErrorMessage = "Proszę wybrać plik";
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.UploadedFile.FileName).ToLower();
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                model.ErrorMessage = "Dozwolone są tylko pliki PDF i obrazy (jpg, png, bmp)";
                return View(model);
            }

            try
            {
                model.Results = _barcodeReaderService.ProcessFile(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas przetwarzania pliku");
                model.ErrorMessage = $"Wystąpił błąd podczas przetwarzania pliku: {ex.Message}";
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
