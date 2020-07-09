using System;
using System.IO;

namespace Chip8
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("./Chip8Emulator.exe <Path to ROM>");
                return;
            }

            var vm = Vm.NewVm(args[0]);

            while(true)
            {
                vm.EmulateCycle();
            }
        }
    }
}
