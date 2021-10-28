using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Fembot.Modules
{
	public class Extra : BaseCommandModule
	{
		/// <summary>
		/// Command that pings the given user 5 times over 5 seconds
		/// </summary>
		/// <param name="context">Command Context Required for executing the command</param>
		/// <param name="user"></param>
		/// <returns></returns>
		[Command("sping")]
		public async Task SpamPing(CommandContext context, [RemainingText] DiscordUser user)
		{
			await context.Message.DeleteAsync();
			for (int i = 0; i < 5; i++)
			{
				await context.Channel.SendMessageAsync(user.Mention);
				await Task.Delay(1000);
			}
		}
	}
}