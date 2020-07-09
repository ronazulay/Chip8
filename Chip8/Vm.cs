using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chip8
{
    class Vm
    {
        private ushort OpCode;
        private ushort I;
        private ushort PC;
        private ushort[] Stack;
        private ushort SP;

        private byte[] V;
        private byte[] Memory;
        private byte[] Gfx;

        private byte delayTimer;
        private byte soundTimer;

        private byte[] key;

        const ushort RomStart = 0x200;

        public void LoadRom(string rom)
        {
            var bytes = File.ReadAllBytes(rom);
            bytes.CopyTo(Memory, RomStart);
        }

        public static Vm NewVm(string rom)
        {
            var vm = new Vm
            {

                PC      = RomStart,
                OpCode  = 0,
                I       = 0,

                Stack           = new ushort[12],
                SP              = 0,
                Memory          = new byte[4096],
                Gfx             = new byte[64 * 32],
                V               = new byte[16],

                key             = new byte[16]
            };
            vm.LoadRom(rom);

            return vm;
        }

        #region OPCodes
        private void OPCode1NNN(ushort NNN)
        {
            PC = NNN;
        }

        private void OPCode00E0()
        {
            Gfx = new byte[64 * 32];
            PC += 2;
        }

        private void OPCode00EE()
        {
            PC = Stack[SP--];
        }

        private void OPCode2NNN(ushort NNN)
        {
            Stack[++SP] = (ushort)(PC + 2);
            PC = NNN;
        }

        private void OPCodeANNN(ushort NNN)
        {
            I = NNN;
            PC += 2;
        }

        private void OPCode6XNN(ushort X, ushort NN)
        {
            V[X] = (byte) NN;
            PC += 2;
        }

        // Source: https://stackoverflow.com/questions/17346592/how-does-chip-8-graphics-rendered-on-screen
        private void OPCodeDXYN(byte X, byte Y, byte N)
        {
            // Initialize the collision detection as no collision detected (yet).
            V[0xF] = 0;

            // Draw up to N lines on the screen.
            for (int line = 0; line < N; line++)
            {
                // y is the starting line Y + current line. If y is larger than the total width of the screen then wrap around (this is the modulo operation).
                var y = (V[Y] + line) % 32;

                // The current sprite being drawn, each line is a new sprite.
                byte sprite = Memory[I + line];

                // Each bit in the sprite is a pixel on or off.
                for (int column = 0; column < 8; column++)
                {
                    // Start with the current most significant bit. The next bit will be left shifted in from the right.
                    if((sprite & 0x80) != 0)
                    {
                        // Get the current x position and wrap around if needed.
                        var x = (V[X] + column) % 64;

                        // Collision detection: If the target pixel is already set then set the collision detection flag in register VF.
                        if(Gfx[y * 64 + x] == 1)
                        {
                            V[0xF] = 1;
                        }

                        // Enable or disable the pixel (XOR operation).
                        Gfx[y * 64 + x] ^= 1;
                    }

                    // Shift the next bit in from the right.
                    sprite <<= 0x1;
                }
            }

            PC += 2;
            DebugGraphics();
        }

        private void OPCode7XNN(byte X, byte NN)
        {
            V[X] += NN;
            PC += 2;
        }

        private void OPCode0NNN(ushort NNN)
        {
            throw new NotImplementedException($"{OpCode:4X} has not been implemented.");
        }

        private void OPCode3XNN(byte X, ushort NN)
        {
            if (V[X] == NN)
                PC += 2;

            PC += 2;
        }

        private void OPCode4XNN(byte X, ushort NN)
        {
            if (V[X] != NN)
                PC += 2;

            PC += 2;
        }

        private void OPCode5XNN(byte X, byte Y)
        {
            if (V[X] == V[Y])
                PC += 2;

            PC += 2;
        }

        private void OPCode8XY0(byte X, byte Y)
        {
            V[X] = V[Y];
            PC += 2;
        }

        private void OPCode8XY1(byte X, byte Y)
        {
            V[X] = (byte) (V[X] | V[Y]);
            PC += 2;
        }

        private void OPCode8XY2(byte X, byte Y)
        {
            V[X] = (byte)(V[X] & V[Y]);
            PC += 2;
        }

        private void OPCode8XY3(byte X, byte Y)
        {
            V[X] = (byte)(V[X] ^ V[Y]);
            PC += 2;
        }

        private void OPCode8XY4(byte X, byte Y)
        {
            int sum = V[X] + V[Y];
            V[X] = (byte)(sum & 0xFF);
            V[0xF] = (byte)(sum > 0xFF ? 1 : 0);
            
            PC += 2;
        }

        private void OPCode8XY5(byte X, byte Y)
        {
            int diff = V[X] - V[Y];
            V[X] = (byte)(diff & 0xFF);
            V[0xF] = (byte)(V[Y] > V[X] ? 1 : 0);

            PC += 2;
        }

        private void OPCode8XY6(byte X, byte Y)
        {
            V[0xF] = (byte)(V[X] & 0x1);
            V[X] >>= 0x1;
            
            PC += 2;
        }

        private void OPCode8XY7(byte X, byte Y)
        {
            int diff = V[Y] - V[X];
            V[X] = (byte)(diff & 0xFF);
            V[0xF] = (byte)(diff > 0 ? 1 : 0);
            
            PC += 2;
        }

        private void OPCode8XYE(byte X, byte Y)
        {
            V[0xF] = (byte)((V[X] & 0x80) >> 7);
            V[X] <<= 0x1;
            
            PC += 2;
        }

        private void OPCodeBNNN(ushort NNN)
        {
            PC = (ushort) (NNN + V[0]);
        }
        
        private void OPCodeCXNN(byte X, byte NN)
        {
            var random = new Random();
            V[X] = (byte) (random.Next(0, 0xFF) & NN);

            PC += 2;
                
        }

        private void OPCodeFX1E(byte X)
        {
            I += V[X];
            PC += 2;
        }

        private void OPCodeFX65(byte X)
        {
            for(int i = 0; i <= X; i++)
            {
                V[i] = Memory[I + i];
            }

            PC += 2;
        }

        private void OPCodeFX55(byte X)
        {
            for (int i = 0; i <= X; i++)
            {
                Memory[I + i] = V[i];
            }

            PC += 2;
        }
        #endregion

        public void EmulateCycle()
        {
            var sw = Stopwatch.StartNew();

            OpCode = (ushort)(Memory[PC] << 8| Memory[PC + 1]);

            ushort NNN  = (ushort)(OpCode & 0x0FFF);
            byte NN     = (byte)(OpCode & 0x00FF);
            byte N      = (byte)(OpCode & 0x000F);
            byte X      = (byte)((OpCode & 0x0F00) >> 8);
            byte Y      = (byte)((OpCode & 0x00F0) >> 4);          

            switch (OpCode & 0xF000)
            {
                case 0x0000 when OpCode == 0x00E0:
                    OPCode00E0();
                    break;
                case 0x0000 when OpCode == 0x00EE:
                    OPCode00EE();
                    break;
                case 0x0000:
                    OPCode0NNN(NNN);
                    break;
                case 0x1000:
                    OPCode1NNN(NNN);
                    break;
                case 0x2000:
                    OPCode2NNN(NNN);
                    break;
                case 0x3000:
                    OPCode3XNN(X, NN);
                    break;
                case 0x4000:
                    OPCode4XNN(X, NN);
                    break;
                case 0x5000:
                    OPCode5XNN(X, Y);
                    break;
                case 0x6000:
                    OPCode6XNN(X, NN);
                    break;
                case 0x7000:
                    OPCode7XNN(X, NN);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 0:
                    OPCode8XY0(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 1:
                    OPCode8XY1(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 2:
                    OPCode8XY2(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 3:
                    OPCode8XY3(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 4:
                    OPCode8XY4(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 5:
                    OPCode8XY5(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 6:
                    OPCode8XY6(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 7:
                    OPCode8XY7(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 0xE:
                    OPCode8XYE(X, Y);
                    break;
                case 0xA000:
                    OPCodeANNN(NNN);
                    break;
                case 0xB000:
                    OPCodeBNNN(NNN);
                    break;
                case 0xC000:
                    OPCodeCXNN(X, NN);
                    break;
                case 0xD000:
                    OPCodeDXYN(X, Y, N);
                    break;
                case 0xE000 when (OpCode & 0x00FF) == 0x9E:
                    DebugRegisters();
                    break;
                case 0xE000 when (OpCode & 0x00FF) == 0xA1:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x07:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x0A:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x15:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x18:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x1E:
                    OPCodeFX1E(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x29:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x33:
                    DebugRegisters();
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x55:
                    OPCodeFX55(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x65:
                    OPCodeFX65(X);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid OpCode: {OpCode:X4} @ PC = 0x{PC:X3}");
            }

            var ms = sw.ElapsedMilliseconds;
            int frame = 1000 / 60;

            if(ms < frame)
            {
                Thread.Sleep((int) (frame - ms));
            }
        }

        public void DebugRegisters()
        {
            var output = new StringBuilder();
            output.AppendLine($"PC              0x{PC:X4}");
            output.AppendLine($"OpCode          0x{OpCode:X4}");
            output.AppendLine($"I               0x{I:X4}");
            output.AppendLine($"sp              0x{SP:X4}");

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
                output.Append($" 0x{Memory[i]:X2}");
                output.Append($" 0x{Memory[i + 1]:X2}");
                output.Append($" 0x{Memory[i + 2]:X2}");
                output.Append($" 0x{Memory[i + 3]:X2}");
                output.Append($" 0x{Memory[i + 4]:X2}");
                output.Append($" 0x{Memory[i + 5]:X2}");
                output.Append($" 0x{Memory[i + 6]:X2}");
                output.Append($" 0x{Memory[i + 7]:X2}");
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
                    output.Append(Gfx[i * 64 + j] > 0 ? "█" : " ");
                }
                output.AppendLine("|");
            }
            output.AppendLine(" ----------------------------------------------------------------");


            Console.Clear();
            Console.WriteLine(output);
        }
    }
}
