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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using IHM.Helpers;

namespace IHM.View.HomePage
{
    /// <summary>
    /// Logique d'interaction pour HomePageView.xaml
    /// </summary>
    public partial class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
            if (Singleton.GetInstance().GetDBB().DBClient != null)
            {
                long espace_utilise_long = Singleton.GetInstance().GetDBB().espace_utilise;

                double espace_utilise_Megabyte = ConvertBytesToMegabytes(espace_utilise_long);
                double espace_utilise_Gegabyte = ConvertMegabytesToGigabytes(espace_utilise_Megabyte);

                SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Espace Indisponible",
                    Values = new ChartValues<ObservableValue> { new ObservableValue((100- espace_utilise_Gegabyte)) },
                    DataLabels = true,
                    ToolTip="Espace Indisponible " + (100- espace_utilise_Gegabyte),
                    Fill = System.Windows.Media.Brushes.Blue
                },
                new PieSeries
                {
                    Title = "Espace disponible",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(espace_utilise_Gegabyte) },
                    DataLabels = true,
                   ToolTip="Espace disponible " + (100- espace_utilise_Gegabyte),
                   Fill = System.Windows.Media.Brushes.DarkCyan
                }
            };

                Chart.LegendLocation = LegendLocation.Bottom;
            }
            DataContext = this;
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        static double ConvertMegabytesToGigabytes(double megabytes) // SMALLER
        {
            // 1024 megabyte in a gigabyte
            return megabytes / 1024.0;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
