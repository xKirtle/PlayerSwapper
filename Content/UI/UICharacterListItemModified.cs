using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.Elements;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;

namespace PlayerSwapper.Content.UI;

public class UICharacterListItemModified : UIPanel
{
    private PlayerFileData _data;
    private Asset<Texture2D> _dividerTexture;
    private Asset<Texture2D> _innerPanelTexture;
    private Asset<Texture2D> _innerPanelTexture2;
    private UICharacter _playerPanel;
    private UIText _buttonLabel;
    private Asset<Texture2D> _buttonCloudActiveTexture;
    private Asset<Texture2D> _buttonCloudInactiveTexture;
    private Asset<Texture2D> _buttonFavoriteActiveTexture;
    private Asset<Texture2D> _buttonFavoriteInactiveTexture;
    private Asset<Texture2D> _buttonPlayTexture;
    private ulong _fileSize;
    
    public bool IsFavorite => _data.IsFavorite;

    public UICharacterListItemModified(PlayerFileData data, int snapPointIndex)
    {
        BorderColor = new Color(89, 116, 213) * 0.7f;
        _dividerTexture = ModContent.Request<Texture2D>("PlayerSwapper/Assets/UI/Divider");
        _innerPanelTexture = ModContent.Request<Texture2D>("PlayerSwapper/Assets/UI/InnerPanelBackground");
        _innerPanelTexture2 = ModContent.Request<Texture2D>("PlayerSwapper/Assets/UI/InnerPanelBackground2");
        _buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive");
        _buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive");
        _buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive");
        _buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive");
        _buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
        _fileSize = (ulong)FileUtilities.GetFileSize(data.Path, data.IsCloudSave);
        Height.Set(100f, 0f);
        Width.Set(0f, 1f);
        SetPadding(6f);

        _data = data;
        
        _playerPanel = new UICharacter(data.Player);
        _playerPanel.Left.Set(12f, 0f);
        _playerPanel.VAlign = 0.2f;
        _playerPanel.OnDoubleClick += (__, _) => { ModContent.GetInstance<PlayerSwapper>().SwapPlayer(data); };
        base.OnDoubleClick += (__, _) => { ModContent.GetInstance<PlayerSwapper>().SwapPlayer(data); };
        Append(_playerPanel);
        
        UIImageButton buttonPlay = new UIImageButton(_buttonPlayTexture);
        buttonPlay.VAlign = .91f;
        buttonPlay.Left.Set(8f, 0f);
        buttonPlay.OnClick += (__, _) => { ModContent.GetInstance<PlayerSwapper>().SwapPlayer(data); };
        buttonPlay.OnMouseOver += PlayMouseOver;
        buttonPlay.OnMouseOut += ButtonMouseOut;
        buttonPlay.SetSnapPoint("Play", snapPointIndex);
        Append(buttonPlay);
        
        UIImageButton buttonFavorite = new UIImageButton(data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
        buttonFavorite.VAlign = .91f;
        buttonFavorite.Left.Set(32f, 0f);
        buttonFavorite.OnClick += FavoriteButtonClick;
        buttonFavorite.OnMouseOver += FavoriteMouseOver;
        buttonFavorite.OnMouseOut += ButtonMouseOut;
        buttonFavorite.SetVisibility(1f, data.IsFavorite ? 0.8f : 0.4f);
        buttonFavorite.SetSnapPoint("Favorite", snapPointIndex);
        Append(buttonFavorite);
        
        if (SocialAPI.Cloud != null) 
        {
            UIImageButton buttonCloud = new UIImageButton(data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
            buttonCloud.VAlign = .91f;
            buttonCloud.Left.Set(56f, 0f);
            buttonCloud.OnMouseOut += ButtonMouseOut;
            buttonCloud.SetVisibility(0.4f, 0.4f);
            Append(buttonCloud);
            buttonCloud.SetSnapPoint("Cloud", snapPointIndex);
        }
        
        _buttonLabel = new UIText("");
        _buttonLabel.VAlign = .91f;
        _buttonLabel.Left.Set(84f, 0f);
        _buttonLabel.Top.Set(-3f, 0f);
        Append(_buttonLabel);

        UIImage textBackground = new UIImage(_innerPanelTexture);
        textBackground.Left.Set(74f, 0f);
        textBackground.Top.Set(35f, 0f);
        textBackground.SetPadding(0f);
        Append(textBackground);
        
        string text = "";
        Color color = Color.White;
        switch (data.Player.difficulty) 
        {
	        case 0:
		        text = Language.GetTextValue("UI.Softcore");
		        break;
	        case 1:
		        text = Language.GetTextValue("UI.Mediumcore");
		        color = Main.mcColor;
		        break;
	        case 2:
		        text = Language.GetTextValue("UI.Hardcore");
		        color = Main.hcColor;
		        break;
	        case 3:
		        text = Language.GetTextValue("UI.Creative");
		        color = Main.creativeModeColor;
		        break;
        }

        UIText uiText = new UIText(text);
        uiText.TextColor = color;
        uiText.Left.Set(70f - FontAssets.MouseText.Value.MeasureString(text).X * 0.5f, 0f);
        uiText.Top.Set(5f, 0f);
        textBackground.Append(uiText);

        UIImage playTimeBackground = new UIImage(_innerPanelTexture2);
		playTimeBackground.Left.Set(219f, 0f);
		playTimeBackground.Top.Set(35f, 0f);
		playTimeBackground.SetPadding(0f);
		Append(playTimeBackground);
		
		TimeSpan playTime = data.GetPlayTime();
		int num2 = playTime.Days * 24 + playTime.Hours;
		string text2 = ((num2 < 10) ? "0" : "") + num2 + playTime.ToString("\\:mm\\:ss");
		
		UIText uiText2 = new UIText(text2);
		uiText2.Left.Set(55f - FontAssets.MouseText.Value.MeasureString(text2).X * 0.5f, 0f);
		uiText2.Top.Set(5f, 0f);
		playTimeBackground.Append(uiText2);

		UIImage divider = new UIImage(_dividerTexture);
		divider.Left.Set(_playerPanel.Left.Pixels + _playerPanel.Width.Pixels, 0f);
		divider.Top.Set(25f, 0f);
		Append(divider);

		UIText name = new UIText(data.Name);
		name.Left.Set(75f, 0f);
		name.Top.Set(7f, 0f);
		Append(name);
    }

    private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement) 
    {
        _buttonLabel.SetText(Language.GetTextValue("UI.Play"));
    }
    
    private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement) 
    {
        _buttonLabel.SetText("");
    }
    
