using LaTeX_UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTO_LaTeX
{
    public class Visio_Insert_latex_viewmodel : Insert_latex_viewmodel
    {
        public Visio_Insert_latex_viewmodel() : base()
        {
            ImageTypes = new List<string> { "png" };
            SelectedImageTypeIndex = 0;
        }

        public override bool Generate_Click()
        {
            return false;
        }
    }
}
