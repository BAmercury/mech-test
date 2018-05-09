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
using ArduinoDriver;
using ArduinoDriver.SerialProtocol;
using ArduinoUploader.Hardware;
using ArduinoUploader;
using System.Threading;
using Microsoft.Win32;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Mechanica
{
    public partial class MainWindow : Window
    {


        public ChartValues<MeasureModel> ChartValues { get; set; }
        public ChartValues<MeasureModel> DisplacementValues { get; set; }

        public double t1 = 0;
        public double t2 = 0;

        public void append_live_chart(double load, double displacement, double distance, double time)
        {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<double,double,double,double>(append_live_chart), new object[] { load, displacement,distance,time });
                Console.WriteLine("Busy");
                return;
            }

            ChartValues.Add(new MeasureModel
            {
                load_plot = load,
                displacement_plot = displacement
            });

            DisplacementValues.Add(new MeasureModel
            {
                load_plot = distance,
                displacement_plot = time
            });


        }

        private void test_btn_Click(object sender, RoutedEventArgs e)
        {

            ChartValues.Add(new MeasureModel
            {
                load_plot = t1,
                displacement_plot = t2
            });

            t1 = t1 + 10;
            t2 = t2 + 20;

           


        }

    }
}
