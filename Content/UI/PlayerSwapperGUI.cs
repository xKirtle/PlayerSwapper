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

public class PlayerSwapperGUI : DraggableUIPanel
{
    public bool IsGUIOpen { get; private set; } = false;
    private const float width = 364f, height = 326f;
    private UIGrid _uiGrid;
    private UIScrollbar _scrollbar;
    public List<Player> drawnPlayers = new List<Player>();
    public PlayerSwapperGUI() : base(ModContent.Request<Texture2D>("PlayerSwapper/Assets/UI/Background"))
    {
        Width.Set(width, 0f);
        Height.Set(height, 0f);
        Left.Set(Main.screenWidth / 2 - width / 2, 0f);
        Top.Set(20f, 0f);
        SetPadding(0f);

        OnLeftMouseDown += (element, listener) =>
        {
            Vector2 MenuPosition = new Vector2(Left.Pixels, Top.Pixels);
            Vector2 clickPos = Vector2.Subtract(element.MousePosition, MenuPosition);
            canDrag = clickPos.X <= width - 24f && clickPos.Y <= 24f;
        };

        OnLeftMouseUp += (__, _) => canDrag = false;

        UIImage closeCross = new UIImage(ModContent.Request<Texture2D>("PlayerSwapper/Assets/UI/CloseCross"));
        closeCross.Width.Set(19f, 0f);
        closeCross.Height.Set(19f, 0f);
        closeCross.VAlign = 0.01f;
        closeCross.HAlign = 0.99f;
        closeCross.OnLeftMouseDown += (__, _) => { ToggleGUI(); };
        Append(closeCross);

        _scrollbar = new UIScrollbar();
        _scrollbar.HAlign = 0.994f;
        _scrollbar.Top.Set(34f, 0f);
        _scrollbar.Height.Set(Height.Pixels - 40f, 0f);
        Append(_scrollbar);

        _uiGrid = new UIGrid();
        _uiGrid.ListPadding = 1f;
        _uiGrid.Left.Set(2f, 0f);
        _uiGrid.Top.Set(24f, 0f);
        _uiGrid.Width.Set(width - 22f, 0f);
        _uiGrid.Height.Set(Height.Pixels - 20f, 0f);
        _uiGrid.SetPadding(2f);
        _uiGrid.SetScrollbar(_scrollbar);
        Append(_uiGrid);
    }

    private void GenerateUICharacters()
    {
        PSPlayer.journeyCharactersIndex = new List<int>();
        PSPlayer.classicCharactersIndex = new List<int>();
        drawnPlayers.Clear();
        _uiGrid.Clear();

        float top = 0f;
        for (int i = 0; i < Main.PlayerList.Count; i++)
        {
            PlayerFileData data = Main.PlayerList[i];
            if (data == Main.ActivePlayerFileData) continue;
            if (!ModContent.GetInstance<PSModConfig>().CanSwapRegardlessOfDifficulty &&
                !PSPlayer.PlayerMatchesWorldDifficulty(data.Player)) continue;

            if (Main.GameMode == GameModeID.Creative)
                PSPlayer.journeyCharactersIndex.Add(i);
            else
                PSPlayer.classicCharactersIndex.Add(i);
            
            UICharacterListItemModified listItem = new UICharacterListItemModified(Main.PlayerList[i], i);
            listItem.Width.Set(width-20f, 0f);
            listItem.Top.Set(top,0f);
            listItem.SetPadding(0f);
            _uiGrid.Add(listItem);
            
            drawnPlayers.Add(listItem.player);

            top += 100f;
        }

        _uiGrid.UpdateOrder();
    }

    public override void Update(GameTime gameTime)
    {
        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
            PlayerInput.LockVanillaMouseScroll("ModLoader/UIGrid");
        }

        UpdatePosition();
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