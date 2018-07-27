using System.Drawing;

namespace GameEngine
{
    class TextComponent : BaseComponent
    {
        private int x;
        private int y;
        private Color colour;
        private EngineClass engine;
        private string Text = "";
        private Font font;
        private SolidBrush TextBrush;

        public TextComponent(EngineClass engine, GameObject ParentObj, int x, int y, Color colour, string Text = "Text", FontFamily fontFam = null, float TextSize = 12, FontStyle TextStyle = FontStyle.Regular) : base(ParentObj)
        {
            if (fontFam == null) fontFam = new FontFamily("Arial");
            this.x = x;
            this.y = y;
            this.colour = colour;
            this.engine = engine;
            this.Text = Text;
            font = new Font(fontFam, TextSize, TextStyle);
            TextBrush = new SolidBrush(colour);
        }

        public void SetText(string newText)
        {
            Text = newText;
        }

        // GETTERS AND SETTERS
        //public Color Colour { get => colour; set => colour = value; }

        public override void Render()
        {
            engine.BufferedGFX.Graphics.DrawString(Text, font, TextBrush, parentObject.x + x, parentObject.y + y);
            //engine.DrawBox(parentObject.x + x, parentObject.y + y, height, width, colour);
        }
    }
}
