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

        public bool test_ongoing = false;


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

        public void Begin_Test(SerialPortStream port, double DisplacementRate, double Displacement)
        {
            port.Open();

            // Prompt device to see if it's ready
            bool quick = true;
            while (quick)
            {
                if (port.BytesToRead > 0)
                {
                    string s = port.ReadLine();
                    s = Regex.Replace(s, @"\r", string.Empty);
                    // Add in a label to signify arduino is ready to recieve commands
                    if (s == "ready")
                    {
                        quick = false;
                        // Update labeel to show we ready
                        break;
                    }
                }
            }

            // Now Reset the system back to 0
            quick = true;
            port.Write("<1>");
            while (quick)
            {
                if (port.BytesToRead > 0)
                {
                    string s = port.ReadLine();
                    s = Regex.Replace(s, @"\r", string.Empty);
                    if (s == "begin")
                    {
                        quick = false;
                        // Signify the system is now reset
                        break;
                    }
                }
            }


            // Now send your commands and let's start the test
            quick = true;
            port.Write("<2>");
            while (quick)
            {
                if (port.BytesToRead > 0)
                {
                    string s = port.ReadLine();
                    s = Regex.Replace(s, @"\r", string.Empty);

                    if (s == "give")
                    {
                        port.WriteLine(DisplacementRate.ToString());
                        //quick = false;
                        //break;
                    }
                    if (s == "again")
                    {
                        port.WriteLine(Displacement.ToString());
                        quick = false;
                        break;
                    }
                }
            }
            test_ongoing = true;
            while (test_ongoing)
            {
                if (port.BytesToRead > 0)
                {
                    string s = port.ReadLine();
                    string[] message = s.Split(',');
                    List<double> temp = new List<double>();
                    foreach (string element in message)
                    {
                        double value = Convert.ToDouble(element);
                        temp.Add(value);
                    }
                    data.Add(temp);
                }
            }
         
        }

        public void Write_File(List<List<double>> data)
        {
            using(TextWriter tw = new StreamWriter("List.csv"))
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
        }
    }

}
