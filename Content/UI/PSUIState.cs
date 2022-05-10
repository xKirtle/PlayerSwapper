using Microsoft.Xna.Framework;
using Terraria.UI;

namespace PlayerSwapper.Content.UI;

public class PSUIState : UIState
{
    public static PSUIState Instance;
    public PlayerSwapperGUI gui;
    public override void OnInitialize()
    {
        Instance = this;
        gui = new PlayerSwapperGUI();
        Append(gui);
    }

    public void Unload()
    {
        gui = null;
    }

    public override void Update(GameTime gameTime)
    {
        // gui.Update(gameTime);
    }
}