using GHIElectronics.UWP.Shields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EnviTelemetryDevice
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string IoTHubConnectionString = "<replace>";

        private FEZHAT hat;

        private DispatcherTimer timer;
        private int registeringCount;
        private bool hasReigstered = false;
        private bool enableTelemetry = false;

        private bool? useD2 = false;

        public MainPage()
        {
            this.InitializeComponent();
            this.Setup();
        }

        public void Setup()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(321);
            timer.Tick += Timer_Tick;
            this.timer = timer;

            this.CreateFezHAT();
            this.CreateIoTHubDeviceClient();

            timer.Start();
        }

        public async void CreateFezHAT()
        {
            this.hat = await FEZHAT.CreateAsync();
        }

        public void CreateIoTHubDeviceClient()
        {
            // TODO: Create an IoT Hub Device Client.
        }

        private void Timer_Tick(object sender, object e)
        {
            var hat = this.hat;
            var dio18 = hat.IsDIO18Pressed();
            var dio22 = hat.IsDIO22Pressed();

            PushButtonFeedback(dio18 || dio22);

            if (dio18 && dio22)
            {
                RegisterDeviceToIoTSolutionOnce();
                RegisteringBlink();
            }
            else if (dio18)
            {
                this.enableTelemetry = true;
            }
            else if (dio22)
            {
                this.enableTelemetry = false;
            }
            else
            {
                if (registeringCount > 0)
                {
                    --registeringCount;
                    RegisteringBlink();
                }
                else
                {
                    if (this.enableTelemetry)
                    {
                        SendTelemetryData();
                    }
                    NormalBlink();
                }
            }
        }

        private void SendTelemetryData()
        {
            // TODO: Send the telemetry data to IoT Hub.
        }

        public void RegisterDeviceToIoTSolutionOnce()
        {
            if (hasReigstered)
            {
                return;
            }
            hasReigstered = true;
            registeringCount = 8;
            // TODO: Register the device to the IoT solution.
        }

        private void PushButtonFeedback(bool isOn)
        {
            this.hat.DIO24On = isOn;
        }

        private void RegisteringBlink()
        {
            if (!useD2.HasValue)
            {
                useD2 = false;
            }
            if (useD2 ?? false)
            {
                useD2 = false;
                hat.D2.Color = FEZHAT.Color.Blue;
                hat.D3.TurnOff();
            }
            else
            {
                useD2 = null;
                hat.D2.TurnOff();
                hat.D3.Color = FEZHAT.Color.Blue;
            }

            useD2 = !useD2;
        }

        private void NormalBlink()
        {
            var hat = this.hat;

            if (useD2 ?? false)
            {
                useD2 = false;
                hat.D2.Color = FEZHAT.Color.Green;
                hat.D3.TurnOff();
            }
            else
            {
                useD2 = null;
                hat.D2.TurnOff();
                hat.D3.Color = FEZHAT.Color.Green;
            }

            if (!useD2.HasValue)
            {
                useD2 = true;
                hat.D2.TurnOff();
                hat.D3.TurnOff();
            }
        }
    }
}
