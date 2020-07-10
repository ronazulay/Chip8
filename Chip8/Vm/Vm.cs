using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Chip8
{
    class Vm
    {
        private byte[] Fonts =
        {
              0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
              0x20, 0x60, 0x20, 0x20, 0x70, // 1
              0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
              0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
              0x90, 0x90, 0xF0, 0x10, 0x10, // 4
              0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
              0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
              0xF0, 0x10, 0x20, 0x40, 0x40, // 7
              0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
              0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
              0xF0, 0x90, 0xF0, 0x90, 0x90, // A
              0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
              0xF0, 0x80, 0x80, 0x80, 0xF0, // C
              0xE0, 0x90, 0x90, 0x90, 0xE0, // D
              0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
              0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        private ushort OpCode;
        private ushort I;
        private ushort PC;
        private ushort[] Stack;
        private ushort SP;
        private byte DelayTimer;
        private byte SoundTimer;

        private byte[] V;
        private byte[] Memory;
        private byte[] Gfx;

        private bool[] Keys;

        const ushort RomStart = 0x200;

        private Window Window;

        public static Vm NewVm(Window window, string rom)
        {
            var vm = new Vm
            {
                PC      = RomStart,
                OpCode  = 0,
                I       = 0,

                Stack   = new ushort[12],
                SP      = 0,
                Memory  = new byte[4096],
                Gfx     = new byte[64 * 32],
                V       = new byte[16],
                Keys    = new bool[16],

                Window = window,
                
                SoundTimer = 0,
                DelayTimer = 0
            };
            vm.LoadFonts();
            vm.LoadRom(rom);

            return vm;
        }

        private void LoadFonts()
        {
            Fonts.CopyTo(Memory, 0x0);
        }

        private void LoadRom(string rom)
        {
            var bytes = File.ReadAllBytes(rom);
            bytes.CopyTo(Memory, RomStart);
        }

        public void KeyUp(byte key)
        {
            Keys[key] = false;
        }

        public void KeyDown(byte key)
        {
            Keys[key] = true;
        }

        public byte[] GetScreenBuffer()
        {
            return Gfx;
        }
        
        public void EmulateCycle()
        {
            OpCode = (ushort)(Memory[PC] << 8| Memory[PC + 1]);

            ushort NNN  = (ushort)(OpCode & 0x0FFF);
            byte NN     = (byte)(OpCode & 0x00FF);
            byte N      = (byte)(OpCode & 0x000F);
            byte X      = (byte)((OpCode & 0x0F00) >> 8);
            byte Y      = (byte)((OpCode & 0x00F0) >> 4);          

            switch (OpCode & 0xF000)
            {
                case 0x0000 when OpCode == 0x00E0:
                    OpCode00E0();
                    break;
                case 0x0000 when OpCode == 0x00EE:
                    OpCode00EE();
                    break;
                case 0x0000:
                    OpCode0NNN(NNN);
                    break;
                case 0x1000:
                    OpCode1NNN(NNN);
                    break;
                case 0x2000:
                    OpCode2NNN(NNN);
                    break;
                case 0x3000:
                    OpCode3XNN(X, NN);
                    break;
                case 0x4000:
                    OpCode4XNN(X, NN);
                    break;
                case 0x5000:
                    OpCode5XNN(X, Y);
                    break;
                case 0x6000:
                    OpCode6XNN(X, NN);
                    break;
                case 0x7000:
                    OpCode7XNN(X, NN);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 0:
                    OpCode8XY0(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 1:
                    OpCode8XY1(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 2:
                    OpCode8XY2(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 3:
                    OpCode8XY3(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 4:
                    OpCode8XY4(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 5:
                    OpCode8XY5(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 6:
                    OpCode8XY6(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 7:
                    OpCode8XY7(X, Y);
                    break;
                case 0x8000 when (OpCode & 0x000F) == 0xE:
                    OpCode8XYE(X, Y);
                    break;
                case 0xA000:
                    OpCodeANNN(NNN);
                    break;
                case 0xB000:
                    OpCodeBNNN(NNN);
                    break;
                case 0xC000:
                    OpCodeCXNN(X, NN);
                    break;
                case 0xD000:
                    OpCodeDXYN(X, Y, N);
                    break;
                case 0xE000 when (OpCode & 0x00FF) == 0x9E:
                    OpCodeEX9E(X);
                    break;
                case 0xE000 when (OpCode & 0x00FF) == 0xA1:
                    OpCodeEXA1(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x07:
                    OpCodeFX07(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x0A:
                    OpCodeFX0A(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x15:
                    OpCodeFX15(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x18:
                    OpCodeFX18(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x1E:
                    OpCodeFX1E(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x29:
                    OpCodeFX29(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x33:
                    OpCodeFX33(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x55:
                    OpCodeFX55(X);
                    break;
                case 0xF000 when (OpCode & 0x00FF) == 0x65:
                    OpCodeFX65(X);
                    break;
                default:
                    throw new InvalidOperationException($"error: Invalid OpCode: {OpCode:X4} @ PC = 0x{PC:X3}");
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            if (DelayTimer > 0) DelayTimer--;
            if (SoundTimer > 0)
            {
                SoundTimer--;
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

        #region OpCodes
        private void OpCode1NNN(ushort NNN)
        {
            PC = NNN;
        }

        private void OpCode00E0()
        {
            Gfx = new byte[64 * 32];
            PC += 2;
        }

        private void OpCode00EE()
        {
            PC = Stack[SP--];
        }

        private void OpCode2NNN(ushort NNN)
        {
            Stack[++SP] = (ushort)(PC + 2);
            PC = NNN;
        }

        private void OpCodeANNN(ushort NNN)
        {
            I = NNN;
            PC += 2;
        }

        private void OpCode6XNN(ushort X, ushort NN)
        {
            V[X] = (byte)NN;
            PC += 2;
        }

        // Source: https://stackoverflow.com/questions/17346592/how-does-chip-8-graphics-rendered-on-screen
        private void OpCodeDXYN(byte X, byte Y, byte N)
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
                    if ((sprite & 0x80) != 0)
                    {
                        // Get the current x position and wrap around if needed.
                        var x = (V[X] + column) % 64;

                        // Collision detection: If the target pixel is already set then set the collision detection flag in register VF.
                        if (Gfx[y * 64 + x] == 1)
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
        }

        private void OpCode7XNN(byte X, byte NN)
        {
            V[X] += NN;
            PC += 2;
        }

        private void OpCode0NNN(ushort NNN)
        {
            throw new NotImplementedException($"error: {OpCode:4X} has not been implemented.");
        }

        private void OpCode3XNN(byte X, ushort NN)
        {
            if (V[X] == NN)
                PC += 2;

            PC += 2;
        }

        private void OpCode4XNN(byte X, ushort NN)
        {
            if (V[X] != NN)
                PC += 2;

            PC += 2;
        }

        private void OpCode5XNN(byte X, byte Y)
        {
            if (V[X] == V[Y])
                PC += 2;

            PC += 2;
        }

        private void OpCode8XY0(byte X, byte Y)
        {
            V[X] = V[Y];
            PC += 2;
        }

        private void OpCode8XY1(byte X, byte Y)
        {
            V[X] = (byte)(V[X] | V[Y]);
            PC += 2;
        }

        private void OpCode8XY2(byte X, byte Y)
        {
            V[X] = (byte)(V[X] & V[Y]);
            PC += 2;
        }

        private void OpCode8XY3(byte X, byte Y)
        {
            V[X] = (byte)(V[X] ^ V[Y]);
            PC += 2;
        }

        private void OpCode8XY4(byte X, byte Y)
        {
            int sum = V[X] + V[Y];
            V[X] = (byte)(sum & 0xFF);
            V[0xF] = (byte)(sum > 0xFF ? 1 : 0);

            PC += 2;
        }

        private void OpCode8XY5(byte X, byte Y)
        {
            int diff = V[X] - V[Y];
            V[X] = (byte)(diff & 0xFF);
            V[0xF] = (byte)(V[Y] > V[X] ? 1 : 0);

            PC += 2;
        }

        private void OpCode8XY6(byte X, byte Y)
        {
            V[0xF] = (byte)(V[X] & 0x1);
            V[X] >>= 0x1;

            PC += 2;
        }

        private void OpCode8XY7(byte X, byte Y)
        {
            int diff = V[Y] - V[X];
            V[X] = (byte)(diff & 0xFF);
            V[0xF] = (byte)(diff > 0 ? 1 : 0);

            PC += 2;
        }

        private void OpCode8XYE(byte X, byte Y)
        {
            V[0xF] = (byte)((V[X] & 0x80) >> 7);
            V[X] <<= 0x1;

            PC += 2;
        }

        private void OpCodeBNNN(ushort NNN)
        {
            PC = (ushort)(NNN + V[0]);
        }

        private void OpCodeCXNN(byte X, byte NN)
        {
            var random = new Random();
            V[X] = (byte)(random.Next(0, 0xFF) & NN);

            PC += 2;

        }

        private void OpCodeFX1E(byte X)
        {
            I += V[X];
            PC += 2;
        }

        private void OpCodeFX65(byte X)
        {
            for (int i = 0; i <= X; i++)
            {
                V[i] = Memory[I + i];
            }

            PC += 2;
        }

        private void OpCodeFX55(byte X)
        {
            for (int i = 0; i <= X; i++)
            {
                Memory[I + i] = V[i];
            }

            PC += 2;
        }

        private void OpCodeFX0A(byte X)
        {
            bool keyPressed = false;

            while (!keyPressed)
            {
                for (byte i = 0; i < 0xF; i++)
                {
                    if (Keys[i])
                    {
                        V[X] = i;
                        keyPressed = true;
                        break;
                    }
                }
            }
            
            PC += 2;
        }

        private void OpCodeEX9E(byte X)
        {
            if (Keys[V[X]])
            {
                PC += 2;
            }
            PC += 2;
        }

        private void OpCodeEXA1(byte X)
        {
            if (!Keys[V[X]])
            {
                PC += 2;
            }
            PC += 2;
        }

        private void OpCodeFX15(byte X)
        {
            DelayTimer = V[X];
            PC += 2;
        }

        private void OpCodeFX18(byte X)
        {
            SoundTimer = V[X];
            PC += 2;
        }

        private void OpCodeFX07(byte X)
        {
            V[X] = DelayTimer;
            PC += 2;
        }

        private void OpCodeFX29(byte X)
        {
            I = (byte)(V[X] * 5);
            PC += 2;
        }

        private void OpCodeFX33(byte X)
        {
            Memory[I] = (byte)(V[X] / 100);
            Memory[I + 1] = (byte)((V[X] / 10) % 10);
            Memory[I + 2] = (byte)((V[X] / 100) % 10);

            PC += 2;
        }
        #endregion
    }
}
