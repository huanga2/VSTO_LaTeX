using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace LaTeX_UI
{
    public class Insert_latex_viewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string LatexText { get; set; } = @"\documentclass{article}
\usepackage{amsmath}
\pagestyle{empty}
\begin{document}




\end{document}";

        public double fontSize = 20;
        public string FontSizeString
        {
            get
            {
                return fontSize.ToString();
            }

            set
            {
                fontSize = double.TryParse(value, out fontSize) ? fontSize : 20;

                fontSize = (fontSize > 0) ? fontSize : 20;
            }
        }

        public double DPI = 1200;
        public string DPIString
        {
            get
            {
                return DPI.ToString();
            }

            set
            {
                DPI = double.TryParse(value, out DPI) ? DPI : 1200;

                DPI = (DPI > 0) ? DPI : 1200;
            }
        }

        public List<string> ImageTypes { get; set; } = new List<string>() { "png", "svg" };

        public int SelectedImageTypeIndex { get; set; }

        public virtual bool Generate_Click()
        {
            return false;
        }

        public Insert_latex_viewmodel() { }
    }
}
