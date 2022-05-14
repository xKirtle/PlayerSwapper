using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PlayerSwapper.Common.Configs;
using PlayerSwapper.Content.UI;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using static PlayerSwapper.Common.Systems.UISystem;

namespace PlayerSwapper;

public class PSPlayer : ModPlayer
{
    public static List<int> journeyCharactersIndex;
    public static List<int> classicCharactersIndex;
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (PlayerSwapper.toggleGUI.JustPressed)
            PSUIState.Instance.gui.ToggleGUI();

        if (PlayerSwapper.nextPlayer.JustPressed)
        {
            int curPlayerIndex = Main.PlayerList.IndexOf(Main.PlayerList.First(x => x.Player == Main.LocalPlayer));
            int nextPlayerIndex;
            if (!ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty)
            {
                if (Main.GameMode == GameModeID.Creative)
                    nextPlayerIndex = journeyCharactersIndex.First(x => x > curPlayerIndex);
                else
                    nextPlayerIndex = classicCharactersIndex.First(x => x > curPlayerIndex);
            }
            else
                nextPlayerIndex = curPlayerIndex + 1;

            if (nextPlayerIndex >= Main.PlayerList.Count) return;
            SwapPlayer(Main.PlayerList[nextPlayerIndex]);
        }
        
        if (PlayerSwapper.previousPlayer.JustPressed)
        {
            int curPlayerIndex = Main.PlayerList.IndexOf(Main.PlayerList.First(x => x.Player == Main.LocalPlayer));
            int previousPlayerIndex;
            if (!ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty)
            {
                if (Main.GameMode == GameModeID.Creative)
                    previousPlayerIndex = journeyCharactersIndex.First(x => x < curPlayerIndex);
                else
                    previousPlayerIndex = classicCharactersIndex.First(x => x < curPlayerIndex);
            }
            else
                previousPlayerIndex = curPlayerIndex - 1;

            if (previousPlayerIndex < 0) return;
            SwapPlayer(Main.PlayerList[previousPlayerIndex]);
        }
    }
    
    public static void SwapPlayer(PlayerFileData data)
    {
        Vector2 oldPos = Main.LocalPlayer.position;
        int oldDir = Main.LocalPlayer.direction;
        int fallStart = Main.LocalPlayer.fallStart;
        int fallStart2 = Main.LocalPlayer.fallStart2;
        float wingTime = Main.LocalPlayer.wingTime;
        
        //Setting values to preview's default
        Main.LocalPlayer.direction = 1;
        Main.LocalPlayer.wingFrame = 0;

        //Saves player to file without unloading world
        Main.ActivePlayerFileData.StopPlayTimer();
        Player.SavePlayer(Main.ActivePlayerFileData);

        //BUG: In multiplayer, the player base skin will be the same as the one the player first joined with for all playerSaves

        //Load new desired player
        Main.PlayerList[Main.PlayerList.IndexOf(data)].SetAsActive();
        Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
        Main.ActivePlayerFileData.StartPlayTimer();

        Main.LocalPlayer.position = oldPos;
        Main.LocalPlayer.direction = oldDir;
        Main.LocalPlayer.fallStart = fallStart;
        Main.LocalPlayer.fallStart2 = fallStart2;
        Main.LocalPlayer.wingTime = wingTime;

        PSUIState.Instance.gui.RefreshGUI();
    }

    public static bool PlayerMatchesWorldDifficulty(Player player) =>
        (Main.GameMode != GameModeID.Creative && player.difficulty != GameModeID.Creative) ||
        (Main.GameMode == GameModeID.Creative && player.difficulty == GameModeID.Creative);

    public override void OnEnterWorld(Player player) => PSUIState.Instance.gui.RefreshGUI();

    public override void Unload()
    {
        journeyCharactersIndex = null;
        classicCharactersIndex = null;
    }
}