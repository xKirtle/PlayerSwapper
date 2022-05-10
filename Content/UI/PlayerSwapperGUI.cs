using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace PlayerSwapper.Content.UI;

public class PlayerSwapperGUI : UIPanel
{
    private const float width = 430f, height = 340f;
    public PlayerSwapperGUI() : base()
    {
        Width.Set(width, 0);
        Height.Set(height, 0);
        Left.Set(Main.screenWidth / 2 - width, 0);
        Top.Set(Main.screenHeight / 2 - height, 0);
        BorderColor = Color.Transparent;
        BackgroundColor = Color.Red;
    }

    public override void Update(GameTime gameTime)
    {
        
    }
}