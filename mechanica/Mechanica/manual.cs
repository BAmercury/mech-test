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
            port.Write("<" + commands.RunTest + "," + commands.DisplacementRate + "," + commands.Displacement + "," + commands.Retract + ">");
            while (enable_mc)
            {
                if (up_control == 1)
                {
                  port.Write("<" + "3" + "," + "2" + "," + "1" + "," + "0" + ">");


                }
                else if (down_control == 1)
                {
                  port.Write("<" + "3" + "," + "2" + "," + "2" + "," + "0" + ">");


                }
                else if (up_control == 0 && down_control == 0)
                {
                  port.Write("<" + "3" + "," + "0" + "," + "0" + "," + "0" + ">");


                }
            }

            tab_controller.SelectedValue = input_tab;
        }




        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyConverter k = new KeyConverter();

            if (enable_mc == true)
            {
                if (Keyboard.IsKeyDown(Key.Z))
                {
                    up_control = 1;


                }
                if (Keyboard.IsKeyDown(Key.X))
                {
                    down_control = 1;
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            KeyConverter t = new KeyConverter();

            if (enable_mc == true)
            {
                if (Keyboard.IsKeyUp(Key.Z))
                {
                    up_control = 0;
                }

                if (Keyboard.IsKeyUp(Key.X))
                {
                    down_control = 0;
                }
            }
        }
    }
}
