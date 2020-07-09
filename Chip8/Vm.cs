using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chip8
{
    class Vm
    {
        private ushort opcode;
        private ushort I;
        private ushort pc;
        private ushort[] stack;
        private int sp;

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

                pc      = RomStart,
                opcode  = 0,
                I       = 0,

                stack           = new ushort[12],
                sp    = -1,
                memory          = new byte[4096],
                gfx             = new bool[64 * 32],
                V               = new byte[16],

                key             = new byte[16]
            };
            vm.LoadRom(rom);

            return vm;
        }

        #region OpCodes
        private void OpCode1NNN()
        {
            pc = (ushort)(opcode & 0x0FFF);
        }

        private void OpCode00E0()
        {
            gfx = new bool[64 * 32];
            pc += 2;
        }

        private void OpCode00EE()
        {
            pc = stack[sp--];
        }

        private void OpCode2NNN()
        {
            stack[++sp] = (ushort)(pc + 2);
            pc = (ushort)(opcode & 0x0FFF);
        }

        private void OpCodeANNN()
        {
            I = (ushort)(opcode & 0x0FFF);
            pc += 2;
        }

        private void OpCode6XNN()
        {
            V[opcode & 0x0F00 >> 8] = (byte) (opcode & 0x00FF);
            pc += 2;
        }

        private void OpCodeDXYN()
        {
            int X = (opcode & 0x0F00) >> 8;
            int Y = (opcode & 0x00F0) >> 4;
            int N = opcode & 0x000F;

            for (int i = 0; i < N; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    gfx[(V[X] + V[Y] * 64) + i * 64 + j] = ((memory[I + i] >> (7 - j)) & 0x1) > 0;
                }
            }            
            DebugGraphics();

            pc += 2;
        }

        private void OpCode7XNN()
        {
            V[(opcode & 0x0F00) >> 8] += (byte)(opcode & 0x00FF);
            pc += 2;
        }
        #endregion

        public void EmulateCycle()
        {
            opcode = (ushort)(memory[pc] << 8| memory[pc + 1]);

            switch (opcode & 0xF000)
            {
                case 0x0000 when opcode == 0x00E0:
                    OpCode00E0();
                    break;
                case 0x0000 when opcode == 0x00EE:
                    OpCode00EE();
                    break;
                case 0x0000:
                    break;
                case 0x1000:
                    OpCode1NNN();
                    break;
                case 0x2000:
                    OpCode2NNN();
                    break;
                case 0x3000:
                    break;
                case 0x4000:
                    break;
                case 0x5000:
                    break;
                case 0x6000:
                    OpCode6XNN();
                    break;
                case 0x7000:
                    OpCode7XNN();
                    break;
                case 0x8000 when (opcode & 0x000F) == 0:
                    break;
                case 0x8000 when (opcode & 0x000F) == 1:
                    break;
                case 0x8000 when (opcode & 0x000F) == 2:
                    break;
                case 0x8000 when (opcode & 0x000F) == 3:
                    break;
                case 0x8000 when (opcode & 0x000F) == 4:
                    break;
                case 0x8000 when (opcode & 0x000F) == 5:
                    break;
                case 0x8000 when (opcode & 0x000F) == 6:
                    break;
                case 0x8000 when (opcode & 0x000F) == 7:
                    break;
                case 0x8000 when (opcode & 0x000F) == 0xE:
                    break;
                case 0xA000:
                    OpCodeANNN();
                    break;
                case 0xB000:
                    break;
                case 0xC000:
                    break;
                case 0xD000:
                    OpCodeDXYN();
                    break;
                case 0xE000 when (opcode & 0x00FF) == 0x9E:
                    break;
                case 0xE000 when (opcode & 0x00FF) == 0xA1:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x07:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x0A:
                    break; 
                case 0xF000 when (opcode & 0x00FF) == 0x15:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x18:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x1E:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x29:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x33:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x55:
                    break;
                case 0xF000 when (opcode & 0x00FF) == 0x65:
                    break;
                default:
                    throw new InvalidOperationException($"Invalid opcode: {opcode:X4} @ PC = 0x{pc:X3}");
            }            
        }

        public void DebugRegisters()
        {
            var output = new StringBuilder();
            output.AppendLine($"pc              0x{pc:X4}");
            output.AppendLine($"opcode          0x{opcode:X4}");
            output.AppendLine($"I               0x{I:X4}");
            output.AppendLine($"sp              0x{sp:X4}");

            foreach(var register in Enumerable.Range(0, 16)) {
                output.AppendLine($"V{register:X}              0x{V[register]:X2}");
            }

            Console.WriteLine(output);
        }

        public void DebugMemory()
        {
            var output = new StringBuilder();

            for (int i = 0; i <= 0xfff; i += 8)
            {
                output.Append($"0x{i:X3}:");
                output.Append($" 0x{memory[i]:X2}");
                output.Append($" 0x{memory[i + 1]:X2}");
                output.Append($" 0x{memory[i + 2]:X2}");
                output.Append($" 0x{memory[i + 3]:X2}");
                output.Append($" 0x{memory[i + 4]:X2}");
                output.Append($" 0x{memory[i + 5]:X2}");
                output.Append($" 0x{memory[i + 6]:X2}");
                output.Append($" 0x{memory[i + 7]:X2}");
                output.AppendLine();
            }

            Console.WriteLine(output);
        }

        public void DebugGraphics()
        {
            var output = new StringBuilder();

            output.AppendLine(" ----------------------------------------------------------------");
            for (int i = 0; i < 32; i++)
            {
                output.Append("|");
                for (int j = 0; j < 64; j++)
                {
                    output.Append(gfx[i * 64 + j] ? "█" : " ");
                }
                output.AppendLine("|");
            }
            output.AppendLine(" ----------------------------------------------------------------");

            Console.WriteLine(output);
        }
    }
}
