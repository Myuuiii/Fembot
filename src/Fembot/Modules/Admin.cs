using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Fembot.Modules
{
	public class Admin : BaseCommandModule
	{
		/// <summary>
		/// Command to change the prefix for the bot
		/// </summary>
		/// <param name="ctx">Command Context required for executing this command</param>
		/// <param name="prefix">The new prefix for the bot</param>
		/// <returns></returns>
		[Command("prefix")]
		[Description("Changes the prefix of the bot. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task Prefix(CommandContext ctx, [RemainingText, Description("The new prefix.")] string prefix)
		{
			Bot._config.Prefix = prefix;
			Bot._config.Save();

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = "Prefix changed";
			embed.Color = DiscordColor.Azure;
			embed.WithDescription($"The prefix has been changed to `{prefix}`");
			await ctx.RespondAsync(embed: embed);

			await ctx.Client.UpdateStatusAsync(new DiscordActivity(prefix + "help", ActivityType.ListeningTo));
		}

		/// <summary>
		/// Sets the role that will be pinged when the 'scrim' trigger is used
		/// </summary>
		/// <param name="ctx">Command Context required for executing this command</param>
		/// <param name="role">The new scrim role</param>
		/// <returns></returns>
		[Command("setscrimrole")]
		[Description("Sets the scrim role for the server. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task SetScrimRole(CommandContext ctx, [RemainingText, Description("The role to set as the scrim role.")] DiscordRole role)
		{
			Bot._config.ScrimRoleId = role.Id;
			Bot._config.Save();

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = "Scrim role set";
			embed.Color = DiscordColor.Azure;
			embed.WithDescription($"The scrim role has been set to `{role.Name}`");
			await ctx.RespondAsync(embed: embed);
		}

		/// <summary>
		/// Sets the role that is granted/revoked to/from a user when using the mute/unmute commands
		/// </summary>
		/// <param name="ctx">Command Context required for executing this command</param>
		/// <param name="role">The new muted role</param>
		/// <returns></returns>
		[Command("setmutedrole")]
		[Description("Sets the muted role for the server. Requires `Administrator` permissions in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
		public async Task SetMutedRole(CommandContext ctx, [RemainingText, Description("The role to set as the muted role.")] DiscordRole role)
		{
			Bot._config.MutedRoleId = role.Id;
			Bot._config.Save();

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = "Muted role set";
			embed.Color = DiscordColor.Azure;
			embed.WithDescription($"The muted role has been set to `{role.Name}`");
			await ctx.RespondAsync(embed: embed);
		}

		/// <summary>
		/// Command that bans the given user from the server
		/// </summary>
		/// <param name="ctx">Command Context required for executing this command</param>
		/// <param name="user">The user to ban from the server</param>
		/// <param name="reason">Optional reason of why the user was banned</param>
		/// <returns></returns>
		[Command("ban")]
		[Description("Bans a user from the server. Requires `Ban Members` permission in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.BanMembers)]
		public async Task Ban(CommandContext ctx, [Description("The user to ban.")] DiscordUser user, [RemainingText, Description("The reason for the ban.")] string reason = "No reason provided")
		{
			await (user as DiscordMember).BanAsync(0, reason);

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = "User banned";
			embed.Color = DiscordColor.Red;
			embed.WithDescription($"{user.Mention} has been banned from the server.\nReason: {reason}");
			await ctx.RespondAsync(embed: embed);
		}

		/// <summary>
		/// Command that kicks the given user from the server
		/// </summary>
		/// <param name="ctx">Command Context required for executing this command</param>
		/// <param name="user">The user to kick from the server</param>
		/// <param name="reason">Optional reason of why the user was kicked</param>
		/// <returns></returns>
		[Command("kick")]
		[Description("Kicks a user from the server. Requires `Kick Members` permission in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.KickMembers)]
		public async Task Kick(CommandContext ctx, [Description("The user to kick.")] DiscordUser user, [RemainingText, Description("The reason for the kick.")] string reason = "No reason provided")
		{
			await (user as DiscordMember).RemoveAsync(reason);

			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			embed.Title = "User kicked";
			embed.Color = DiscordColor.Yellow;
			embed.WithDescription($"{user.Mention} has been kicked from the server.\nReason: {reason}");
			await ctx.RespondAsync(embed: embed);
		}

		/// <summary>
		/// Command that mutes a user
		/// </summary>
		/// <param name="ctx">Command Context required for executing this command</param>
		/// <param name="user">The user to mute</param>
		/// <returns></returns>
		[Command("mute")]
		[Description("Mutes a user from the server. Requires the `Mute Members` permission in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.MuteMembers)]
		public async Task Mute(CommandContext ctx, [RemainingText, Description("The user to mute.")] DiscordUser user)
		{
			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			if (ctx.Guild.Roles.Any(r => r.Value.Id == Bot._config.MutedRoleId))
			{
				if (!(user as DiscordMember).Roles.Any(r => r.Id == Bot._config.MutedRoleId))
				{
					await (user as DiscordMember).GrantRoleAsync(ctx.Guild.Roles.First(r => r.Value.Id == Bot._config.MutedRoleId).Value);

					embed.Title = "User muted";
					embed.Color = DiscordColor.Grayple;
					embed.WithDescription($"{user.Mention} has been muted.");
					await ctx.RespondAsync(embed: embed);
				}
				else
				{
					embed.Title = "User already muted";
					embed.WithDescription($"{user.Mention} is already muted.");
					await ctx.RespondAsync(embed: embed);
				}
			}
			else
			{
				embed.Title = "Muted role not set";
				embed.WithDescription($"The muted role has not been set. Please use `{Bot._config.Prefix}setmutedrole` to set the muted role.");
				await ctx.RespondAsync(embed: embed);
			}
		}

		/// <summary>
		/// Command that unmutes a user
		/// </summary>
		/// <param name="ctx">Command Context required for executing the command</param>
		/// <param name="user">The user to unmute</param>
		/// <returns></returns>
		[Command("unmute")]
		[Description("Unmutes a user from the server. Requires the `Mute Members` permission in this server")]
		[RequireUserPermissions(DSharpPlus.Permissions.MuteMembers)]
		public async Task Unmute(CommandContext ctx, [RemainingText, Description("The user to unmute.")] DiscordUser user)
		{
			DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
			if (ctx.Guild.Roles.Any(r => r.Value.Id == Bot._config.MutedRoleId))
			{
				if ((user as DiscordMember).Roles.Any(r => r.Id == Bot._config.MutedRoleId))
				{
					await (user as DiscordMember).RevokeRoleAsync(ctx.Guild.Roles.First(r => r.Value.Id == Bot._config.MutedRoleId).Value);

					embed.Title = "User unmuted";
					embed.Color = DiscordColor.Grayple;
					embed.WithDescription($"{user.Mention} has been unmuted.");
					await ctx.RespondAsync(embed: embed);
				}
				else
				{
					embed.Title = "User is not muted";
					embed.WithDescription($"{user.Mention} is is not muted.");
					await ctx.RespondAsync(embed: embed);
				}
			}
			else
			{
				embed.Title = "Muted role not set";
				embed.WithDescription($"The muted role has not been set. Please use `{Bot._config.Prefix}setmutedrole` to set the muted role.");
			}
		}
	}
}