using Scratch_Bot_Lib.Modules;

namespace scratch_bot_tests
{
    public class TestDiceRoll
    {
        [TestCase(DiceType.d2)]
        [TestCase(DiceType.d4)]
        [TestCase(DiceType.d6)]
        [TestCase(DiceType.d8)]
        [TestCase(DiceType.d10)]
        [TestCase(DiceType.d12)]
        [TestCase(DiceType.d20)]
        [TestCase(DiceType.d100)]
        public void TestRoll(DiceType type)
        {
            dice = new DiceRoll(type);

            RollDice100Times(ref dice, type);
        }

        [Test]
        public void TestInvalidDice()
        {
            dice = new(DiceType.invalid);

            Assert.That(dice.value, Is.EqualTo(0));
            dice.Roll();
            Assert.That(dice.value, Is.EqualTo(0));
        }

        private static void RollDice100Times(ref DiceRoll dice, DiceType type)
        {
            for (int i = 0; i < 100; i++)
            {
                dice.Roll();
                Assert.That(dice.value, Is.GreaterThan(0));
                Assert.That(dice.value, Is.LessThanOrEqualTo((int)type));
            }
        }

        private DiceRoll dice = new(DiceType.invalid);
    }
}