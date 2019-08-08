using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace LaTeX_UI
{
    public class Insert_latex_viewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsManager SettingsManager { get; set; } = new SettingsManager();

        public LatexData LatexData { get; set; } = new LatexData();

        public bool EnableDPI { get => SelectedImageTypeIndex != 1; }

        public List<string> ImageTypes { get; set; } = new List<string>() { "png", "svg" };

        public int SelectedImageTypeIndex
        {
            get { return SettingsManager.Settings.DefaultImageType; }
            set { SettingsManager.Settings.DefaultImageType = value; }
        }

        public string StatusText { get; set; }

        public virtual async Task<bool> Generate_ClickAsync()
        {
            try
            {
                var tempFileName = Path.GetTempPath() + "VSTO_latex";

                File.WriteAllText(tempFileName + ".tex", LatexData.LatexText);

                StatusText = "TEX ⇒ DVI";
                await Task.Run(() => Latex_ToolChain.CreateDVI(tempFileName));

                if (SelectedImageTypeIndex == 0)
                {
                    StatusText = "DVI ⇒ PNG";
                    Latex_ToolChain.CreatePNG(tempFileName, LatexData.DPI);
                    DropImage(tempFileName + ".png", false);
                }
                else if (SelectedImageTypeIndex == 1)
                {
                    StatusText = "DVI ⇒ SVG";
                    await Task.Run(() => Latex_ToolChain.CreateSVG(tempFileName));

                    DropImage(tempFileName + ".svg", true);
                }

                return true;
            }
            catch (Exception e)
            {
                var msgBox = MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }

        public virtual void DropImage(string fileName, bool isSvg)
        {
            throw new NotImplementedException();
        }

        public Insert_latex_viewmodel() { }
    }
}
