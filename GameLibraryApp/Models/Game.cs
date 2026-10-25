using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CoverImagePath { get; set; } = string.Empty;
        public string ExecutablePath { get; set; } = string.Empty; // Holds .exe path, steam:// URL, or shortcut
    }
}
