using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SimpleParserForm
{
    public partial class ResultForm : Form
    {
        
        public ResultForm(IEnumerable<string> parsedText)
        {
            InitializeComponent();
            
            var enumerable = parsedText.ToList();
            ShowResult(enumerable);
        }

        private void ShowResult(IEnumerable<string> parsedText)
        {
            resultTextBox.Text = string.Join("\n", parsedText.ToArray());
        }
    }
}