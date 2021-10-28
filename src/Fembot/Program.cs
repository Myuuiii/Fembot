using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

namespace Fembot
{
	class Bot
	{
		public static DiscordClient _client;
		public static FembotConfiguration _config;
		public static CommandsNextExtension _commands;

		public static void Main(string[] args) => Bot.MainAsync().GetAwaiter().GetResult();

		private static async Task MainAsync()
		{
			if (!File.Exists("config.json"))
			{
				Console.WriteLine("Config file not found, creating one.");
				File.WriteAllText("config.json", JsonConvert.SerializeObject(new FembotConfiguration(), Formatting.Indented));
				Console.WriteLine("Please edit the config file and restart the bot.");
				return;
			}
			else
			{
				_config = JsonConvert.DeserializeObject<FembotConfiguration>(File.ReadAllText("config.json"));
			}

			_client = new DiscordClient(new DiscordConfiguration()
			{
				Token = _config.Token,
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.All
			});

			_commands = _client.UseCommandsNext(new CommandsNextConfiguration()
			{
				UseDefaultCommandHandler = false
			});

			_commands.RegisterCommands<Modules.Admin>();
			_commands.RegisterCommands<Modules.Roster>();
			_commands.RegisterCommands<Modules.Extra>();

			await _client.ConnectAsync();

			_client.MessageCreated += clientMessageReceived;
			_client.Ready += clientReady;

			await Task.Delay(-1);
		}

		private static async Task clientReady(DiscordClient sender, ReadyEventArgs e)
		{
			await _client.UpdateStatusAsync(new DiscordActivity($"{_config.Prefix}help", ActivityType.ListeningTo), UserStatus.Online);
		}

		private static async Task clientMessageReceived(DiscordClient sender, MessageCreateEventArgs e)
		{
			if (e.Author.IsBot) return;

			switch (e.Message.Content.ToLower())
			{
				case "uwu":
				case "owo":
					await e.Channel.SendMessageAsync(e.Message.Content);
					break;
				case "scrim":
					if (_config.ScrimRoleId != 0) { await e.Channel.SendMessageAsync($"<@&{_config.ScrimRoleId}>"); }
					else { await e.Channel.SendMessageAsync("Scrim role not set."); }
					break;
			}

			#region Command handler
			CommandsNextExtension cnext = sender.GetCommandsNext();
			DiscordMessage msg = e.Message;

			int cmdStart = msg.GetStringPrefixLength(_config.Prefix);
			if (cmdStart != -1)
			{
				string prefix = msg.Content.Substring(0, cmdStart);
				string cmdString = msg.Content.Substring(cmdStart);

				Command command = cnext.FindCommand(cmdString, out var args);
				CommandContext ctx = cnext.CreateContext(msg, prefix, command, args);
				new Thread(async () => await cnext.ExecuteCommandAsync(ctx)).Start();
			}
			#endregion
		}
	}
}

