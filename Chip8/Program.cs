using System;
using System.IO;

namespace Chip8
{
    class Program
    {
        static void Main(string[] args)
        {
            var vm = Vm.NewVm("test.rom");

            while(true)
            {
                vm.EmulateCycle();
            }
        }
    }
}
