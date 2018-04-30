using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using RJCP.IO.Ports;
using ArduinoDriver;
using ArduinoDriver.SerialProtocol;
using ArduinoUploader.Hardware;
using ArduinoUploader;
using System.Text.RegularExpressions;
namespace Mechanica
{
    public partial class MainWindow : Window
    {

        // List that contains lists of each data readout
        public List<List<double>> data = new List<List<double>>();

        public string PortName = "COM9";

        public struct CommandPacket
        {
            public string RunTest;
            public string DisplacementRate, Displacement;
            public string Retract;
        };

        CommandPacket command_message;

        public SerialPortStream MainPort = new SerialPortStream();


        public void Run_Uploader(string serialPortName, string FilePath)
        {
            // Upload HEX Uno file
            var uploader = new ArduinoSketchUploader(
                new ArduinoSketchUploaderOptions()
                {
                    FileName = FilePath,
                    PortName = serialPortName,
                    ArduinoModel = ArduinoModel.UnoR3
                }
                );
            try
            {
                uploader.UploadSketch();
            }
            catch
            {
                MessageBox.Show("Error! Please try erasing current program on Arduino",
                    "Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Find_Port_Name()
        {
            string _serialPortName = "COM9";
            //Find Serial Port arduino is connected too
            var allPortNames = SerialPortStream.GetPortNames();
            var distinctPorts = allPortNames.Distinct().ToList();


            // If we don't specify a COM port, automagically select one from list
            if (distinctPorts.SingleOrDefault() != null)
            {
                _serialPortName = distinctPorts.Single();

            }

            return _serialPortName;
        }

        public void Main_PortHandler(SerialPortStream port)
        {
            string serialPortName = Find_Port_Name();
            port.PortName = serialPortName;
            port.BaudRate = 115200;
            port.Open();
            append_connect_box("Connected!");


        }

        public void Begin_Test(CommandPacket commands, SerialPortStream port)
        {
            port.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");

            bool test_in_progress = true;
            while (test_in_progress)
            {
                if (port.BytesToRead > 0)
                {
                    string s = port.ReadLine();
                    //s = Regex.Replace(s, @"\r", string.Empty);
                    if (s == "done")
                    {
                        test_in_progress = false;
                        break;
                    }
                    string[] message = s.Split(',');
                    List<double> temp = new List<double>();
                    //temp.Add(Convert.ToDouble(s));
                    foreach (string element in message)
                    {

                        //Console.WriteLine(element);

                        double value = Convert.ToDouble(element);
                        temp.Add(value);
                        Console.WriteLine(element);

                    }
                    data.Add(temp);
                }
            }

            MessageBox.Show("Test Finished, Data ready to save","Mechanica",MessageBoxButton.OK, MessageBoxImage.Information);


        }

        public void Begin_Retract(CommandPacket commands, SerialPortStream port)
        {

        }

        public void Write_File(List<List<double>> data, string file_path)
        {
            using(TextWriter tw = new StreamWriter(file_path))
            {
                foreach (List<double> member in data)
                {
                    foreach (double guy in member)
                    {
                        tw.Write(guy);
                        tw.Write(",");
                    }
                    tw.WriteLine();
                }

            }
            MessageBox.Show("Wrote File", "Mechanica", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}
