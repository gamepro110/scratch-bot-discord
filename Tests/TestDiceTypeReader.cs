using Discord.Commands;

using NUnit.Framework.Internal;

using Scratch_Bot_Lib;
using Scratch_Bot_Lib.Modules;

namespace scratch_bot_tests
{
    public class TestDiceTypeReader
    {
        [TestCase("d2", ExpectedResult = DiceType.d2)]
        [TestCase("d4", ExpectedResult = DiceType.d4)]
        [TestCase("d6", ExpectedResult = DiceType.d6)]
        [TestCase("d8", ExpectedResult = DiceType.d8)]
        [TestCase("d10", ExpectedResult = DiceType.d10)]
        [TestCase("d12", ExpectedResult = DiceType.d12)]
        [TestCase("d20", ExpectedResult = DiceType.d20)]
        [TestCase("d100", ExpectedResult = DiceType.d100)]
        public DiceType ParseExpected(string input)
        {
            return Enum.Parse<DiceType>(input);
        }

        [TestCase("+afsd")]
        [TestCase("fasdfasdfdsasdfafdsfadsfadsfadsadfsfds")]
        [TestCase("!@#$%^&*()")]
        [TestCase("d3")]
        [TestCase("d3")]
        [TestCase("d7")]
        [TestCase("d9")]
        [TestCase("d11")]
        [TestCase("d13")]
        [TestCase("d19")]
        [TestCase("d21")]
        [TestCase("d99")]
        [TestCase("d101")]
        public void ParseUnexpected(string input)
        {
            DiceType actual = DiceType.invalid;
            Assert.Catch<ArgumentException>(
                () =>
                {
                    actual = Enum.Parse<DiceType>(input);
                }
            );
            Assert.That(actual, Is.EqualTo(DiceType.invalid));
        }
    }
}