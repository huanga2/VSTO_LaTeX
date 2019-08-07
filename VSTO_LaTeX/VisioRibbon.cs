using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LaTeX_UI;
using Microsoft.Office.Tools.Ribbon;

namespace VSTO_LaTeX
{
    public partial class VisioRibbon
    {
        private void VisioRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            Window win = new Insert_latex_view(new Visio_Insert_latex_viewmodel());
            win.ShowDialog();
        }
    }
}
