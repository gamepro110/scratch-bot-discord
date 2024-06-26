﻿using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib.Modules
{
    public enum DiceType
    {
        invalid = -1,
        d2 = 2,
        d4 = 4,
        d6 = 6,
        d8 = 8,
        d10 = 10,
        d12 = 12,
        d20 = 20,
        d100 = 100,
    }

    public class DiceRoll(DiceType type)
    {
        public DiceRoll Roll()
        {
            if (TypeOfDice == DiceType.invalid)
            {
                value = 0;
                return this;
            }

            Random rand = new();
            const int max = 100000;

            long val = rand.Next(max) * rand.Next(max);
            if (val < 0)
            {
                val *= -1;
            }
            value = 1 + Convert.ToInt32(val % (int)TypeOfDice);
            return this;
        }

        public int value;
        private DiceType TypeOfDice { get; init; } = type;
    }

    public partial class EmptyModule : CustomBaseModule
    {
        [Command("temp")]
        public async Task TempTask(int num = 30)
        {
            for (int i = 0; i < num; i++)
            {
                await ReplyAsync($"text {i}");
            }

            await ReplyAsync("done");
        }

        [Command("Roll")]
        [Remarks("Roll a D2, 4, 6, 8, 10, 12, 20, 100")]
        public async Task RollDice(DiceType diceToRoll)
        {
            EmbedBuilder builder = new()
            {
                Color = Color.Orange,
                Title = $"{diceToRoll}"
            };

            DiceRoll roll = new DiceRoll(diceToRoll).Roll();

            builder.Description = $"{roll.value}";

            await SendEmbed(builder);
        }

        [Command("Roll")]
        [Remarks("Roll n D4, 6, 8, 10, 12, 20, 100 dice (n == any positive number)")]
        public async Task RollXAmountDice(int amountOfDice, DiceType diceToRoll) // TODO add field limit of 25
        {
            EmbedBuilder builder = new()
            {
                Color = Color.Orange,
                Title = $"{amountOfDice}{diceToRoll}",
            };

            List<DiceRoll> rolls = [];

            for (int i = 0; i < amountOfDice; i++)
            {
                rolls.Add(new DiceRoll(diceToRoll).Roll());
            }

            if (amountOfDice > 1)
            {
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
                    (rolls.Count > 2) ? string.Join(
                        "+",
                        rolls
                            .GetRange(0, rolls.Count - 1)
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
            }
            else
            {
                DiceRoll roll = new DiceRoll(diceToRoll).Roll();
                builder.Description = $"{roll.value}";
            }

            await SendEmbed(builder);
        }
    }
}