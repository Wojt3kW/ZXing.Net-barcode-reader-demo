using BarcodeReaderDemo.Models;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace BarcodeReaderDemo.Services
{
    public class BarcodeReaderService
    {
        public List<BarcodeResult> ProcessFile(BarcodeViewModel model)
        {
            string extension = Path.GetExtension(model.UploadedFile!.FileName).ToLower();

            if (extension == ".pdf")
            {
                return ReadBarcodesFromPdf(model.UploadedFile);
            }
            else if (extension is ".jpg" or ".jpeg" or ".png" or ".bmp")
            {
                using var memoryStream = new MemoryStream();
                model.UploadedFile.CopyTo(memoryStream);
                return ReadBarcodesFromImageStream(memoryStream, 1);

            }

            return new List<BarcodeResult>
            {
                new BarcodeResult {
                    BarcodeFormat = "Error",
                    BarcodeText = "Nieobsługiwany format pliku: " + extension
                }
            };
        }


        private List<BarcodeResult> ReadBarcodesFromPdf(IFormFile file)
        {
            var results = new List<BarcodeResult>();

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] pdfBytes = stream.ToArray();

                    // Pobierz liczbę stron
                    var pageSizes = PDFtoImage.Conversion.GetPageSizes(pdfBytes);
                    int pageCount = pageSizes.Count;

                    for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
                    {
                        var size = pageSizes[pageNumber];
                        int newWidth = (int)(size.Width * 2f);
                        int newHeight = (int)(size.Height * 2f);

                        // Ustaw opcje renderowania z powiększeniem 2x
                        var options = new PDFtoImage.RenderOptions
                        {
                            Width = newWidth,
                            Height = newHeight,
                            Dpi = 600,
                        };

                        using (var bitmap = PDFtoImage.Conversion.ToImage(pdfBytes, pageNumber, null, options))
                        using (var imageStream = new MemoryStream())
                        {
                            bitmap.Encode(imageStream, SKEncodedImageFormat.Png, 100);
                            imageStream.Position = 0;
                            results.AddRange(ReadBarcodesFromImageStream(imageStream, pageNumber + 1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wczytywania dokumentu PDF: {ex.Message}");
            }

            if (results.Count == 0)
            {
                results.Add(new BarcodeResult
                {
                    BarcodeFormat = "Info",
                    BarcodeText = "Nie znaleziono kodów kreskowych w pliku PDF lub nie można ich odczytać",
                    PageNumber = "Wszystkie strony"
                });
            }

            return results;
        }


        private List<BarcodeResult> ReadBarcodesFromImageStream(Stream imageStream, int pageNumber)
        {
            List<BarcodeResult> results = new List<BarcodeResult>();

            try
            {
                imageStream.Position = 0;
                using (SKBitmap bitmap = SKBitmap.Decode(imageStream))
                {
                    if (bitmap == null)
                    {
                        results.Add(new BarcodeResult
                        {
                            BarcodeFormat = "Error",
                            BarcodeText = "Nie można odczytać obrazu (nieobsługiwany lub uszkodzony format pliku)",
                            PageNumber = pageNumber.ToString()
                        });
                        return results;
                    }
                    results = DecodeBarcodesFromBitmap(bitmap, pageNumber);
                }

                if (results.Count == 0)
                {
                    results.Add(new BarcodeResult
                    {
                        BarcodeFormat = "Info",
                        BarcodeText = "Nie znaleziono kodów kreskowych w obrazie lub nie można ich odczytać",
                        PageNumber = pageNumber.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                results.Add(new BarcodeResult
                {
                    BarcodeFormat = "Error",
                    BarcodeText = $"Błąd podczas przetwarzania obrazu: {ex.Message}",
                    PageNumber = pageNumber.ToString()
                });
            }

            return results;
        }

        private List<BarcodeResult> DecodeBarcodesFromBitmap(SKBitmap bitmap, int pageNumber)
        {
            var results = new List<BarcodeResult>();

            var barcodeReader = new BarcodeReader();
            barcodeReader.AutoRotate = true;
            barcodeReader.Options.TryHarder = true;
            barcodeReader.Options.TryInverted = true;
            barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>
            {
                BarcodeFormat.All_1D,
                BarcodeFormat.QR_CODE,
                BarcodeFormat.DATA_MATRIX,
                BarcodeFormat.PDF_417,
                BarcodeFormat.AZTEC
            };

            var resultsArray = barcodeReader.DecodeMultiple(bitmap);

            if (resultsArray != null)
            {
                foreach (var result in resultsArray)
                {
                    results.Add(new BarcodeResult
                    {
                        BarcodeFormat = result.BarcodeFormat.ToString(),
                        BarcodeText = result.Text,
                        PageNumber = pageNumber.ToString()
                    });
                }
            }

            return results;
        }

    }
}
