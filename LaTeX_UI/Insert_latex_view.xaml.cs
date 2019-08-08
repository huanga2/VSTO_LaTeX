using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaTeX_UI
{
    /// <summary>
    /// Interaction logic for Insert_latex_view.xaml
    /// </summary>
    public partial class Insert_latex_view : Window
    {
        private Insert_latex_viewmodel viewmodel;

        public Insert_latex_view(Insert_latex_viewmodel viewmodel)
        {
            InitializeComponent();
            this.viewmodel = viewmodel;
            this.DataContext = this.viewmodel;
        }

        private async void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (await viewmodel.Generate_ClickAsync())
            {
                this.Close();
            }
        }
    }
}
