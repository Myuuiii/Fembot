using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Fembot.Modules
{
	public class Roster : BaseCommandModule
	{
		/// <summary>
		/// Command that shows the current roster 
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <returns></returns>
		[Command("roster")]
		[Description("Displays the current roster.")]
		public async Task ViewRoster(CommandContext ctx)
		{
			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.WithTitle("The current roster");
			embed.Color = DiscordColor.Purple;
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Bot._config.Scrims.Count; i++)
			{
				sb.Append($"{i + 1} - {Bot._config.Scrims[i]}\n");
			}
			embed.WithDescription(sb.ToString());
			embed.WithThumbnail("https://github.com/Myuuiii/Fembot/raw/master/doc/ico.png");

			await ctx.RespondAsync(embed: embed.Build());
		}

		/// <summary>
		/// Command that adds a new scrim to the current roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <param name="name">The name of the enemy team</param>
		/// <returns></returns>
		[Command("addroster")]
		[Description("Adds a new scrim to the roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task AddRoster(CommandContext ctx, [RemainingText, Description("The enemy team's name")] string name)
		{
			Bot._config.Scrims.Add(name);
			Bot._config.Save();

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = $"Added {name} to the scrim roster";
			embed.Color = DiscordColor.PhthaloGreen;

			await ctx.RespondAsync(embed: embed.Build());
		}

		/// <summary>
		/// Command that removes a scrim from the current roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <param name="index">Index of the game to remove</param>
		/// <returns></returns>
		[Command("removeroster")]
		[Description("Removes a scrim from the current roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task RemoveScrim(CommandContext ctx, [Description("Index of the game to remove")] int index)
		{
			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			if (index - 1 < 0 || index - 1 >= Bot._config.Scrims.Count)
			{
				embed.Title = $"Invalid Index";
				embed.Color = DiscordColor.Red;

				await ctx.RespondAsync(embed: embed.Build());
				return;
			}
			else
			{
				embed.Title = $"Removed {Bot._config.Scrims[index - 1]} from the roster";
				embed.Color = DiscordColor.IndianRed;

				Bot._config.Scrims.RemoveAt(index - 1);
				Bot._config.Save();

				await ctx.RespondAsync(embed: embed.Build());
			}
		}

		/// <summary>
		/// Command that overwrites the current roster with a new roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <param name="name">New formatted roster</param>
		/// <returns></returns>
		[Command("setroster")]
		[Description("Sets the current roster to the specified roster. Requires `Administrator` permissions in this server. Set every roster item on a new line (shift+enter) and only leave the command on the first line")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task SetRoster(CommandContext ctx, [RemainingText, Description("The new roster")] string roster)
		{
			string[] lines = roster.Split('\n');
			Bot._config.Scrims.Clear();
			for (int i = 0; i < lines.Length; i++)
			{
				Bot._config.Scrims.Add(lines[i]);
			}
			Bot._config.Save();

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = $"Set the current roster";
			embed.Color = DiscordColor.Chartreuse;
			await ctx.RespondAsync(embed: embed.Build());
		}

		/// <summary>
		/// Command that clears the current roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <returns></returns>
		[Command("clearroster")]
		[Description("Clears the roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task ClearRoster(CommandContext ctx)
		{
			Bot._config.Scrims.Clear();
			Bot._config.Save();

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = $"Cleared the roster";
			embed.Color = DiscordColor.IndianRed;
			await ctx.RespondAsync(embed: embed.Build());
		}
	}
}