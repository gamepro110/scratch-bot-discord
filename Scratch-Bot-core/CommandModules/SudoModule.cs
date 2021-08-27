using Discord;
using Discord.Commands;

namespace Scratch_Bot_core.Modules
{
    [Group("Sudo")]
    public partial class SudoModule : CustomBaseModule
    {
        [Command("Ban")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser user, [Remainder] string? reason = null)
        {
            await user.Guild.AddBanAsync(user, 0, reason);
            await ReplyAsync("ok!");
        }

        [Command("purge")]
        [Summary("Cleanup x messages. (calling message will also be cleaned up, default = 10)")]
        [RequireUserPermission(ChannelPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeMessages(int _amount = 10)
        {
            EmbedBuilder _em = new();

            if (_amount <= 0)
            {
                _em.Color = Color.DarkBlue;
                _em.Title = "cant delete anything if you dont give me an amount to delete...";
            }
            else
            {
                IEnumerable<IMessage> _messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, _amount).FlattenAsync();
                IEnumerable<IMessage> _filteredMessages = _messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14); // trying to bulk delete messages older than 14 days will result in a bad request!!
                int _filteredCount = _filteredMessages.Count();

                if (_filteredCount == 0)
                {
                    _em.Color = Color.DarkBlue;
                    _em.Title = "got nothing to delete tho...";
                }
                else
                {
                    await (Context.Channel as ITextChannel).DeleteMessagesAsync(_filteredMessages);//deletes previous messages
                    await (Context.Channel as ITextChannel).DeleteMessageAsync(Context.Message);//deletes calling messages

                    _em.Color = Color.Green;
                    _em.Title = "did thing. you proud??";
                    _em.Description = $"deleted {_filteredCount + 1} {(_filteredCount > 1 ? "messages" : "message")}.";
                }
            }

            await ReplyAsync(embed: _em.Build());
        }
    }

    public class HelpModule : CustomBaseModule
    {
        public HelpModule(CommandService _commandService, IServiceProvider _serviceProvider)
        {
            commandService = _commandService;
            provider = _serviceProvider;
        }

        Func<IEnumerable<CommandInfo>, string> PrintModuleCommands = (IEnumerable<CommandInfo> cmdInfo) =>
        {
            string output = "";

            foreach (var item in cmdInfo)
            {
                output += $"   {item.Name}\n";
            }

            return output;
        };

        public void RecursiveListLoop(IEnumerable<ModuleInfo> modules, ref string text, int givenDepth = 0)
        {
            foreach (var mod in modules)
            {
                text += $"Module: {mod.Name}\n";
                text += "cmd:\n";
                text += PrintModuleCommands(mod.Commands);
                text += "submods:\n";
                RecursiveListLoop(mod.Submodules, ref text, givenDepth++);
                text += "\n";
            }
            return;
        }

        [Command("h",true)]
        public async Task Task(string name = "")
        {
            string txt = "";
            RecursiveListLoop(commandService.Modules, ref txt);

            EmbedBuilder builder = new()
            {
                Title = "all commands",
                Description = txt,
            };

            SendEmbed(builder);
        }

        [Command("help")]
        [Summary("Displays All Commands")]
        public async Task Help(string _name = "")
        {
            EmbedBuilder _embed = new();
            ModuleInfo _mod;
            if (string.IsNullOrEmpty(_name))
            {
                _mod = commandService.Modules.FirstOrDefault(m => m.Name.Replace("Module", "").ToLower() != "");
                if (_mod == null)
                {
                    await ReplyAsync("bot confussion...");
                    return;
                }

                _embed.Description = $"{_mod.Summary}\n" +
                                     (!string.IsNullOrEmpty(_mod.Remarks) ? $"{_mod.Remarks}" : $"") +
                                     (_mod.Submodules.Any() ? $"Sub mods: {string.Join(", ", _mod.Submodules.Select(m => m.Name))}" : "");
            }
            else
            {
                _mod = commandService.Modules.FirstOrDefault(m => m.Name.Replace("Module", "").ToLower() == _name.ToLower());
                if (_mod == null)
                {
                    await ReplyAsync("No mod with that name was found...");
                    return;
                }

                _embed.Description = _mod.Submodules.Any() ? $"Sub mods: {string.Join(", ", _mod.Submodules.Select(m => m.Name))}" : "";
            }

            _embed.Title = _mod.Name;
            _embed.Color = Color.DarkBlue;

            AddCommands(_mod, ref _embed);

            await ReplyAsync(embed: _embed.Build());
        }

        private void AddCommands(ModuleInfo mod, ref EmbedBuilder embed)
        {
            foreach (CommandInfo item in mod.Commands)
            {
                item.CheckPreconditionsAsync(Context, provider).GetAwaiter().GetResult();
                AddCommand(item, ref embed);
            }
        }

        private static void AddCommand(CommandInfo info, ref EmbedBuilder embed)
        {
            string value = string.Format(
                "**how to use** `{0}{1}`\n{2}\n{3}",
                Settings.CommandPrefix,
                info.Aliases[0],
                info.Summary,
                (string.IsNullOrEmpty(info.Remarks) ? "" : '\n' + info.Remarks + '\n')
                );

            embed.AddField(f =>
            {
                f.Name = $"__**{info.Name}**__";
                f.Value = value;
            });
        }

        private readonly CommandService commandService;
        private readonly IServiceProvider provider;
    }
}
