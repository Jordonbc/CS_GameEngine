using System.Drawing;

namespace GameEngine
{
    public class BoxComponent : BaseComponent
    {
        private int x;
        private int y;
        private Color colour;
        private EngineClass engine;
        

        public BoxComponent(EngineClass engine,GameObject ParentObj, int x, int y, Color colour, int width, int height) : base(ParentObj)
        {
            this.x = x;
            this.y = y;
            this.colour = colour;
            this.width = width;
            this.height = height;
            this.engine = engine;
        }


        // GETTERS AND SETTERS
        public Color Colour { get => colour; set => colour = value; }

        public override void Render()
        {
            engine.DrawBox(parentObject.GetX() + x,parentObject.GetY() + y, height, width, colour);
        }
    }
}
