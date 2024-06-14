using Discord;
using Discord.Commands;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib.Modules
{
    public class HelpModule : CustomBaseModule
    {
        public HelpModule(CommandService _commandService, IServiceProvider _serviceProvider, LoggingService _loggingService)
        {
            commandService = _commandService;
            provider = _serviceProvider;
            loggingService = _loggingService;
        }

        [Command("H", true)]
        [Summary("Help Summary")]
        public async Task HelpCommand(string moduleName = "")
        {
            EmbedBuilder builder = new()
            {
                Title = $"Help {moduleName}",
                Color = Color.DarkBlue,
            };

            moduleName = moduleName.ToLower();
            string text = "";
            string modName = "";

            if (moduleName == "")
            {
                foreach (var mod in commandService.Modules)
                {
                    text = "";
                    modName = mod.Name.Replace("Module", "").Replace("Empty", "");

                    if (string.IsNullOrEmpty(modName))
                    {
                        ForeachCommand(mod, ref text);
                        builder.Description = text;
                    }
                    else
                    {
                        ForeachCommand(mod, ref text);
                        builder.AddField(f =>
                        {
                            f.Name = $"*{modName}*";
                            f.Value = string.IsNullOrWhiteSpace(text) ? "Empty == true" : text;
                            f.IsInline = false;
                        });
                    }
                }
            }
            else
            {
                string name = "";
                ModuleInfo module = commandService.Modules.First(mod =>
                {
                    name = mod.Name.Replace("Module", "").ToLower();
                    return name == moduleName;
                });

                if (module == null)
                {
                    builder.AddField(f =>
                    {
                        f.Name = "Failed to find Fodule.";
                        f.Value = $"Could not find {modName}.";
                    });
                }
                else
                {
                    ForeachCommand(module, ref text);
                    builder.AddField(f =>
                    {
                        f.Name = name;
                        f.Value = string.IsNullOrWhiteSpace(text) ? "Empty == true" : text;
                        f.IsInline = true;
                    });
                }
            }
            await SendEmbed(builder);
        }

        private void ForeachCommand(ModuleInfo mod, ref string text)
        {
            string txt = "";
            mod.Commands.ToList().ForEach(c =>
            {
                // TODO fix mod.parent.group 2 level limit
                txt += $"**{Settings.CommandPrefix}{(mod.IsSubmodule ? $"{mod.Parent.Group} " : "")}{(string.IsNullOrWhiteSpace(mod.Group) ? "" : $"{mod.Group} ")}{c.Name}**";
                if (c.Parameters.Count > 0)
                {
                    ForeachParameter(c, ref txt);
                }

                if (!string.IsNullOrWhiteSpace(c.Summary))
                {
                    txt += "\n";
                    txt += $"[{c.Summary}]";
                    txt += "\n";
                }
                else
                {
                    txt += "\n";
                }
            });
            text += txt;
        }

        private void ForeachParameter(CommandInfo c, ref string text)
        {
            object? value = null;

            text += " (";
            if (c.Parameters.Count == 1)
            {
                var par = c.Parameters[0];
                AddParameterName(ref text, par);
                value = par.DefaultValue;

                if (value != null)
                {
                    if (value is string)
                    {
                        text += $" = \"{value}\"";
                    }
                    else
                    {
                        text += $" = {value}";
                    }
                }
            }
            else
            {
                foreach (var par in c.Parameters)
                {
                    AddParameterName(ref text, par);
                    value = par.DefaultValue;
                    string isString = value is string ? "\"" : "";

                    if (par != c.Parameters[^1])
                    {
                        text += value == null ? ", " : $" = {isString}{value}{isString}, ";
                    }
                    else
                    {
                        text += value == null ? "" : $" = {isString}{value}{isString}";
                    }
                }
            }
            text += ")";

            static void AddParameterName(ref string txt, ParameterInfo par)
            {
                // edge case for clearity
                if (par.Type == typeof(string) && par.DefaultValue == null)
                {
                    txt += $"\"{par.Name}\"";
                }
                else
                {
                    txt += par.Name;
                }
            }
        }

        private readonly CommandService commandService;
        private readonly IServiceProvider provider;
        private readonly LoggingService loggingService;
    }
}
