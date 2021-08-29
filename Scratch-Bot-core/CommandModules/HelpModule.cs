using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    public class HelpModule : CustomBaseModule
    {
        public HelpModule(CommandService _commandService, IServiceProvider _serviceProvider, LoggingService _loggingService)
        {
            commandService = _commandService;
            provider = _serviceProvider;
            loggingService = _loggingService;
        }

        Func<IEnumerable<CommandInfo>, string> ModuleCommands = (IEnumerable<CommandInfo> cmdInfo) =>
        {
            string output = "";

            foreach (var item in cmdInfo)
            {
                output += $"--{item.Name}\n";
            }

            return output;
        };

        public void PrintModules(IEnumerable<ModuleInfo> modules, ref string text)
        {
            foreach (var mod in modules)
            {
                text += $"Module: {mod.Name}\n";
                text += "{\n";
                text += ModuleCommands(mod.Commands);
                //text += "submods:\n";
                text += "}\n\n";
            }
            return;
        }

        [Command("ha2", true)]
        public async Task DebugPrintModules(string name = "")
        {
            string txt = "";
            PrintModules(commandService.Modules, ref txt);

            EmbedBuilder builder = new()
            {
                Title = "all commands",
                Description = txt,
            };

            await SendEmbed(builder);
        }

        #region newHelp

        private void BuildEmbed(ref EmbedBuilder builder, string modName)
        {
            string text = "";

            if (modName == "")
            {
                foreach (var mod in commandService.Modules)
                {
                    builder.AddField(f => 
                    {
                        text = "";
                        mod.Commands.ToList().ForEach(c =>
                        {
                            text += $"{c.Name.Replace("Module", "").Replace("Empty", "")}(";

                            object? value = null;
                            if (c.Parameters.Count == 1)
                            {
                                text += c.Parameters[0].Name;
                                value = c.Parameters[0].DefaultValue;
                                text += value == null ? "" : $" = {value}";
                            }
                            else
                            {
                                foreach (var par in c.Parameters)
                                {
                                    text += par.Name;
                                    value = par.DefaultValue;

                                    if (par != c.Parameters[^1])
                                    {
                                        text += value == null ? "" : $" = {value}, ";
                                    }
                                    else
                                    {
                                        text += value == null ? "" : $" = {value}";
                                    }
                                }
                            }
                            
                            text += ")\n";
                        });

                        f.Name = mod.Name.Replace("Module", "");
                        f.Value = text;
                        f.IsInline = false;
                    });
                }
            }
            else
            {
                string name = "";
                ModuleInfo module = commandService.Modules.First((ModuleInfo mod) =>
                {
                    name = mod.Name.Replace("Module", "").ToLower();
                    return name == modName;
                });

                if (module != null)
                {
                    builder.AddField(f => 
                    {
                        module.Commands.ToList().ForEach(c => 
                        {
                            text += $"**{c.Name}**(";

                            if (c.Parameters.Count() == 1)
                            {
                                text += c.Parameters[0].Name;
                                text += c.Parameters[0].DefaultValue == null ? "" : $" = {c.Parameters[0].DefaultValue}";
                            }
                            else
                            {
                                c.Parameters.ToList().ForEach(p => 
                                {
                                    text += p.Name;

                                    if (p != c.Parameters[^1])
                                    {
                                        text += p.DefaultValue == null ? ", " : $" = {p.Name}, ";
                                    }
                                    else
                                    {
                                        text += p.DefaultValue == null ? "" : $" = {p.Name}";
                                    }
                                });
                            }

                            text += ")\n";
                        });

                        f.Name = name;
                        f.Value = text;
                        f.IsInline = true;
                    });
                }
                else
                {
                    builder.AddField(f =>
                    {
                        f.Name = "Failed to find Fodule.";
                        f.Value = $"Could not find {modName}.";
                    });
                }
            }
        }

        [Command("h", true)]
        public async Task HelpCommand(string moduleName = "")
        {
            EmbedBuilder builder = new()
            {
                Title = "Help",
            };

            BuildEmbed(ref builder, moduleName);
            
            await SendEmbed(builder);
        }

        #endregion

        #region help
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
        #endregion help
        

        private readonly CommandService commandService;
        private readonly IServiceProvider provider;
        private readonly LoggingService loggingService;
    }
}
