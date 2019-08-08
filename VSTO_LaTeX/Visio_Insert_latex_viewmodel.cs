using LaTeX_UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Visio = Microsoft.Office.Interop.Visio;

namespace VSTO_LaTeX
{
    public class Visio_Insert_latex_viewmodel : Insert_latex_viewmodel
    {
        protected readonly Visio.Application vApp = Globals.ThisAddIn.Application;

        private readonly Visio.Shape selectedShape;

        public Visio_Insert_latex_viewmodel() : base()
        {
        }

        public Visio_Insert_latex_viewmodel(Visio.Shape shape) : this()
        {
            selectedShape = shape;

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(selectedShape.Data1)))
            {
                var ser = new DataContractJsonSerializer(LatexData.GetType());
                LatexData = ser.ReadObject(ms) as LaTeX_UI.LatexData;
            }
        }

        public override void DropImage(string imageFile, bool isSvg)
        {
            Visio.Page vPag = vApp.ActivePage;
            double magicScale = isSvg ? 1 : 96 / (double) LatexData.DPI;

            if (vPag != null)
            {
                var shpNew = vPag.Import(imageFile);

                if (selectedShape != null)
                {
                    shpNew.CellsU["PinX"].FormulaU = selectedShape.CellsU["PinX"].FormulaU;
                    shpNew.CellsU["PinY"].FormulaU = selectedShape.CellsU["PinY"].FormulaU;
                }

                //Set size
                string oldWidth_s = shpNew.CellsU["Width"].FormulaU;
                double oldWidth_d = double.Parse(oldWidth_s.Replace(" mm", ""), CultureInfo.InvariantCulture);

                shpNew.CellsU["Width"].FormulaU = String.Format("{0:f} mm", oldWidth_d * magicScale * LatexData.fontSize / 10);

                string oldHeight_s = shpNew.CellsU["Height"].FormulaU;
                double oldHeight_d = double.Parse(oldHeight_s.Replace(" mm", ""), CultureInfo.InvariantCulture);
                shpNew.CellsU["Height"].FormulaU = String.Format("{0:f} mm", oldHeight_d * magicScale * LatexData.fontSize / 10);

                shpNew.AddNamedRow((short)Visio.VisSectionIndices.visSectionAction, "RightClick", 0);
                shpNew.CellsU["Actions.RightClick"].FormulaU = "\"Edit LaTeX image\"";
                shpNew.CellsU["Actions.RightClick.Action"].FormulaU = "RUNADDONWARGS(\"QueueMarkerEvent\", \"/app=VisioTex\")";

                using (var ms = new MemoryStream())
                {
                    var ser = new DataContractJsonSerializer(typeof(LatexData));
                    ser.WriteObject(ms, LatexData);
                    byte[] json = ms.ToArray();

                    shpNew.Data1 = Encoding.UTF8.GetString(json, 0, json.Length);
                }

                selectedShape?.Delete();
            }
        }
    }
}
