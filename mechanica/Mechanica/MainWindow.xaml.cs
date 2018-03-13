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



        //desired distance LA should travel too
        public int desired_distance;






        //Dump int for incoming load cell sensor data
        public int loadcell_data;






        private void begin_test_btn_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(input_distance_inp.Text, out desired_distance);


        }


                    //append_loadcell_box(loadcell_recieve.PinValue.ToString());

                


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
