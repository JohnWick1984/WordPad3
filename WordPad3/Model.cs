using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WordPad3
{
    public class TextEditorModel 
    {

        
        public string ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SaveFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
            }
            catch
            {
                
            }
        }
    }
}
