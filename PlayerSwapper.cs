using Microsoft.Xna.Framework;
using PlayerSwapper.Content.UI;
using Terraria;
using Terraria.IO;
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
			nextPlayer = KeybindLoader.RegisterKeybind(this, "Next Player", "I");
			previousPlayer = KeybindLoader.RegisterKeybind(this, "Previous Player", "O");
			toggleGUI = KeybindLoader.RegisterKeybind(this, "Toggle GUI", "F");
		}

		public void SwapPlayer(PlayerFileData data)
		{
			//TODO: Add ModConfig to prevent Journey characters from joining Non-Journey worlds and vice-versa
			
			Vector2 oldPos = Main.LocalPlayer.position;
			int oldDir = Main.LocalPlayer.direction;

			//Setting values to preview's default
			Main.LocalPlayer.direction = 1;
			Main.LocalPlayer.wingFrame = 0;

			//Saves player to file without unloading world
			Main.ActivePlayerFileData.StopPlayTimer();
			Player.SavePlayer(Main.ActivePlayerFileData);
			Player.ClearPlayerTempInfo();

			//BUG: In multiplayer, the player base skin will be the same as the one the player joined with for all playerSaves

			//Load new desired player
			Main.PlayerList[Main.PlayerList.IndexOf(data)].SetAsActive();
			Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
			Main.ActivePlayerFileData.StartPlayTimer();
			Player.Hooks.EnterWorld(Main.myPlayer);

			Main.LocalPlayer.position = oldPos;
			Main.LocalPlayer.direction = oldDir;

			PSUIState.Instance.gui.RefreshGUI();
		}
	}
}