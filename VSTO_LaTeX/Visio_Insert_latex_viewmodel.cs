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
            ImageTypes = new List<string> { "png", "svg" };
            SelectedImageTypeIndex = 0;
        }

        public override bool Generate_Click()
        {
            try
            {
                var tempFileName = Path.GetTempPath() + "VSTO_latex";

                File.WriteAllText(tempFileName + ".tex", LatexText);

                StatusText = "TEX => DVI";
                CreateDVI(tempFileName);

                if (SelectedImageTypeIndex == 0)
                {
                    StatusText = "DVI => PNG";
                    CreatePNG(tempFileName);
                    DropImage(vApp.ActivePage, tempFileName + ".png", false);
                }
                else if (SelectedImageTypeIndex == 1)
                {
                    StatusText = "DVI => SVG";
                    CreateSVG(tempFileName);
                    DropImage(vApp.ActivePage, tempFileName + ".svg", true);
                }

                return true;
            }
            catch (Exception e)
            {
                var msgBox = MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }

        private void CreateDVI(string tempFileName)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetTempPath(),
                FileName = "pdflatex.exe",
                Arguments = String.Format("-shell-escape -output-format dvi -interaction=batchmode \"{0}.tex\"", tempFileName)
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private void CreatePNG(string tempFileName)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetTempPath(),
                FileName = "dvipng.exe",
                Arguments = String.Format("-q -D {1} -T tight -bg Transparent -o \"{0}.png\" \"{0}.dvi\"", tempFileName, DPIString)
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private void CreateSVG(string tempFileName)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetTempPath(),
                FileName = "dvisvgm.exe",
                Arguments = String.Format("--no-fonts -o \"{0}.svg\" \"{0}.dvi\"", tempFileName)
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private void DropImage(Visio.Page vPag, string imageFile, bool isSvg = false)
        {
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
