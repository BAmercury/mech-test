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
    
    public partial class MainWindow : Window
    {

        public void Arduino_Start(ArduinoDriver.ArduinoDriver driver)
        {
            using (driver)
            {
                set_pins(driver);

                while (true)
                {

                }
                
            }
        }

        private void set_pins(ArduinoDriver.ArduinoDriver driver)
        {
            //driver.Send(new PinModeRequest(EnablePin, PinMode))
        }




        



    }

}
