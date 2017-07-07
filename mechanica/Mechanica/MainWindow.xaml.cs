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

namespace Mechanica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public const ArduinoModel Arduino = ArduinoModel.UnoR3;

        public string desired_distance;

        private const int EnablePin = 8;
        private const int PWMPin = 11;
        private const int PWMPin2 = 3;

        private const int loadcell_pin = 0;

        public string loadcell_data;

        private const int lvdt_pin = 1;
        public string lvdt_data;



        private void begin_test_btn_Click(object sender, RoutedEventArgs e)
        {


            Thread oThread = new Thread(new ThreadStart(data_extract));
            oThread.Start();


        }

        private void data_extract()
        {

            using (var driver = new ArduinoDriver.ArduinoDriver(Arduino, false))
            {
                driver.Send(new PinModeRequest(EnablePin, PinMode.Output));
                driver.Send(new PinModeRequest(PWMPin, PinMode.Output));
                driver.Send(new PinModeRequest(PWMPin2, PinMode.Output));

                while (true)
                {

                    //driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));

                    //driver.Send(new AnalogWriteRequest(PWMPin2, 0));
                    //driver.Send(new AnalogWriteRequest(PWMPin, 255));
                    var loadcell_recieve = driver.Send(new AnalogReadRequest(0));
                    append_loadcell_box(loadcell_recieve.PinValue.ToString());


                    var lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                    append_lvdt_box(lvdt_recieve.PinValue.ToString());

                    Thread.Sleep(1000);

                }
            }

        }


        public void append_loadcell_box(string value)
        {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(append_loadcell_box), new object[] { value });
                return;
            }
            loadcell_data_rd.Text = value;
        }

        public void append_lvdt_box(string value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(append_lvdt_box), new object[] {  value });
                return;

            }
            lvdt_data_rd.Text = value;
        }

      
    }
}
