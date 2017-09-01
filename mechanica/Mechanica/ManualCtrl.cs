using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using ArduinoDriver;
using ArduinoDriver.SerialProtocol;
using ArduinoUploader.Hardware;
using ArduinoUploader;

namespace Mechanica
{
    //Manual Control Class
    public partial class MainWindow : Window
    {


        //manual control variables
        public bool enable_mc = false;
        public int up_control = 0;
        public int down_control = 0;


        public void manual_control()
        {
            using (var driver = new ArduinoDriver.ArduinoDriver(Arduino, true))
            {
                driver.Send(new PinModeRequest(EnablePin, PinMode.Output));
                driver.Send(new PinModeRequest(PWMPin, PinMode.Output));
                driver.Send(new PinModeRequest(PWMPin2, PinMode.Output));


                //While Loop for move to distance control system
                while (true)
                {
                    var loadcell_recieve = driver.Send(new AnalogReadRequest(0));
                    append_loadcell_box(loadcell_recieve.PinValue.ToString());


                    var lvdt_recieve = driver.Send(new AnalogReadRequest(1));
                    append_lvdt_box(lvdt_recieve.PinValue.ToString());

                    if (up_control == 1)
                    {
                        driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));
                        driver.Send(new AnalogWriteRequest(PWMPin2, 0));
                        driver.Send(new AnalogWriteRequest(PWMPin, 255));
                    }
                    else if (down_control == 1)
                    {
                        driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));
                        driver.Send(new AnalogWriteRequest(PWMPin2, 255));
                        driver.Send(new AnalogWriteRequest(PWMPin, 0));
                    }
                    else if (up_control == 0 && down_control == 0)
                    {
                        driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.Low));
                        driver.Send(new AnalogWriteRequest(PWMPin2, 255));
                        driver.Send(new AnalogWriteRequest(PWMPin, 0));
                    }
                    

                    
                    //driver.Send(new DigitalWriteRequest(EnablePin, DigitalValue.High));
                    //driver.Send(new AnalogWriteRequest(PWMPin2, 0));
                    //driver.Send(new AnalogWriteRequest(PWMPin, 255));



                }
            }



        }




        private void up_control_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void down_control_btn_Click(object sender, RoutedEventArgs e)
        {

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
