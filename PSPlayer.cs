using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PlayerSwapper.Common.Configs;
using PlayerSwapper.Content.UI;
using Terraria;
using Terraria.DataStructures;
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
            int nextPlayerIndex = GetNextPlayerIndex();
            if (IsValidPlayerIndex(nextPlayerIndex))
                SwapPlayer(Main.PlayerList[nextPlayerIndex]);
        }
        
        if (PlayerSwapper.previousPlayer.JustPressed)
        {
            int previousPlayerIndex = GetPreviousPlayerIndex();
            if (IsValidPlayerIndex(previousPlayerIndex))
                SwapPlayer(Main.PlayerList[previousPlayerIndex]);
        }
    }

    private static bool IsValidPlayerIndex(int index) => index >= 0 && index < Main.PlayerList.Count;
    
    private static int GetCurrentPlayerIndex() => Main.PlayerList.IndexOf(Main.PlayerList.FirstOrDefault(x => x.Player == Main.LocalPlayer));

    private static int GetNextPlayerIndex() {

        int curPlayerIndex = GetCurrentPlayerIndex();
        if (curPlayerIndex == -1) return -1;
        
        if (ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty)
            return (curPlayerIndex + 1 + Main.PlayerList.Count) % Main.PlayerList.Count;

        var relevantCharactersIndex = Main.GameMode == GameModeID.Creative ? journeyCharactersIndex : classicCharactersIndex;
        
        return relevantCharactersIndex[^1] <= curPlayerIndex ?
            relevantCharactersIndex[0] :
            relevantCharactersIndex.First(x => x > curPlayerIndex);
    }
    
    private static int GetPreviousPlayerIndex() {

        int curPlayerIndex = GetCurrentPlayerIndex();
        if (curPlayerIndex == -1) return -1;
        
        if (ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty)
            return (curPlayerIndex - 1 + Main.PlayerList.Count) % Main.PlayerList.Count;

        var relevantCharactersIndex = Main.GameMode == GameModeID.Creative ? journeyCharactersIndex : classicCharactersIndex;
        
        return relevantCharactersIndex[0] >= curPlayerIndex ?
            relevantCharactersIndex[^1] :
            relevantCharactersIndex.Last(x => x > curPlayerIndex);
    }
    
    public static void SwapPlayer(PlayerFileData data)
    {
        var oldPos = Main.LocalPlayer.position;
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

    public override void OnEnterWorld() => PSUIState.Instance.gui.RefreshGUI();

    public override void Unload()
    {
        journeyCharactersIndex = null;
        classicCharactersIndex = null;
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (PSUIState.Instance?.gui?.IsGUIOpen == true)
        {
            drawInfo.colorArmorHead = Color.White;
            drawInfo.colorArmorBody = Color.White;
            drawInfo.colorArmorLegs = Color.White;
        }
    }
}