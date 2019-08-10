using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaTeX_UI
{
    public static class Latex_ToolChain
    {
        public static void CreateDVI(string tempFileName)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetTempPath(),
                FileName = "latex.exe",
                Arguments = String.Format("-shell-escape -output-format dvi -interaction=batchmode \"{0}.tex\"", tempFileName)
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        public static void CreatePNG(string tempFileName, int DPI)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetTempPath(),
                FileName = "dvipng.exe",
                Arguments = String.Format("-q -D {1} -T tight -bg Transparent -o \"{0}.png\" \"{0}.dvi\"", tempFileName, DPI.ToString())
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        public static void CreateSVG(string tempFileName)
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
    }
}
