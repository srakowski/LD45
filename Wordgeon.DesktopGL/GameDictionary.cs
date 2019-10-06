using System.IO;
using System.Linq;

namespace Wordgeon
{
    static class GameDictionary
    {
        private static string[] words;

        public static void Load()
        {
            words = File.ReadAllLines(@".\Content\words.txt")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.ToLower().Trim())
                .ToArray();
                
        }

        public static bool HasWord(string value) => words.Contains(value.ToLower().Trim());
    }
}
