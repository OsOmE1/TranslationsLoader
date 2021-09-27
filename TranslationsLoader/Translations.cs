using System.Collections.Generic;
using System.IO;

namespace TranslationsLoader
{
    public class Translations
    {
        public Dictionary<string, string> _translations;
        public Translations(string path)
        {
            _translations = new Dictionary<string, string>();

            string[] translationText = File.ReadAllLines(path);
            foreach (string t in translationText)
            {
                if (!t.Contains("⇨"))
                {
                    continue;
                }

                _translations.Add(t.Split("⇨")[0], t.Split("⇨")[1]);
            }
        }
    }
}
