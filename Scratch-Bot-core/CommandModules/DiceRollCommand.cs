using Discord;
using Discord.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    public enum DiceType
    {
        d4 = 4,
        d6 = 6,
        d8 = 8,
        d10 = 10,
        d12 = 12,
        d20 = 20,
        d100 = 100,
    }

    class DiceRoll
    {
        public DiceRoll(DiceType type)
        {
            TypeOfDice = type;
        }

        public DiceRoll Roll()
        {
            value = new Random().Next(0, (int)TypeOfDice) + 1;
            return this;
        }

        public int value;
        private DiceType TypeOfDice { get; init; }
    }

    
    public partial class EmptyModule : CustomBaseModule
    {
        [Command("Roll")]
        public async Task RollXAmountDice(int amountDice, DiceType Die)
        {
            EmbedBuilder builder = new()
            {
                Color = Color.Orange,
                Title = $"{amountDice}{Die}",
            };

            List<DiceRoll> rolls = new();

            for (int i = 0; i < amountDice; i++)
            {
                rolls.Add(new DiceRoll(Die).Roll());
            }

            foreach (var item in rolls)
            {
                builder.AddField(f =>
                {
                    f.Name = $"{rolls.IndexOf(item) + 1}e roll";
                    f.Value = $"{item.value}";
                    f.IsInline = true;
                });
            }

            int sum = 0;
            rolls.ForEach(roll => sum += roll.value);

            string txt = string.Format(
                "({0}+{1}) = {2}",
                (rolls.Count >2)?string.Join(
                    "+",
                    rolls
                        .GetRange(0, rolls.IndexOf(rolls[^2]))
                        .Select(r => r.value)
                ) : rolls[0].value,
                rolls[^1].value,
                sum
            );

            builder.AddField(f =>
            {
                f.Name = "Total";
                f.Value = txt;
            });

            await SendEmbed(builder);
        }
    }
}
