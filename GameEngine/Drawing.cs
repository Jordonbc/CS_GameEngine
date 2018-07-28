using System.Drawing;
namespace GameEngine
{
    partial class EngineClass
    {
        public void DrawBox(int x, int y, int width, int height, Color colour)
        {
            SolidBrush brush = new SolidBrush(colour);
            BufferedGFX.Graphics.FillRectangle(brush, x, y, width, height);
        }
    }
}
