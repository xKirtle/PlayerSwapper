using Terraria;
using Terraria.ModLoader;

namespace PlayerSwapper
{
	public class PlayerSwapper : Mod
	{
		internal static ModKeybind nextPlayer;
		internal static ModKeybind previousPlayer;
		internal static ModKeybind toggleGUI;
		
		public override void Load()
		{
			nextPlayer = KeybindLoader.RegisterKeybind(this, "NextPlayer", "I");
			previousPlayer = KeybindLoader.RegisterKeybind(this, "PreviousPlayer", "O");
			toggleGUI = KeybindLoader.RegisterKeybind(this, "ToggleMenu", "F");
		}
	}
}