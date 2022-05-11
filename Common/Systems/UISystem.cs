using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PlayerSwapper.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace PlayerSwapper.Common.Systems;

public class UISystem : ModSystem
{
    private GameTime _lastUpdateUiGameTime;
    internal UserInterface userInterface;
    internal PSUIState uiState;

    public override void Load()
    {
        if (!Main.dedServ && Main.netMode != NetmodeID.Server)
        {
            uiState = new PSUIState();
            userInterface = new UserInterface();
        }
    }

    public override void Unload()
    {
        uiState.Unload();
        uiState = null;
        userInterface = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _lastUpdateUiGameTime = gameTime;
        if (userInterface?.CurrentState != null)
            userInterface.Update(gameTime);
    }
    
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
        if (interfaceLayer != -1)
        {
            layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer("Player Swapper: Cursor",
                delegate
                {
                    if (_lastUpdateUiGameTime != null && userInterface?.CurrentState != null)
                        userInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}