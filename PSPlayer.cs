using System;
using System.Linq;
using Microsoft.Xna.Framework;
using PlayerSwapper.Content.UI;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static PlayerSwapper.Common.Systems.UISystem;

namespace PlayerSwapper;

public class PSPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        //Hardcoded test example
        if (PlayerSwapper.nextPlayer.JustPressed)
        {
            Main.NewText($"Previous player: {Main.LocalPlayer.name}");
            
            // Main.PlayerList[0].Player
            Main.NewText("Pressed Hotkey");
            
            //Relevant hook, Main.LoadPlayers() (loads players in the player selection menu)
            //Main.PlayerList[i].SetAsActive(); -> Points the ActivePlayerFileData to this

            Vector2 oldPos = Main.LocalPlayer.position;
            int oldDir = Main.LocalPlayer.direction;
            
            //Saves player to file without unloading world
            Main.ActivePlayerFileData.StopPlayTimer();
            Player.SavePlayer(Main.ActivePlayerFileData);
            Player.ClearPlayerTempInfo();

            //Load new desired player?
            Main.PlayerList[2].SetAsActive();
            Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
            Main.ActivePlayerFileData.StartPlayTimer();
            Player.Hooks.EnterWorld(Main.myPlayer);

            Main.LocalPlayer.position = oldPos;
            Main.LocalPlayer.direction = oldDir;
            
            Main.NewText($"Current player: {Main.LocalPlayer.name}");
        }

        if (PlayerSwapper.toggleGUI.JustPressed)
        {
            userInterface.SetState(userInterface.CurrentState == uiState ? null : uiState);
            if (userInterface.CurrentState == uiState)
                PSUIState.Instance.gui.RefreshGUI();
        }
    }
}