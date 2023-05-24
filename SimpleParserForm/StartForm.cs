using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using SimpleParserForm.parser;

namespace SimpleParserForm
{
    public partial class StartForm : Form
    {
        private string _url;
        private bool _parsing = false;
        
        public StartForm()
        {
            InitializeComponent();
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            if (_parsing) return;
            if (string.IsNullOrEmpty(_url))
            {
                MessageBox.Show("URL не должен быть пустым", "WARNING", MessageBoxButtons.OK);
                return;
            }
            _parsing = true;
            var parser = new Parser(_url);
            var result = new List<string>();

            var oldCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            
            var thread = new Thread(() => result.AddRange(parser.ParseText()));
            thread.Start();
            thread.Join();

            Cursor.Current = oldCursor;
            _parsing = false;
            var resultForm = new ResultForm(result);
            resultForm.Show();
        }

        private void urlTextBox_TextChanged_1(object sender, EventArgs e)
        {
            _url = urlTextBox.Text;
        }
    }
}