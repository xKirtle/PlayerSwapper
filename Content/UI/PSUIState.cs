using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI;

namespace PlayerSwapper.Content.UI;

public class PSUIState : UIState
{
    public static PSUIState Instance;
    public PlayerSwapperGUI gui;

    public PSUIState() : base()
    {
        Instance = this;
        gui = new PlayerSwapperGUI();
        Append(gui);
    }

    public void Unload()
    {
        Instance = null;
        gui = null;
    }

    public override void Update(GameTime gameTime)
    {
        gui.Update(gameTime);
    }
}