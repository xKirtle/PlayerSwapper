//Modified from https://github.com/tModLoader/tModLoader/blob/master/ExampleMod/UI/DragableUIPanel.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PlayerSwapper.Content.UI
{
    public class DraggableUIPanel : UIImage
    {
        private Vector2 offset;
        private bool dragging;
        public bool canDrag;

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            dragging = true;
            if (canDrag)
                offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            Vector2 end = evt.MousePosition;
            dragging = false;

            if (canDrag)
            {
                Left.Set(end.X - offset.X, 0f);
                Top.Set(end.Y - offset.Y, 0f);
                canDrag = false;
            }

            Recalculate();
        }

        public void UpdatePosition()
        {
            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;

            if (dragging && canDrag)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Terraria.Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Terraria.Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                Recalculate();
            }
        }

        public DraggableUIPanel(Asset<Texture2D> texture) : base(texture) { }
    }
}