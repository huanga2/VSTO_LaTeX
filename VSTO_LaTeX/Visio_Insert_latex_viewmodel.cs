using LaTeX_UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Visio = Microsoft.Office.Interop.Visio;

namespace VSTO_LaTeX
{
    public class Visio_Insert_latex_viewmodel : Insert_latex_viewmodel
    {
        private readonly Visio.Application vApp = Globals.ThisAddIn.Application;

        public Visio_Insert_latex_viewmodel() : base()
        {
        }

        public override void DropImage(string imageFile, bool isSvg)
        {
            Visio.Page vPag = vApp.ActivePage;
            double magicScale = isSvg ? 1 : 96 / DPI;

            if (vPag != null)
            {
                var shpNew = vPag.Import(imageFile);
                //Set position
                shpNew.CellsU["PinX"].FormulaU = "75mm";
                shpNew.CellsU["PinY"].FormulaU = "175mm";
                //Set size
                var test = shpNew.CellsU["Width"].FormulaU;
                var ehh = double.Parse(test.Replace(" mm", ""), CultureInfo.InvariantCulture);

                shpNew.CellsU["Width"].FormulaU = String.Format("{0:f} mm", ehh * magicScale * fontSize / 10);

                test = shpNew.CellsU["Height"].FormulaU;
                ehh = double.Parse(test.Replace(" mm", ""), CultureInfo.InvariantCulture);
                shpNew.CellsU["Height"].FormulaU = String.Format("{0:f} mm", ehh * magicScale * fontSize / 10);

                shpNew.Data1 = LatexText;
                shpNew.Data2 = fontSize.ToString();
            }
        }
    }
}
