using BarcodeReaderDemo.Models;
using SkiaSharp;
using Syncfusion.PdfToImageConverter;
using ZXing;
using ZXing.SkiaSharp;

namespace BarcodeReaderDemo.Services
{
    public class BarcodeReaderService
    {
        private readonly IWebHostEnvironment _environment;

        public BarcodeReaderService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public List<BarcodeResult> ProcessFile(BarcodeViewModel model)
        {
            string extension = Path.GetExtension(model.UploadedFile!.FileName).ToLower();

            if (extension == ".pdf")
            {
                return ReadBarcodesFromPdf(model.UploadedFile);
            }
            else if (extension is ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif" or ".tiff" or ".tif")
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
                    file.CopyTo(stream);                    // Reset the position of the memory stream to the beginning
                    stream.Seek(0, SeekOrigin.Begin);
                    PdfToImageConverter pdfToImageConverter = new PdfToImageConverter();
                    pdfToImageConverter.Load(stream);

                    // Pobieramy rozmiar strony, aby podwoić rozdzielczość zachowując proporcje
                    for (int pageNumber = 0; pageNumber < pdfToImageConverter.PageCount; pageNumber++)
                    {
                        // Ustawiamy dwukrotne powiększenie przy zachowaniu proporcji
                        int originalWidth = 0;
                        int originalHeight = 0;

                        // Pobieramy oryginalny rozmiar strony (w punktach)
                        using (Stream sizeCheckStream = pdfToImageConverter.Convert(pageNumber, false, false))
                        {
                            sizeCheckStream.Position = 0; // Reset the position of the stream to the beginning
                            using (SKImage tempImg = SKImage.FromEncodedData(sizeCheckStream))
                            {
                                originalWidth = tempImg.Width;
                                originalHeight = tempImg.Height;
                            }
                        }

                        // Podwajamy rozmiary dla lepszego rozpoznawania kodów
                        float newWidth = originalWidth * 2f;
                        float newHeight = originalHeight * 2;

                        // Konwertujemy z powiększeniem
                        Stream imageStream = pdfToImageConverter.Convert(pageNumber, new Syncfusion.Drawing.SizeF(newWidth, newHeight), true, false, false);
                        imageStream.Position = 0;
                        results.AddRange(ReadBarcodesFromImageStream(imageStream, pageNumber + 1));
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
                using (SKBitmap bitmap = SKBitmap.Decode(imageStream))
                {
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
