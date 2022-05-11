using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlayerSwapper.Common.Configs;
using PlayerSwapper.Common.Systems;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;

namespace PlayerSwapper.Content.UI;

public class PlayerSwapperGUI : UIPanel
{
    public bool IsGUIOpen { get; private set; } = false;
    private const float width = 360f, height = 306f;
    private UIGrid _uiGrid;
    private UIScrollbar _scrollbar;
    public PlayerSwapperGUI() : base()
    {
        Width.Set(width, 0f);
        Height.Set(height, 0f);
        Left.Set(Main.screenWidth / 2 - width / 2, 0f);
        Top.Set(20f, 0f);
        SetPadding(0f);
        BorderColor = Color.Transparent;
        BackgroundColor = Color.Transparent;
        
        _scrollbar = new UIScrollbar();
        _scrollbar.HAlign = 0.995f;
        _scrollbar.Top.Set(10f, 0f);
        _scrollbar.Height.Set(Height.Pixels - 20f, 0f);
        Append(_scrollbar);

        _uiGrid = new UIGrid();
        _uiGrid.ListPadding = 1f;
        _uiGrid.Width.Set(width - 20f, 0f);
        _uiGrid.Height.Set(Height.Pixels, 0f);
        _uiGrid.SetPadding(2f);
        _uiGrid.SetScrollbar(_scrollbar);
        Append(_uiGrid);
    }

    private void GenerateUICharacters()
    {
        _uiGrid.Clear();
        RemoveChild(_scrollbar);

        float top = 0f;
        for (int i = 0; i < Main.PlayerList.Count; i++)
        {
            PlayerFileData data = Main.PlayerList[i];
            if (data == Main.ActivePlayerFileData) continue;
            if (!ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty &&
                !PSPlayer.PlayerMatchesWorldDifficulty(data.Player)) continue;

            UICharacterListItemModified listItem = new UICharacterListItemModified(Main.PlayerList[i], i);
            listItem.Width.Set(width-20f, 0f);
            listItem.Top.Set(top,0f);
            listItem.SetPadding(0f);
            _uiGrid.Add(listItem);

            top += 100f;
        }
        
        if (_uiGrid._items.Count > 3)
            Append(_scrollbar);
        
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

    public void ToggleGUI()
    {
        UISystem uiSystem = ModContent.GetInstance<UISystem>();
        uiSystem.userInterface.SetState(uiSystem.userInterface.CurrentState == uiSystem.uiState ? null : uiSystem.uiState);
        IsGUIOpen = uiSystem.userInterface.CurrentState == uiSystem.uiState;
        
        if (IsGUIOpen)
            RefreshGUI();
    }

    public void RefreshGUI() => GenerateUICharacters();
}