using NUnit.Framework;
using System.Linq;

namespace Chip8.Tests
{
    public class TestRoms
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Runs_test_opcode_Successfully()
        {
            byte[] ok =
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x00, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x00, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01,
                0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x01, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
                0x01, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x01, 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x01, 0x01, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            var vm = Vm.NewVm(new MockWindow(), @"Roms\test_opcode.ch8");
            foreach(var _ in Enumerable.Range(0, 512))
            {
                vm.EmulateCycle();
            }

            CollectionAssert.AreEqual(vm.GetScreenBuffer(), ok);
        }
    }
}