using System;
using System.IO;

namespace Usb.Events.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();

            usbEventWatcher.UsbDeviceRemoved += (na, device) => Console.WriteLine("Removed:" + Environment.NewLine + device + Environment.NewLine);

            usbEventWatcher.UsbDeviceAdded += (na, device) =>
            {
                Console.WriteLine("Added:" + Environment.NewLine + device + Environment.NewLine);

                Device dev = Device.Get(device.DeviceSystemPath);

                if (dev == null)
                    return;

                Console.WriteLine("Device Desc: " + dev.GetStringProperty(Device.DEVPKEY_Device_DeviceDesc));
                Console.WriteLine("Bus Reported Device Desc: " + dev.GetStringProperty(Device.DEVPKEY_Device_BusReportedDeviceDesc));
                Console.WriteLine("Friendly Name: " + dev.GetStringProperty(Device.DEVPKEY_Device_FriendlyName));
                Console.WriteLine();

                Console.WriteLine("Parent: " + dev.ParentPnpDeviceId);

                Device parent = Device.Get(dev.ParentPnpDeviceId);

                if (parent == null)
                    return;

                Console.WriteLine("Device Desc: " + parent.GetStringProperty(Device.DEVPKEY_Device_DeviceDesc));
                Console.WriteLine("Bus Reported Device Desc: " + parent.GetStringProperty(Device.DEVPKEY_Device_BusReportedDeviceDesc));
                Console.WriteLine("Friendly Name: " + parent.GetStringProperty(Device.DEVPKEY_Device_FriendlyName));
                Console.WriteLine();

                foreach (string pnpDeviceId in dev.ChildrenPnpDeviceIds)
                {
                    Console.WriteLine("Child: " + pnpDeviceId);

                    Device child = Device.Get(pnpDeviceId);

                    if (child == null)
                        continue;

                    Console.WriteLine("Device Desc: " + child.GetStringProperty(Device.DEVPKEY_Device_DeviceDesc));
                    Console.WriteLine("Bus Reported Device Desc: " + child.GetStringProperty(Device.DEVPKEY_Device_BusReportedDeviceDesc));
                    Console.WriteLine("Friendly Name: " + child.GetStringProperty(Device.DEVPKEY_Device_FriendlyName));
                    Console.WriteLine();
                }
            };

            usbEventWatcher.UsbDriveEjected += (na, path) => Console.WriteLine("Ejected:" + Environment.NewLine + path + Environment.NewLine);

            usbEventWatcher.UsbDriveMounted += (na, path) =>
            {
                Console.WriteLine("Mounted:" + Environment.NewLine + path + Environment.NewLine);

                foreach (string entry in Directory.GetFileSystemEntries(path))
                    Console.WriteLine(entry);

                Console.WriteLine();
            };

            Console.ReadLine();
        }
    }
}
