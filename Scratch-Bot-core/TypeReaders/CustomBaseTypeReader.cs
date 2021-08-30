using Scratch_Bot_core;
using Scratch_Bot_core.Modules;
using Discord;
using Discord.Commands;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace Scratch_Bot_core.TypeReaders
{
    public class DiceTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            DiceType result;
            string patern = @"^d[0-9][1-9]*[1-9]*$";

            Match match = Regex.Match(input, patern);
            if (match.Success)
            {
                string parsedInput = match.Value;
                result = Enum.Parse<DiceType>(parsedInput);
                return Task.FromResult(TypeReaderResult.FromSuccess(result));
            }

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "input could not be parsed as boolean"));
        }
    }
}