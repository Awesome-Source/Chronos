using System;
using System.Windows.Forms;

namespace Chronos.WebView
{
    public partial class MainForm : Form
    {
        public MainForm(string uriString)
        {
            InitializeComponent();
            webView.Source = new Uri(uriString);
        }
    }
}
