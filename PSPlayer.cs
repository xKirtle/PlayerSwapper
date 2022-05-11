using System;
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
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (PlayerSwapper.toggleGUI.JustPressed)
            PSUIState.Instance.gui.ToggleGUI();

        if (PlayerSwapper.nextPlayer.JustPressed)
        {
            int curPlayerIndex = Main.PlayerList.IndexOf(Main.PlayerList.First(x => x.Player == Main.LocalPlayer));
            int nextPlayerIndex = curPlayerIndex + 1;
            if (!ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty)
            {
                while (nextPlayerIndex < Main.PlayerList.Count)
                {
                    PlayerFileData data = Main.PlayerList[nextPlayerIndex];
                    if (PlayerMatchesWorldDifficulty(data.Player))
                        break;

                    nextPlayerIndex++;
                }
                
                if (nextPlayerIndex >= Main.PlayerList.Count)
                    nextPlayerIndex = curPlayerIndex;
            }
            
            if (curPlayerIndex != nextPlayerIndex)
                ModContent.GetInstance<PlayerSwapper>().SwapPlayer(Main.PlayerList[nextPlayerIndex]);
        }
        
        if (PlayerSwapper.previousPlayer.JustPressed)
        {
            int curPlayerIndex = Main.PlayerList.IndexOf(Main.PlayerList.First(x => x.Player == Main.LocalPlayer));
            int previousPlayerIndex = curPlayerIndex - 1;
            if (!ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty)
            {
                while (previousPlayerIndex >= 0)
                {
                    PlayerFileData data = Main.PlayerList[previousPlayerIndex];
                    if (PlayerMatchesWorldDifficulty(data.Player))
                        break;

                    previousPlayerIndex--;
                }
                
                if (previousPlayerIndex < 0)
                    previousPlayerIndex = curPlayerIndex;
            }
            
            if (curPlayerIndex != previousPlayerIndex)
                ModContent.GetInstance<PlayerSwapper>().SwapPlayer(Main.PlayerList[previousPlayerIndex]);
        }
    }

    public static bool PlayerMatchesWorldDifficulty(Player player) =>
        (Main.GameMode != GameModeID.Creative && player.difficulty != GameModeID.Creative) ||
        (Main.GameMode == GameModeID.Creative && player.difficulty == GameModeID.Creative);

    public override void OnEnterWorld(Player player) => PSUIState.Instance.gui.RefreshGUI();
}