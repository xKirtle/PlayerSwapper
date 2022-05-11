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
        if (PlayerSwapper.toggleGUI.JustPressed)
            PSUIState.Instance.gui.ToggleGUI();

        if (PlayerSwapper.nextPlayer.JustPressed)
        {
            int curPlayerIndex = Main.PlayerList.IndexOf(Main.PlayerList.First(x => x.Player == Main.LocalPlayer));
            if (curPlayerIndex + 1 < Main.PlayerList.Count)
                ModContent.GetInstance<PlayerSwapper>().SwapPlayer(Main.PlayerList[curPlayerIndex + 1]);
        }
        
        if (PlayerSwapper.previousPlayer.JustPressed)
        {
            int curPlayerIndex = Main.PlayerList.IndexOf(Main.PlayerList.First(x => x.Player == Main.LocalPlayer));
            if (curPlayerIndex - 1 >= 0)
                ModContent.GetInstance<PlayerSwapper>().SwapPlayer(Main.PlayerList[curPlayerIndex - 1]);
        }
    }
}