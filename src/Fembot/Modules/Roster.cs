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
		[Description("Displays the current roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task RosterAsync(CommandContext ctx)
		{
			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.WithTitle("The current roster");
			embed.WithColor(DiscordColor.Purple);
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Bot._config.Scrims.Count; i++)
			{
				sb.Append($"{i + 1} - {Bot._config.Scrims[i]}\n");
			}
			embed.WithDescription(sb.ToString());

			await ctx.RespondAsync("", embed.Build());
		}

		/// <summary>
		/// Command that adds a new scrim to the current roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <param name="name">The name of the enemy team</param>
		/// <returns></returns>
		[Command("addscrim")]
		[Description("Adds a player to the scrim roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task AddScrimAsync(CommandContext ctx, [RemainingText, Description("The enemy team's name")] string name)
		{
			Bot._config.Scrims.Add(name);
			Bot._config.Save();

			await ctx.RespondAsync($"Added `{name}` to the scrim roster.");
		}

		/// <summary>
		/// Command that removes a scrim from the current roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <param name="index">Index of the game to remove</param>
		/// <returns></returns>
		[Command("removescrim")]
		[Description("Removes a player from the scrim roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task RemoveScrimAsync(CommandContext ctx, [Description("Index of the game to remove")] int index)
		{
			if (index - 1 < 0 || index - 1 >= Bot._config.Scrims.Count)
			{
				await ctx.RespondAsync("Invalid index.");
				return;
			}
			else
			{
				Bot._config.Scrims.RemoveAt(index - 1);
				Bot._config.Save();

				await ctx.RespondAsync($"Removed the scrim from the roster.");
			}
		}

		/// <summary>
		/// Command that overwrites the current roster with a new roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <param name="name">New formatted roster</param>
		/// <returns></returns>
		[Command("setcurrentroster")]
		[Description("Sets the current roster to the specified roster. Requires `Administrator` permissions in this server. Set every roster item on a new line (shift+enter) and only leave the command on the first line")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task SetCurrentRosterAsync(CommandContext ctx, [RemainingText, Description("The new roster")] string roster)
		{
			string[] lines = roster.Split('\n');
			Bot._config.Scrims.Clear();
			for (int i = 0; i < lines.Length; i++)
			{
				Bot._config.Scrims.Add(lines[i]);
			}
			Bot._config.Save();
			await ctx.RespondAsync("Roster set.");
		}

		/// <summary>
		/// Command that clears the current roster
		/// </summary>
		/// <param name="ctx">Command Context Required for executing the command</param>
		/// <returns></returns>
		[Command("clearroster")]
		[Description("Clears the roster. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task ClearRosterAsync(CommandContext ctx)
		{
			Bot._config.Scrims.Clear();
			Bot._config.Save();
			await ctx.RespondAsync("Roster cleared.");
		}
	}
}