using System.ComponentModel.DataAnnotations;

namespace BarcodeReaderDemo.Models
{
    public class BarcodeViewModel
    {
        [Display(Name = "Plik (PDF lub obraz)")]
        public IFormFile? UploadedFile { get; set; }

        public string? FilePath { get; set; }

        public List<BarcodeResult> Results { get; set; } = new List<BarcodeResult>();

        public string? ErrorMessage { get; set; }

        public bool HasResults => Results != null && Results.Count > 0;
    }

    public class BarcodeResult
    {
        public string BarcodeFormat { get; set; } = "";

        public string BarcodeText { get; set; } = "";

        public string PageNumber { get; set; } = "";
    }
}
