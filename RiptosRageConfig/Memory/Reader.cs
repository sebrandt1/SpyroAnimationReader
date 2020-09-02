using System;
using System.Linq;
using System.Diagnostics;
using RiptosRageConfig.SpyroProperties;
using System.Threading;
using System.Windows.Input;

namespace RiptosRageConfig.Memory
{
    public class Reader
    {
        private static string processName = "ePSXe";
        private static VAMemory VAM;
        private static Process MainProcess;
        private static IntPtr BaseAddress = IntPtr.Zero;
        static int Frame = 0;

        static Reader()
        {
            MainProcess = Process.GetProcessesByName(processName).FirstOrDefault();

            if (MainProcess != null)
            {
                BaseAddress = MainProcess.MainModule.BaseAddress;
                VAM = new VAMemory(processName);
            }
            else
                Console.WriteLine("Could not locate ePSXe process, are you sure it's running?");
        }

        private int GetLocAddress(int address)
        {
            return VAM.ReadInt32(BaseAddress + address);
        }

        private object GetValueOfAddress(int locAddress, int offset, string expectedType)
        {
            switch(expectedType.ToLower())
            {
                case "int32":
                    return VAM.ReadInt32((IntPtr)locAddress + offset);

                case "float":
                    return VAM.ReadFloat((IntPtr)locAddress + offset);

                default:
                    return null;
            }
        }

        private void SetValueOfAddress(int locAddress, int offset, object value, string type = null)
        {
            if(type.ToLower() == "int32")
                VAM.WriteInt32((IntPtr)locAddress + offset, (int)value);
            if (type.ToLower() == "float")
                VAM.WriteFloat((IntPtr)locAddress + offset, (int)value);
        }

        public void Read()
        {
            while (true)
            {
                ReadAnimation();

                Frame++;
                Thread.Sleep(33);
            }
        }
        private int ReadAnimation()
        {
            if (BaseAddress != IntPtr.Zero)
            {
                int anim = (int)GetValueOfAddress(GetLocAddress(Addresses.a_Animation), Addresses.o_Animation, "int32");
                string animType = Animations.AnimConversions.ContainsKey(anim) ? Animations.AnimConversions[anim] : "unknown state";

                Console.ForegroundColor = Animations.AnimConversions.ContainsKey(anim) ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine("{0,-40}f: [{1}]", new object[]
                    {
                            "[" + animType + "]",
                            Frame
                    });
                return anim;
            }
            return -1;
        }
    }
}
