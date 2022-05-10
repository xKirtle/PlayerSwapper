using Terraria;
using Terraria.ModLoader;
using WorldGen = On.Terraria.WorldGen;

namespace PlayerSwapper
{
	public class PlayerSwapper : Mod
	{
		internal static ModKeybind nextPlayer;
		internal static ModKeybind previousPlayer;
		internal static ModKeybind toggleGUI;
		
		public override void Load()
		{
			nextPlayer = KeybindLoader.RegisterKeybind(this, "Next Player", "I");
			previousPlayer = KeybindLoader.RegisterKeybind(this, "Previous Player", "O");
			toggleGUI = KeybindLoader.RegisterKeybind(this, "Toggle GUI", "F");
		}
	}
}