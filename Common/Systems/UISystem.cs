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
    private GameTime lastUpdateUiGameTime;
    internal static UserInterface userInterface;
    internal static PSUIState uiState;

    public override void Load()
    {
        if (!Main.dedServ && Main.netMode != NetmodeID.Server)
        {
            uiState = new PSUIState();
            userInterface = new UserInterface();
            // userInterface.SetState(uiState);
        }
    }

    public override void Unload()
    {
        uiState.Unload();
        uiState = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        lastUpdateUiGameTime = gameTime;
        if (userInterface?.CurrentState != null)
            userInterface.Update(gameTime);
    }
    
    //TODO: Check which layer it should insert on
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
        if (interfaceLayer != -1)
        {
            layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer("Player Swapper: Logic 1",
                delegate
                {
                    if (lastUpdateUiGameTime != null && userInterface?.CurrentState != null)
                        userInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}