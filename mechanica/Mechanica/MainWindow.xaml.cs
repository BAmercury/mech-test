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

        public int displacement_data;






        private void begin_test_btn_Click(object sender, RoutedEventArgs e)
        {


            command_message.Displacement = input_displacement_inp.Text;
            command_message.DisplacementRate = input_displacment_rate_inp.Text;
            command_message.RunTest = "1";
            command_message.Retract = "0";
            Begin_Test(command_message, MainPort);


            




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

        public void append_distance_box(string value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(append_distance_box), new object[] {  value });
                return;

            }
            displacement_data_rd.Text = value;
        }

        public void append_connect_box(string value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(append_connect_box), new object[] { value });
                return;
            }
            connect_lbl.Text = value;
        }

        private void connect_btn_Click(object sender, RoutedEventArgs e)
        {
            Main_PortHandler(MainPort);
        }

        private void retract_test_btn_Click(object sender, RoutedEventArgs e)
        {
            command_message.Displacement = "0";
            command_message.DisplacementRate = "0";
            command_message.RunTest = "0";
            command_message.Retract = "1";
            Begin_Retract(command_message, MainPort);

        }

        private void write_file_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.ShowDialog();
            Write_File(data, saveFileDialog.FileName);
            
        }
    }
}
