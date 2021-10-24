using System.IO;
using Newtonsoft.Json;

namespace Fembot
{
	public class FembotConfiguration
	{
		public string Token { get; set; } = "";
		public string Prefix { get; set; } = "";
		public ulong ScrimRoleId { get; set; } = 0;
		public ulong MutedRoleId { get; set; } = 0;

		public void Save()
		{
			File.WriteAllText("./config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
		}
	}
}