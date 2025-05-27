# Barcode Reader Demo

Web application for reading barcodes and QR codes from PDF files and images.

[Polski](#barcode-reader-demo-pl) | [English](#barcode-reader-demo-en)

---

<a id="barcode-reader-demo-en"></a>
## Barcode Reader Demo [EN]

### Features

- Reading barcodes and QR codes from PDF files and images (JPG, PNG, BMP, GIF, TIFF)
- Support for multi-page PDF documents
- Support for various code types:
  - 1D barcodes (EAN-13, CODE128, CODE39, etc.)
  - 2D codes (QR Code, Data Matrix, Aztec, PDF417)
- Automatic detection of multiple codes on a single image
- Enhanced recognition quality through 2x image enlargement
- Support for image rotation for better code recognition

---

<a id="barcode-reader-demo-pl"></a>
## Barcode Reader Demo [PL]

### Funkcjonalności

- Odczyt kodów kreskowych i QR z plików PDF oraz obrazów (JPG, PNG, BMP, GIF, TIFF)
- Obsługa wielu stron w dokumentach PDF
- Obsługa różnych typów kodów:
  - Kody kreskowe 1D (EAN-13, CODE128, CODE39 i inne)
  - Kody 2D (QR Code, Data Matrix, Aztec, PDF417)
- Automatyczne wykrywanie wielu kodów na jednym obrazie
- Zwiększona jakość rozpoznawania dzięki dwukrotnemu powiększeniu obrazów
- Obsługa rotacji obrazów dla lepszego rozpoznawania kodów

### Technologies [EN]

The application was built using the following technologies:

- **ASP.NET Core 8.0** - framework for building modern web applications
- **ZXing.Net** - open-source library for reading barcodes and QR codes
- **PDFtoImage** - library for converting PDF files to images
- **Bootstrap 5** - CSS framework for responsive user interface
- **Docker** - containerization for easy deployment

### Technologie [PL]

Aplikacja została zbudowana z wykorzystaniem następujących technologii:

- **ASP.NET Core 8.0** - framework do budowania nowoczesnych aplikacji webowych
- **ZXing.Net** - biblioteka open-source do odczytu kodów kreskowych i QR
- **PDFtoImage** - biblioteka do konwersji plików PDF na obrazy
- **Bootstrap 5** - framework CSS dla responsywnego interfejsu użytkownika
- **Docker** - konteneryzacja dla łatwego wdrażania

### System Requirements [EN]

- .NET 8.0 SDK or higher
- In case of running in Docker container: Docker Desktop or Docker Engine

### Running the Application [EN]

#### Locally

1. Clone the repository
2. Navigate to the project directory
3. Run the application using the command:

```
dotnet run
```

4. Open a browser and go to: `https://localhost:5001` or `http://localhost:5000`

#### Docker

1. Build the Docker image:

```
docker build -t barcode-reader-demo .
```

2. Run the container:

```
docker run -p 8080:8080 -p 8081:8081 barcode-reader-demo
```

3. Open a browser and go to: `http://localhost:8080`

### Wymagania systemowe [PL]

- .NET 8.0 SDK lub wyższy
- W przypadku uruchomienia w kontenerze Docker: Docker Desktop lub Docker Engine

### Uruchomienie aplikacji [PL]

#### Lokalnie

1. Sklonuj repozytorium
2. Przejdź do katalogu projektu
3. Uruchom aplikację za pomocą polecenia:

```
dotnet run
```

4. Otwórz przeglądarkę i przejdź do adresu: `https://localhost:5001` lub `http://localhost:5000`

#### Docker

1. Zbuduj obraz Docker:

```
docker build -t barcode-reader-demo .
```

2. Uruchom kontener:

```
docker run -p 8080:8080 -p 8081:8081 barcode-reader-demo
```

3. Otwórz przeglądarkę i przejdź do adresu: `http://localhost:8080`

### How to Use [EN]

1. Open the application in a browser
2. Select a PDF file or an image containing barcodes/QR codes
3. Click the "Upload and Analyze" button
4. The code reading results will be displayed in a table

> **Note:** PDF files are converted to images using the PDFtoImage library for improved barcode recognition.

### Limitations [EN]

- Recognition quality depends on the clarity of codes and image quality
- For PDF files with low quality or complex graphics, recognition may be more difficult

### Jak używać [PL]

1. Otwórz aplikację w przeglądarce
2. Wybierz plik PDF lub obraz zawierający kody kreskowe/QR
3. Kliknij przycisk "Prześlij i analizuj"
4. Wyniki odczytu kodów zostaną wyświetlone w tabeli

> **Uwaga:** Pliki PDF są konwertowane na obrazy za pomocą biblioteki PDFtoImage dla lepszej skuteczności rozpoznawania kodów.

### Ograniczenia [PL]

- Jakość rozpoznawania zależy od wyraźności kodów oraz jakości obrazów
- W przypadku plików PDF o niskiej jakości lub ze złożonymi grafikami, rozpoznawanie może być utrudnione

### License [EN]

This project uses the following libraries according to their licenses:

- ZXing.Net - Apache License 2.0
- PDFtoImage - MIT License

### Licencja [PL]

Ten projekt wykorzystuje następujące biblioteki zgodnie z ich licencjami:

- ZXing.Net - Apache License 2.0
- PDFtoImage - licencja MIT

---

Project created for demonstration purposes. | Projekt stworzony w celach demonstracyjnych.