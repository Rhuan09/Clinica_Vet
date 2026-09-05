# Clinica_Vet

A veterinary clinic management desktop application built with **C#**, **WinUI 3 / XAML**, and **SQLite**.

---

## Overview

**Clinica_Vet** is an administrative and medical records desktop software designed for veterinary clinics and pet healthcare providers. It streamlines client records, animal patient tracking, clinical appointments, examinations, medical treatments, and clinic inventory management.

---

## Key Features

- **Patient Management (`Animal`, `Especie`)**: Complete medical history and profiles for domestic pets, species categorization, breed, and identification.
- **Client & Owner Profiles (`Cliente`)**: Detailed owner information, contact details, linked pet profiles, and billing histories.
- **Appointments & Consultations (`Consulta`, `Exame`, `Tratamento`)**: Scheduling and recording veterinary consultations, diagnostics, laboratory exams, and prescribed treatment regimens.
- **Inventory & Stock Tracking (`Produto`, `ProdutoHistorico`)**: Clinic medication and supplies inventory tracking with historical transaction auditing.
- **Staff Administration (`Veterinario`, `Usuario`)**: Veterinarian profiles, specialization details, and application user credentials.

---

## Architecture & Tech Stack

- **Framework**: WinUI 3 (Windows App SDK) / XAML
- **Language**: C# (.NET)
- **Design Pattern**: MVVM (Model-View-ViewModel)
- **Data Access & Storage**: Entity Framework Core with SQLite (`veterinario.db`)
- **Structure**:
  - `Models/`: Domain entities (`Animal`, `Cliente`, `Consulta`, `Produto`, etc.)
  - `ViewModels/`: Presentation logic and view-model bindings (`ClienteViewModel`, `ConsultaViewModel`, `EstoqueAtualViewModel`, etc.)
  - `Views/`: XAML user interfaces and modular user controls
  - `DbContexts/` & `DataAccess/`: Database contexts and data repository abstractions

---

## Getting Started

### Prerequisites

- Windows 10 (version 1809 or higher) or Windows 11
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **.NET Desktop Development** and **Windows App SDK C# Templates** workloads installed
- [.NET SDK](https://dotnet.microsoft.com/download)

### Installation & Execution

1. Clone the repository:
   ```bash
   git clone https://github.com/Rhuan09/Clinica_Vet.git
   cd Clinica_Vet
   ```
2. Open `Clinica_Vet.sln` in Visual Studio 2022.
3. Ensure the active solution configuration is set to `Debug` / `x64` (or `x86`).
4. Build and run the project (`F5`).

---

## License

This project is licensed under the terms of the [LICENSE.txt](LICENSE.txt) file.
