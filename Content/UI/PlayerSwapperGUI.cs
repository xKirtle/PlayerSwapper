using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader.UI.Elements;

namespace PlayerSwapper.Content.UI;

public class PlayerSwapperGUI : UIPanel
{
    private const float width = 360f, height = 306f;
    private UIGrid _uiGrid;
    public PlayerSwapperGUI() : base()
    {
        Width.Set(width, 0f);
        Height.Set(height, 0f);
        Left.Set(Main.screenWidth / 2 - width / 2, 0f);
        Top.Set(20f, 0f);
        SetPadding(0f);
        BorderColor = Color.Transparent;
        BackgroundColor = Color.RoyalBlue;

        //TODO: Add logic to hide scrollbar if not necessary
        UIScrollbar scrollbar = new UIScrollbar();
        scrollbar.HAlign = 0.995f;
        scrollbar.Top.Set(10f, 0f);
        scrollbar.Height.Set(Height.Pixels - 20f, 0f);
        Append(scrollbar);

        _uiGrid = new UIGrid();
        _uiGrid.ListPadding = 1f;
        _uiGrid.Width.Set(width - 20f, 0f);
        _uiGrid.Height.Set(Height.Pixels, 0f);
        _uiGrid.SetPadding(2f);
        _uiGrid.SetScrollbar(scrollbar);
        Append(_uiGrid);
    }

    private void GenerateUICharacters()
    {
        _uiGrid.Clear();

        float top = 0f;
        for (int i = 0; i < Main.PlayerList.Count; i++)
        {
            if (Main.PlayerList[i] == Main.ActivePlayerFileData) continue;

            UICharacterListItemModified listItem = new UICharacterListItemModified(Main.PlayerList[i], i);
            listItem.Width.Set(width-20f, 0f);
            listItem.Top.Set(top,0f);
            listItem.SetPadding(0f);
            _uiGrid.Add(listItem);

            top += 100f;
        }
        
        _uiGrid.UpdateOrder();
    }

    public override void Update(GameTime gameTime)
    {
        Left.Set(Main.screenWidth / 2 - width / 2, 0f);
        Top.Set(20f, 0f);

        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
            PlayerInput.LockVanillaMouseScroll("ModLoader/UIGrid");
        }
    }

    public void RefreshGUI()
    {
        GenerateUICharacters();
    }
}