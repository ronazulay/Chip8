using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chip8
{
    class Vm
    {
        private ushort opcode;
        private ushort I;
        private ushort pc;
        private ushort sp;
        private ushort[] stack;

        private byte[] V;
        private byte[] memory;
        private bool[] gfx;

        private byte delayTimer;
        private byte soundTimer;

        private byte[] key;

        const ushort RomStart = 0x200;

        public void LoadRom(string rom)
        {
            var bytes = File.ReadAllBytes(rom);
            bytes.CopyTo(memory, RomStart);
        }

        public static Vm NewVm(string rom)
        {
            var vm = new Vm
            {

                pc      = 0x200,
                opcode  = 0,
                I       = 0,
                sp      = 0,

                stack   = new ushort[16],
                memory  = new byte[4096],
                gfx     = new bool[64 * 32],
                V       = new byte[16],

                key     = new byte[16]
            };
            vm.LoadRom(rom);

            return vm;
        }

        public void EmulateCycle()
        {

        }

        public void Debug()
        {
            var output = @"test
test
            test
             ";
            Console.WriteLine(output);
        }
    }
}
