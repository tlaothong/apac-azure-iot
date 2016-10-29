using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviTelemetryDevice.Factories
{
    public static class DeviceInfoFactory
    {
        public const string OBJECT_TYPE_DEVICE_INFO = "DeviceInfo";
        public const string VERSION_1_0 = "1.0";
        public const string HOST_NAME = "<host>";

        public static DeviceModel CreateDeviceModel(string deviceName) {
            var device = new DeviceModel()
            {
                ObjectType = OBJECT_TYPE_DEVICE_INFO,
                Version = VERSION_1_0,
                IsSimulatedDevice = false,
                ObjectName = deviceName,
            };
            device.DeviceProperties = new DeviceProperties
            {
                DeviceID = deviceName,
                HostName = HOST_NAME,
                HubEnabledState = true,
                CreatedTime = DateTime.UtcNow,
                Latitude = 16.483000,
                Longitude = 102.819400,
                InstalledRAM = "1 GB",
                Processor = "ARM",
                DeviceState = "normal",
                Manufacturer = "APAC IoT",
            };
            var telemetries = device.Telemetry;
            telemetries.Add(new Telemetry("Temperature", "Temperature", "double"));
            telemetries.Add(new Telemetry("Light", "Light", "double"));

            var commands = device.Commands;
            commands.Add(new Command("PingDevice"));

            return device;
        }
    }
}
