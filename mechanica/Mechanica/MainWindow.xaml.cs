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


        //desired distance LA should travel too
        public int desired_distance;

        public int la_rest = 7;


        //Arduino Pin Setup for LA
        private const int EnablePin = 8;
        private const int PWMPin = 11;
        private const int PWMPin2 = 3;


        //Analog In Pin for Loadcell sensor
        private const int loadcell_pin = 0;


        //Dump int for incoming load cell sensor data
        public int loadcell_data;


        //Analog in Pin for LVDT sensor
        private const int lvdt_pin = 1;

        //Dump int for incoming lvdt sensor data
        public int lvdt_data;


        //control system bools
        public bool move_bool = false;
        public bool retract_bool = false;




        private void begin_test_btn_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(input_distance_inp.Text, out desired_distance);

            move_bool = true;

            Thread oThread = new Thread(new ThreadStart(data_extract));
            oThread.Start();


        }
        /// <summary>
        /// Move to distance control schema and collects data
        /// </summary>
        private void data_extract()
        {

            using (var driver = new ArduinoDriver.ArduinoDriver(Arduino, true))
            {
                driver.Send(new PinModeRequest(EnablePin, PinMode.Output));
                driver.Send(new PinModeRequest(PWMPin, PinMode.Output));
                driver.Send(new PinModeRequest(PWMPin2, PinMode.Output));


                //While Loop for move to distance control system
                while (true)
                {

                    //driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));

                    //driver.Send(new AnalogWriteRequest(PWMPin2, 0));
                    //driver.Send(new AnalogWriteRequest(PWMPin, 255));
                    var loadcell_recieve = driver.Send(new AnalogReadRequest(0));
                    append_loadcell_box(loadcell_recieve.PinValue.ToString());

                    //comment
                    

                    //Move to distance loop
                    if (move_bool == true)
                    {

                        retract_bool = false;

                        var lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                        append_lvdt_box(lvdt_recieve.PinValue.ToString());


                        if (desired_distance > lvdt_recieve.PinValue)
                        {
                            driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));
                            driver.Send(new AnalogWriteRequest(PWMPin2, 0));
                            driver.Send(new AnalogWriteRequest(PWMPin, 255));
                            move_bool = true;
                            lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                        }
                        else if (desired_distance <= lvdt_recieve.PinValue)
                        {
                            move_bool = false;
                        }
                    }
                    else
                    {
                        retract_bool = true;
                    }


                    //Retract distance loop
                    if (retract_bool == true)
                    {
                        move_bool = false;
                        var lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                        append_lvdt_box(lvdt_recieve.PinValue.ToString());
                        if (la_rest <= lvdt_recieve.PinValue)
                        {

                            driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));
                            driver.Send(new AnalogWriteRequest(PWMPin2, 255));
                            driver.Send(new AnalogWriteRequest(PWMPin, 0));

                             lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                            append_lvdt_box(lvdt_recieve.PinValue.ToString());


                        }
                        else if (la_rest >= lvdt_recieve.PinValue)
                        {
                            //Thread.CurrentThread.Abort();
                            lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                            append_lvdt_box(lvdt_recieve.PinValue.ToString());
                        }
                        driver.Dispose();
                        Thread.CurrentThread.Abort();
                    }

                    //Thread.Sleep(1000);

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

        private void manual_ctrl_btn_Click(object sender, RoutedEventArgs e)
        {
            input_distance_inp.Visibility = Visibility.Hidden;
            input_distance_lbl.Visibility = Visibility.Hidden;
            begin_test_btn.Visibility = Visibility.Hidden;
            //lvdt_data_rd.Visibility = Visibility.Hidden;
            //lvdt_data_lbl.Visibility = Visibility.Hidden;
            //loadcell_data_rd.Visibility = Visibility.Hidden;
            //loadcell_data_lbl.Visibility = Visibility.Hidden;


            //up_control_btn.Visibility = Visibility.Visible;
            //down_control_btn.Visibility = Visibility.Visible;

            enable_mc = true;

            Thread oThread = new Thread(new ThreadStart(manual_control));
            oThread.Start();





        }


    }
}
