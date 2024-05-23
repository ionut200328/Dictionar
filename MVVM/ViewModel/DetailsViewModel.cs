using Dictionar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionar.MVVM.ViewModel
{
    public class DetailsViewModel: Core.ViewModel
    {
        private readonly IWordDataService _wordDataService;
        private string _selectedWord;
        private string _selectedWordDefinition;
        private string _selectedImage;
        private string _selectdCatgory;

        public DetailsViewModel( IWordDataService wordDataService)
        {
            _wordDataService = wordDataService;
        }

        private void LoadWord()
        {
            SelectedDescription = _wordDataService.GetDescription(SelectedWord);
            SelectedImage = _wordDataService.GetImage(SelectedWord);
            if(SelectedImage == null)
            {
                SelectedImage = "E:\\AN II Sem 2\\MVP\\Dictionar\\Data\\Images\\NOIMG.jpg";
            }
            SelectdCatgory = _wordDataService.GetCategory(SelectedWord);
        }

        public string SelectdCatgory
        {
            get => _selectdCatgory;
            set
            {
                _selectdCatgory = value;
                OnPropertyChanged(nameof(SelectdCatgory));
            }
        }
        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        public string SelectedWord
        {
            get => _selectedWord;
            set
            {
                _selectedWord = value;
                OnPropertyChanged(nameof(SelectedWord));
            }
        }
        public string SelectedDescription
        {
            get => _selectedWordDefinition;
            set
            {
                _selectedWordDefinition = value;
                OnPropertyChanged(nameof(SelectedDescription));
            }
        }

        public override void SetParameter(object parameter)
        {
            if (parameter is string word)
            {
                SelectedWord = word;
                LoadWord();

            }
        }
    }
}
