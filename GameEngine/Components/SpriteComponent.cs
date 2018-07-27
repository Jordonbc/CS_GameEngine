using System;
using System.Drawing;

namespace GameEngine
{
    public enum HRotation
    {
        Left,
        Right
    }
    public enum VRotation
    {
        Up,
        Down
    }

    public class SpriteComponent : BaseComponent
    {
        private int x;
        private int y;
        private Rectangle rect;
        private Image Img;
        private EngineClass engine;

        VRotation VertImgRotation = VRotation.Up;
        HRotation HorImgRotation = HRotation.Right;



        public SpriteComponent(EngineClass engine,GameObject ParentObj, int x, int y, int width, int height, Image Img) : base(ParentObj)
        {
            this.x = x;
            this.y = y;
            this.engine = engine;
            this.Img = Img;
            base.width = width;
            base.height = height;
            VertImgRotation = VRotation.Up;
            HorImgRotation = HRotation.Right;
            rect = new Rectangle(base.parentObject.x, base.parentObject.y, width, height);
        }

        public void MakeTransparent()
        {
            Bitmap b = new Bitmap(Img);
            b.MakeTransparent(Color.White);
            Img = b;
        }

        public void SetImageRotation(HRotation Hrot, VRotation Vrot)
        {
            if(Vrot == VRotation.Down && VertImgRotation != VRotation.Down)
            {
                Img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                VertImgRotation = VRotation.Down;
            }

            if (Vrot == VRotation.Up && VertImgRotation != VRotation.Up)
            {
                Img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                VertImgRotation = VRotation.Up;
            }

            if (Hrot == HRotation.Left && HorImgRotation != HRotation.Left)
            {
                Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                HorImgRotation = HRotation.Left;
            }
            if (Hrot == HRotation.Right && HorImgRotation != HRotation.Right)
            {
                Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                HorImgRotation = HRotation.Right;
            }
        }

        public void resizeImage(int width, int height)
        {
            rect = new Rectangle(base.parentObject.x, base.parentObject.y, width, height);
        }
        public override void Render()
        {
            rect = new Rectangle(base.parentObject.x, base.parentObject.y, rect.Width, rect.Height);
            engine.BufferedGFX.Graphics.DrawImage(Img, rect);
            //engine.DrawBox(parentObject.x + x,parentObject.y + y, height, width, colour);
        }
    }
}
