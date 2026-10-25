using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using GameLibraryApp.Data;
using GameLibraryApp.Models;

namespace GameLibraryApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Game> _games = new();

        [ObservableProperty]
        private Game? _selectedGame;

        [ObservableProperty]
        private bool _isDetailOpen = false;

        [ObservableProperty]
        private bool _isGridOpen = true;

        // Dynamic Cover Card Dimensions
        [ObservableProperty]
        private double _coverWidth = 140;

        [ObservableProperty]
        private double _coverHeight = 200;

        partial void OnCoverWidthChanged(double value)
        {
            CoverHeight = value * 1.428; // Maintains 1:1.43 poster aspect ratio
        }

        // Add Modal State
        [ObservableProperty]
        private bool _isAddGameOpen = false;

        [ObservableProperty]
        private string _newGameTitle = string.Empty;

        [ObservableProperty]
        private string _newGameCoverPath = string.Empty;

        [ObservableProperty]
        private string _newGameExePath = string.Empty;

        // Edit Modal State
        [ObservableProperty]
        private bool _isEditGameOpen = false;

        [ObservableProperty]
        private string _editGameTitle = string.Empty;

        [ObservableProperty]
        private string _editGameCoverPath = string.Empty;

        [ObservableProperty]
        private string _editGameExePath = string.Empty;

        public MainViewModel()
        {
            InitializeDatabaseAndLoadGames();
        }

        [RelayCommand]
        public void SelectGame(Game game)
        {
            SelectedGame = game;
            IsDetailOpen = true;
            IsGridOpen = false;
        }

        [RelayCommand]
        public void CloseDetail()
        {
            IsDetailOpen = false;
            IsGridOpen = true;
        }

        [RelayCommand]
        public void LaunchGame()
        {
            if (SelectedGame == null || string.IsNullOrWhiteSpace(SelectedGame.ExecutablePath))
            {
                MessageBox.Show("No executable path or launch URL configured for this game.", "Cannot Launch", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = SelectedGame.ExecutablePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch game:\n{ex.Message}", "Execution Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public void OpenAddGame()
        {
            NewGameTitle = string.Empty;
            NewGameCoverPath = string.Empty;
            NewGameExePath = string.Empty;
            IsAddGameOpen = true;
        }

        [RelayCommand]
        public void CancelAddGame() => IsAddGameOpen = false;

        [RelayCommand]
        public void BrowseCover()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                NewGameCoverPath = dialog.FileName;
            }
        }

        [RelayCommand]
        public void BrowseExe()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Executable & Shortcuts (*.exe;*.url;*.lnk)|*.exe;*.url;*.lnk|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                NewGameExePath = dialog.FileName;
            }
        }

        [RelayCommand]
        public void SaveGame()
        {
            if (string.IsNullOrWhiteSpace(NewGameTitle)) return;

            var newGame = new Game
            {
                Title = NewGameTitle.Trim(),
                CoverImagePath = string.IsNullOrWhiteSpace(NewGameCoverPath)
                    ? "https://via.placeholder.com/300x400?text=No+Cover"
                    : NewGameCoverPath.Trim(),
                ExecutablePath = NewGameExePath.Trim()
            };

            using var db = new AppDbContext();
            db.Games.Add(newGame);
            db.SaveChanges();

            Games.Add(newGame);
            IsAddGameOpen = false;
        }

        [RelayCommand]
        public void OpenEditGame()
        {
            if (SelectedGame == null) return;

            EditGameTitle = SelectedGame.Title;
            EditGameCoverPath = SelectedGame.CoverImagePath;
            EditGameExePath = SelectedGame.ExecutablePath;
            IsEditGameOpen = true;
        }

        [RelayCommand]
        public void CancelEditGame() => IsEditGameOpen = false;

        [RelayCommand]
        public void BrowseEditCover()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                EditGameCoverPath = dialog.FileName;
            }
        }

        [RelayCommand]
        public void BrowseEditExe()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Executable & Shortcuts (*.exe;*.url;*.lnk)|*.exe;*.url;*.lnk|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                EditGameExePath = dialog.FileName;
            }
        }

        [RelayCommand]
        public void SaveEditGame()
        {
            if (SelectedGame == null || string.IsNullOrWhiteSpace(EditGameTitle)) return;

            using var db = new AppDbContext();
            var gameToUpdate = db.Games.FirstOrDefault(g => g.Id == SelectedGame.Id);
            if (gameToUpdate != null)
            {
                gameToUpdate.Title = EditGameTitle.Trim();
                gameToUpdate.CoverImagePath = string.IsNullOrWhiteSpace(EditGameCoverPath)
                    ? "https://via.placeholder.com/300x400?text=No+Cover"
                    : EditGameCoverPath.Trim();
                gameToUpdate.ExecutablePath = EditGameExePath.Trim();

                db.SaveChanges();

                var updatedGame = new Game
                {
                    Id = gameToUpdate.Id,
                    Title = gameToUpdate.Title,
                    CoverImagePath = gameToUpdate.CoverImagePath,
                    ExecutablePath = gameToUpdate.ExecutablePath
                };

                int index = Games.IndexOf(SelectedGame);
                if (index >= 0)
                {
                    Games[index] = updatedGame;
                }

                SelectedGame = updatedGame;
            }

            IsEditGameOpen = false;
        }

        [RelayCommand]
        public void DeleteGame()
        {
            if (SelectedGame == null) return;

            using var db = new AppDbContext();
            var gameToDelete = db.Games.FirstOrDefault(g => g.Id == SelectedGame.Id);
            if (gameToDelete != null)
            {
                db.Games.Remove(gameToDelete);
                db.SaveChanges();
            }

            Games.Remove(SelectedGame);
            SelectedGame = null;

            IsDetailOpen = false;
            IsGridOpen = true;
        }

        private void InitializeDatabaseAndLoadGames()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            Games = new ObservableCollection<Game>(db.Games.ToList());
        }
    }
}