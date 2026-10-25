# 🎮 Game Library Manager

A clean, responsive desktop game library manager and launcher built with **C#**, **WPF** (.NET 10), and **SQLite** following the **MVVM** pattern.

---

## 🌟 Key Features

- **Full CRUD Management:** Add, edit, and delete games with local persistence via SQLite & EF Core.
- **Game Launcher:** Execute local `.exe` binaries, desktop shortcuts (`.lnk`/`.url`), or protocol URIs (e.g., `steam://rungameid/...`).
- **Responsive High-Contrast UI:** Dark/monochrome aesthetic featuring adaptive grid layouts, card drop shadows (`DropShadowEffect`), and proportioned detail view splits.
- **Dynamic Cover Scaling:** Real-time cover poster size slider maintaining proper aspect ratios (1:1.43).
- **Detail Inspector:** Comprehensive game view with quick-action icon controls and overlapping poster display.

---

## 🛠️ Tech Stack

- **Language & Framework:** C# / .NET 10
- **UI Framework:** Windows Presentation Foundation (WPF)
- **MVVM Framework:** `CommunityToolkit.Mvvm`
- **Database & ORM:** SQLite via Entity Framework Core
- **Process Execution:** `System.Diagnostics.Process`

---

## 🚀 Getting Started

### Prerequisites
- **Windows 10 / 11**
- **.NET 10 SDK** or **Visual Studio 2022 / 2026** (with the .NET desktop development workload installed)

### Installation & Running
1. Clone the repository:
   ```bash
   git clone [https://github.com/CrisAce12/GameLibraryApp.git](https://github.com/CrisAce12/GameLibraryApp.git)
