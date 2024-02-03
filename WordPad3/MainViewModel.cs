using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.IO.Packaging;
using System.Windows.Markup;
using System.Windows.Data;
using System.Globalization;
using System.Resources;
using System.Threading;


namespace WordPad3
{
    public partial class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class TextEditorViewModel : ViewModelBase
    {
        private readonly TextEditorModel _model;
        private readonly ResourceManager _resourceManager;
        public TextEditorViewModel()
        {
            _model = new TextEditorModel();
            _resourceManager = new ResourceManager("Wordpad3.Resources.Strings", typeof(TextEditorViewModel).Assembly);

            SetLanguage("ru");
            _selectedFont = new FontFamily("Arial");
            NewCommand = new RelayCommand(NewFile);
            OpenCommand = new RelayCommand(OpenFile);
            SaveCommand = new RelayCommand(SaveFile);
            SwitchLanguageCommand = new RelayCommand(SwitchLanguage);

        }
        private void NewFile()
        {
            TextContent = string.Empty;
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    TextContent = _model.ReadFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    _model.SaveFile(saveFileDialog.FileName, TextContent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private FontFamily _selectedFont;

        public FontFamily SelectedFont
        {
            get { return _selectedFont; }
            set
            {
                if (_selectedFont != value)
                {
                    _selectedFont = value;
                    OnPropertyChanged(nameof(SelectedFont));
                }
            }
        }

        private string _textContent;

        public string TextContent
        {
            get { return _textContent; }
            set
            {
                if (_textContent != value)
                {
                    _textContent = value;
                    OnPropertyChanged(nameof(TextContent));
                }
            }
        }

        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SwitchLanguageCommand { get; }

        private string GetLocalizedString(string key)
        {
            return _resourceManager.GetString(key);
        }

        private void SwitchLanguage()
        {
            
            if (Thread.CurrentThread.CurrentUICulture.Name == "ru")
                SetLanguage("en");
            else
                SetLanguage("ru");
        }

        private void SetLanguage(string cultureCode)
        {
            CultureInfo newCulture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentUICulture = newCulture;

            OnPropertyChanged(nameof(TextContent));
            OnPropertyChanged(nameof(MenuButton));
            OnPropertyChanged(nameof(NewCommandText));
            OnPropertyChanged(nameof(OpenCommandText));
            OnPropertyChanged(nameof(SaveCommandText));
        }
        public string MenuButton => GetLocalizedString("Menu");
        public string NewCommandText => GetLocalizedString("New");
        public string OpenCommandText => GetLocalizedString("Open");
        public string SaveCommandText => GetLocalizedString("Save");
    }

  

}