    private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement) 
    {
	    _data.ToggleFavorite();
	    UIImageButton buttonFavorite = (UIImageButton) evt.Target;
	    buttonFavorite.SetImage(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
	    buttonFavorite.SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
	    
	    _buttonLabel.SetText(Language.GetTextValue("UI." + (_data.IsFavorite ? "Unfavorite" : "Favorite")));
	    (base.Parent.Parent as UIGrid)?.UpdateOrder();
    }
    
    private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement) 
    { 
	    _buttonLabel.SetText(Language.GetTextValue("UI." + (_data.IsFavorite ? "Unfavorite" : "Favorite")));
    }

    public override int CompareTo(object obj) 
    {
	    UICharacterListItemModified uICharacterListItem = obj as UICharacterListItemModified;
	    if (uICharacterListItem != null) 
	    {
		    if (IsFavorite && !uICharacterListItem.IsFavorite)
			    return -1;

		    if (!IsFavorite && uICharacterListItem.IsFavorite)
			    return 1;

		    if (_data.Name.CompareTo(uICharacterListItem._data.Name) != 0)
			    return _data.Name.CompareTo(uICharacterListItem._data.Name);

		    return _data.GetFileName().CompareTo(uICharacterListItem._data.GetFileName());
	    }

	    return base.CompareTo(obj);
    }
    
    public override void MouseOver(UIMouseEvent evt) 
    {
        base.MouseOver(evt);
        BackgroundColor = new Color(73, 94, 171) * 0.75f;
        BorderColor = new Color(89, 116, 213) * 0.75f;
        _playerPanel.SetAnimated(true);
    }

    public override void MouseOut(UIMouseEvent evt) 
    {
        base.MouseOut(evt);
        BackgroundColor = new Color(63, 82, 151) * 0.7f;
        BorderColor = new Color(89, 116, 213) * 0.7f;
        _playerPanel.SetAnimated(false);
    }
}