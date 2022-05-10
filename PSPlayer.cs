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
        {
            userInterface.SetState(userInterface.CurrentState == uiState ? null : uiState);
            if (userInterface.CurrentState == uiState)
                PSUIState.Instance.gui.RefreshGUI();
        }
    }
}