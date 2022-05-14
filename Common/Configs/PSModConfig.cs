using System.ComponentModel;
using PlayerSwapper.Content.UI;
using Terraria.ModLoader.Config;

namespace PlayerSwapper.Common.Configs;

public class PSModConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Label("Swap to character regardless of the world difficulty")]
    [Tooltip("If enabled, characters created for Journey Mode will be able to play on Non-Journey Mode worlds and vice versa")]
    [DefaultValue(true)]
    public bool CanSwapRegardlessOfDifficulty;

    public override void OnChanged() => PSUIState.Instance?.gui.RefreshGUI();
}