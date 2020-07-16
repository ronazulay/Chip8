﻿using NUnit.Framework;
using System.Linq;

namespace Chip8.Tests
{
    public class TestOpCodes
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_OpCode00E0_ShouldClearScreen()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x00, 0xE0  // 0x00E0 - Clear the screen
            });
            vm.EmulateCycle();

            foreach(var pixel in vm.Gfx) {
                Assert.AreEqual(pixel, 0);
            }
        }

        [Test]
        public void Test_OpCode00EE_ShouldSetPCTo0x202()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x22, 0x04,  // Call subroutine at 0x204
                0x13, 0x37,  // Execution should continue here at 0x202
                0x00, 0xEE   // Return to caller

            });
            vm.EmulateCycles(2);

            Assert.AreEqual(vm.PC, 0x202);
        }

        [Test]
        public void Test_OpCode1NNN_ShouldSetPCTo0x123()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x11, 0x23,  // Jump to address 0x123
            });
            vm.EmulateCycle();

            Assert.AreEqual(vm.PC, 0x123);
        }

        [Test]
        public void Test_OpCode3XNN_ShouldSetPCTo0x206()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x60, 0x11,  // Set V0 to 0x11.
                0x30, 0x11   // Should skip the next instruction
            });
            vm.EmulateCycles(2);

            Assert.AreEqual(vm.PC, 0x206);
        }

        [Test]
        public void Test_OpCode4XNN_ShouldSetPCTo0x206()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x60, 0x11,  // Set V0 to 0x11.
                0x40, 0x12   // Should skip the next instruction
            });
            vm.EmulateCycles(2);

            Assert.AreEqual(vm.PC, 0x206);
        }

        [Test]
        public void Test_OpCode5XY0_ShouldSetPCTo0x208()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x60, 0x11,  // Set V0 to 0x11.
                0x61, 0x11,  // Set V1 to 0x11.
                0x50, 0x10   // Should skip the next instruction
            });
            vm.EmulateCycles(3);

            Assert.AreEqual(vm.PC, 0x208);
        }

        [Test]
        public void Test_OpCode6XNN_ShouldSetV0To0xAA()
        {
            var vm = Vm.NewVm(null, new byte[] {
                0x60, 0xAA,  // Set V0 to 0xAA.
            });
            vm.EmulateCycle();

            Assert.AreEqual(vm.V[0], 0xAA);
        }
    }
}