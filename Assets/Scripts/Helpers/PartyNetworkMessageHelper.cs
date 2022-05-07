using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using PlayFabStudy.Models;

namespace PlayFabStudy.Helpers
{
    public class PartyNetworkMessageHelper
    {
        public static void BufferData<T>(T data, out byte[] buffer, out IntPtr unmanagedPointer)
        {
            var json = JsonUtility.ToJson(data);
            buffer = Encoding.ASCII.GetBytes(json);
            var size = Marshal.SizeOf(buffer[0]) * buffer.Length;
            unmanagedPointer = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, 0, unmanagedPointer, buffer.Length);
        }

        public static string GetStringFromBuffer(IntPtr buffer, uint bufferSize)
        {
            var managerdArray = new byte[bufferSize];
            Marshal.Copy(buffer, managerdArray, 0, (int)bufferSize);
            return Encoding.ASCII.GetString(managerdArray);
        }

        public static T GetParsedDataFromBuffer<T>(IntPtr buffer, uint bufferSize)
        {
            try
            {
                var json = GetStringFromBuffer(buffer, bufferSize);
                return JsonUtility.FromJson<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public static T GetDataFromMessage<T>(IntPtr buffer, uint bufferSize)
        {
            return GetParsedDataFromBuffer<PartyNetworkMessage<T>>(buffer, bufferSize).MessageData;
        }

        public static PartyNetworkMessageTypes GetTypeFromMessage(IntPtr buffer, uint bufferSize)
        {
            return GetParsedDataFromBuffer<PartyNetworkMessage<object>>(buffer, bufferSize).MessageType;
        }
    } 
}
