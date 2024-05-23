using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dictionar.Services
{
    public class WordDetail
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public interface IWordDataService
    {
        Dictionary<string, WordDetail> GetWords();
        WordDetail GetWordDetail(string word);
        string GetCategory(string word);
        string GetDescription(string word);
        string GetImage(string word);
        List<string> GetCategories();
        void SaveChanges();
        void UpdateWord(string selectedWord, string selectedDescription, string selectedImage, string selectedCatgory);
        void AddWord(string word, string description, string image, string category);
        void DeleteWord(string selectedWord);
        bool WordExists(string selectedWord);
    }

    public class WordDataService : IWordDataService
    {
        private Dictionary<string, WordDetail> _words;
        private string _wordsFilePath = "E:\\AN II Sem 2\\MVP\\Dictionar\\Data\\Words.json";

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_words, Formatting.Indented);
            File.WriteAllText(_wordsFilePath, json);
        }

        public WordDataService()
        {
            // Assuming this is the path to your JSON file; adjust as necessary
            var wordsFilePath = "E:\\AN II Sem 2\\MVP\\Dictionar\\Data\\Words.json";
            LoadWordsFromJson(wordsFilePath);
        }

        private void LoadWordsFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            _words = JsonConvert.DeserializeObject<Dictionary<string, WordDetail>>(json);
        }

        public Dictionary<string, WordDetail> GetWords()
        {
            return _words;
        }

        public WordDetail GetWordDetail(string word)
        {
            if (!_words.ContainsKey(word))
            {
                return null;
            }
            return _words[word];
        }

        //getter for category, description and image
        public string GetCategory(string word)
        {
            if(!_words.ContainsKey(word))
            {
                return string.Empty;
            }
            return _words[word].Category;
        }
        public string GetDescription(string word)
        {
            if (!_words.ContainsKey(word))
            {
                return string.Empty;
            }
            return _words[word].Description;
        }
        public string GetImage(string word)
        {
            if (!_words.ContainsKey(word))
            {
                return null;
            }
            var imagePath = _words[word].Image;
            // Assuming "NOIMG.jpg" is your default image. Adjust as needed.
            if (string.IsNullOrEmpty(imagePath) || imagePath.EndsWith("NOIMG.jpg"))
            {
                return null;
            }
            return imagePath;
        }

        public List<string> GetCategories()
        {
            return _words.Values
                         .Select(w => w.Category)
                         .Distinct()
                         .ToList();
        }

        public void UpdateWord(string selectedWord, string selectedDescription, string selectedImage, string selectedCatgory)
        {
            _words[selectedWord].Description = selectedDescription;
            _words[selectedWord].Image = selectedImage;
            _words[selectedWord].Category = selectedCatgory;
        }

        public void DeleteWord(string selectedWord)
        {
            _words.Remove(selectedWord);
        }

        public void AddWord(string word, string description, string image, string category)
        {
            if(image == string.Empty || image == null || image == "E:\\AN II Sem 2\\MVP\\Dictionar\\Data\\Images\\NOIMG.jpg")
            {
                image = null;
            }
            _words.Add(word, new WordDetail
            {
                Description = description,
                Image = image,
                Category = category
            });
        }

        public bool WordExists(string selectedWord)
        {
            return _words.ContainsKey(selectedWord);
        }

    }
}
