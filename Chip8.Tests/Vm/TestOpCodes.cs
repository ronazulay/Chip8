using NUnit.Framework;
using System.Linq;

namespace Chip8.Tests
{
    public class TestOpcodes
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_OpCode00E0_ShouldClearScreen()
        {
            var vm = Vm.NewVm(new MockWindow(), new byte[] {
                0x00, 0xE0  // 0x00E0 - Clear the screen
            });
            vm.EmulateCycle();

            foreach(var pixel in vm.Gfx) {
                Assert.AreEqual(pixel, 0);
            }
        }

        [Test]
        public void Test_OpCode00EE_ShouldReturnToPC0x202()
        {
            var vm = Vm.NewVm(new MockWindow(), new byte[] {
                0x22, 0x04,  // 0x2204 - Call subroutine at 0x204
                0x13, 0x37,  // Execution should continue here at 0x202
                0x00, 0xEE   // 0x00EE - Return to caller

            });
            vm.EmulateCycle();
            vm.EmulateCycle();

            Assert.AreEqual(vm.PC, 0x202);
        }
    }
}