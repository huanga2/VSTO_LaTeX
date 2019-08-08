using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LaTeX_UI
{
    [DataContract]
    public class LatexData
    {
        [DataMember]
        public string LatexText { get; set; } = @"\documentclass{article}
\usepackage{amsmath}
\pagestyle{empty}
\begin{document}




\end{document}";

        [DataMember]
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

        [DataMember]
        public int DPI = 1200;

        public string DPIString
        {
            get
            {
                return DPI.ToString();
            }

            set
            {
                DPI = int.TryParse(value, out DPI) ? DPI : 1200;

                DPI = (DPI > 0) ? DPI : 1200;
            }
        }
    }
}
