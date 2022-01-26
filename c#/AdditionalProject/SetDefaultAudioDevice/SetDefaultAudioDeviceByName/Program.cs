using CoreAudioApi;
using NAudio.CoreAudioApi;
using System;

namespace SetDefaultAudioDeviceByName
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DEBUG
            //args = new string[1];
            //args[0] = "TEST";

            var result = "DeviceNotFound";
            if (args == null || args.Length == 0)
                result = "Error, pass the device name as parameter!";
            else
            {
                try
                {
                    var deviceName = args[0];
                    var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                    foreach (var endpoint in
                             enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
                    {
                        if (endpoint.FriendlyName.ToLower().Contains(deviceName.ToLower()))
                        {
                            // Create a new audio PolicyConfigClient
                            var client = new PolicyConfigClient();
                            // Using PolicyConfigClient, set the given device as the default playback communication device
                            client.SetDefaultEndpoint(endpoint.ID, ERole.eCommunications);
                            // Using PolicyConfigClient, set the given device as the default playback device
                            client.SetDefaultEndpoint(endpoint.ID, ERole.eMultimedia);
                            result = "Successful";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }

            Console.WriteLine(result);
        }
    }
}
