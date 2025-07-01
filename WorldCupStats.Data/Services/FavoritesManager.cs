using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WorldCupStats.Data.Services
{
    public static class FavoritesManager
    {
        private static readonly string FavoritesPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "WorldCupStats",
            "favorites.txt");

        // Estructura para contener los datos cargados
        public class FavoritesData
        {
            public string FavoriteTeamCode { get; set; } = "";
            public List<string> FavoritePlayerIds { get; set; } = new List<string>();
        }

        public static bool TryLoadFavorites(out FavoritesData data)
        {
            data = new FavoritesData();

            if (!File.Exists(FavoritesPath))
                return false;

            try
            {
                string content = File.ReadAllText(FavoritesPath).Trim();
                if (string.IsNullOrEmpty(content))
                    return false;

                var parts = content.Split('|');
                if (parts.Length != 2)
                    return false;

                data.FavoriteTeamCode = parts[0];
                data.FavoritePlayerIds = parts[1]
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SaveFavorites(FavoritesData data)
        {
            try
            {
                string directory = Path.GetDirectoryName(FavoritesPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string line = $"{data.FavoriteTeamCode}|{string.Join(",", data.FavoritePlayerIds)}";
                File.WriteAllText(FavoritesPath, line);
            }
            catch (Exception ex)
            {
                // Puedes agregar manejo de errores o logs aquí
                Console.WriteLine($"Error saving favorites: {ex.Message}");
            }
        }
    }
}
