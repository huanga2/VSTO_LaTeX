using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Visio = Microsoft.Office.Interop.Visio;
using Office = Microsoft.Office.Core;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using LaTeX_UI;

namespace VSTO_LaTeX
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Globals.ThisAddIn.Application.MarkerEvent += RightClickLatexImageEvent;
        }

        private void RightClickLatexImageEvent(Visio.Application app, int SequenceNum, string ContextString)
        {
            Regex rx = new Regex(@"/shape=(Sheet.[0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var rxMatch = rx.Match(ContextString);

            if (!rxMatch.Success)
            {
                return;
            }

            Visio.Shape currentShape = Globals.ThisAddIn.Application.ActivePage.Shapes.ItemU[rxMatch.Groups[1].ToString()];

            Window win = new Insert_latex_view(new Visio_Insert_latex_viewmodel(currentShape));
            win.ShowDialog();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
