# GEMINI.md

## Project Overview

This is an ASP.NET Core MVC web application designed to convert various image formats (PNG, JPG, JPEG, SVG) into the `.ICO` format. The application allows users to upload an image, select desired output resolutions (16x16, 32x32, 64x64, 128x128), and download the generated `.ICO` file.

The project is built on .NET 8 and utilizes the `Magick.NET` library for robust image processing. It features a responsive user interface built with Bootstrap and includes a background service for automatic cleanup of temporary files.

## Building and Running

To build and run this project locally, follow these steps:

1.  **Clone the repository.**
2.  **Navigate to the project directory:**
    ```bash
    cd "Converte ICO"
    ```
3.  **Run the application:**
    ```bash
    dotnet run
    ```
4.  **Access the application** in your web browser at the URL provided in the console output (typically `http://localhost:5179`).

## Development Conventions

*   **Technology Stack:**
    *   **Backend:** ASP.NET Core MVC (.NET 8)
    *   **Image Processing:** Magick.NET
    *   **Frontend:** Bootstrap 5, FontAwesome 6
    *   **Database:** SQLite (for Identity)
*   **Architecture:**
    *   The application follows the Model-View-Controller (MVC) pattern.
    *   A dedicated `ImageConverterService` encapsulates all image processing logic.
    *   A `TempFileCleanupService` runs as a background task to manage temporary files.
    *   Session state is managed using `TempData` to pass information between requests during the conversion process.
*   **Code Style:**
    *   The code adheres to standard C# and ASP.NET Core conventions.
    *   Dependency injection is used to provide services like `IImageConverterService` to the controllers.
*   **Testing:**
    *   No explicit test project was found. To ensure quality, it is recommended to add a unit testing project to cover the `ImageConverterService` logic and controller actions.
