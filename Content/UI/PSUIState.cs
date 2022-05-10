using Microsoft.Xna.Framework;
using Terraria.UI;

namespace PlayerSwapper.Content.UI;

public class PSUIState : UIState
{
    public PlayerSwapperGUI gui;
    public override void OnInitialize()
    {
        gui = new PlayerSwapperGUI();
        Append(gui);
    }

    public void Unload()
    {
        gui = null;
    }

    public override void Update(GameTime gameTime)
    {
        gui.Update(gameTime);
    }
}