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
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;

namespace Mechanica
{
    public partial class MainWindow : Window
    {

        //manual control variables
        public bool enable_mc = false;
        public int up_control = 0;
        public int down_control = 0;
        


        public void Begin_Manual(CommandPacket commands, SerialPortStream port)
        {
            port.Write("<" + commands.RunTest + "," + "0" + "," + "0" + "," + commands.Retract + ">");

            while (enable_mc)
            {

                if (up_control == 1)
                {
                    commands.RunTest = "3";
                    commands.Displacement = "1";
                    commands.DisplacementRate = "2";
                    commands.Retract = "0";
                    try
                    {
                        port.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");
                    }
                    catch
                    {

                    }


                }
                else if (down_control == 1)
                {
                    commands.RunTest = "3";
                    commands.Displacement = "2";
                    commands.DisplacementRate = "2";
                    commands.Retract = "0";
                    try
                    {
                        port.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");
                    }
                    catch
                    {

                    }


                }
                else if (up_control == 0 && down_control == 0)
                {
                    commands.RunTest = "3";
                    commands.Displacement = "0";
                    commands.DisplacementRate = "0";
                    commands.Retract = "0";
                    port.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");







                }
            }


        }




        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyConverter k = new KeyConverter();

            if (enable_mc == true)
            {
                if (MainPort.BytesToRead > 0)
                {
                    string s = MainPort.ReadLine();
                    s = Regex.Replace(s, @"\r", string.Empty);
                    try
                    {
                        double load = Convert.ToDouble(s);
                        load = ((load - 0) * (105.369 - 40.92)) / ((1023 - 0) + 40.92);
                        append_load_finecontrol_box(load.ToString());

                    }
                    catch
                    {

                    }
                }
                if (Keyboard.IsKeyDown(Key.Z))
                {
                    Console.WriteLine("Z");
                    up_control = 1;
                    CommandPacket commands;
                    commands.RunTest = "3";
                    commands.Displacement = "1";
                    commands.DisplacementRate = "2";
                    commands.Retract = "0";
                    MainPort.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");



                }
                if (Keyboard.IsKeyDown(Key.X))
                {
                    Console.WriteLine("X");
                    CommandPacket commands;
                    commands.RunTest = "3";
                    commands.Displacement = "3";
                    commands.DisplacementRate = "2";
                    commands.Retract = "0";
                    MainPort.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");
                    down_control = 1;
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            KeyConverter t = new KeyConverter();

            if (enable_mc == true)
            {
                if (MainPort.BytesToRead >0)
                {
                    string s = MainPort.ReadLine();
                    s = Regex.Replace(s, @"\r", string.Empty);
                    try
                    {
                        double load = Convert.ToDouble(s);
                        load = ((load - 0) * (105.369 - 40.92)) / ((1023 - 0) + 40.92);
                        append_load_finecontrol_box(load.ToString());

                    }
                    catch
                    {

                    }
                }
                if (Keyboard.IsKeyUp(Key.Z))
                {
                    Console.WriteLine("Stop");
                    up_control = 0;
                    CommandPacket commands;
                    commands.RunTest = "3";
                    commands.Displacement = "0";
                    commands.DisplacementRate = "0";
                    commands.Retract = "0";
                    MainPort.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");
                }

                if (Keyboard.IsKeyUp(Key.X))
                {
                    Console.WriteLine("Stopped");
                    CommandPacket commands;
                    commands.RunTest = "3";
                    commands.Displacement = "0";
                    commands.DisplacementRate = "0";
                    commands.Retract = "0";
                    MainPort.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");
                    down_control = 0;
                }
            }
        }
    }
}
