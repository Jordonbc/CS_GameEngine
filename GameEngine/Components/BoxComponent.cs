using System;
using System.Drawing;

namespace GameEngine
{
    public class BoxComponent : BaseComponent
    {
        private EngineClass engine;


        public BoxComponent(EngineClass engine,GameObject ParentObj, int x, int y, Color colour, int width, int height) : base(ParentObj)
        {
            this.Colour = colour;
            this.engine = engine;
            setRectangle(x, y, width, height);
        }

        public BoxComponent(EngineClass engine, GameObject ParentObj, Color colour, Rectangle rectangle) : base(ParentObj)
        {
            this.Colour = colour;
            this.engine = engine;
            setRectangle(rectangle);
        }

        private void setRectangle(int x, int y, int w, int h)
        {
            rectangle = new Rectangle(parentObject.GetX() + x, parentObject.GetY() + y, w * (int)parentObject.Engine.GlobalScale[0], h * (int)parentObject.Engine.GlobalScale[1]);
        }

        private void setRectangle(Rectangle rect)
        {
            rectangle = new Rectangle(parentObject.GetX() + rect.X, parentObject.GetY() + rect.Y, rect.Width * (int)parentObject.Engine.GlobalScale[0], rect.Height * (int)parentObject.Engine.GlobalScale[1]);
        }

        // GETTERS AND SETTERS
        public Color Colour { get; set; }

        public override void Render()
        {
            //Console.WriteLine("RENDERING Box Component");
            SolidBrush brush = new SolidBrush(Colour);
            engine.BufferedGFX.Graphics.FillRectangle(brush, parentObject.GetX() + rectangle.X, parentObject.GetY() + rectangle.Y, rectangle.Width * (int)parentObject.Engine.GlobalScale[0], rectangle.Height * (int)parentObject.Engine.GlobalScale[1]);
        }

        public override void Tick()
        {

        }
    }
}
