using System.Drawing;
namespace GameEngine
{
    partial class EngineClass
    {
        public void SetBackgroundColour(int R, int G, int B)
        {
            Color Colour = Color.FromArgb(R, G, B);
            backgroundColour = Colour;
        }

        public void SetBackgroundColour(int R, int G, int B, int A)
        {
            Color Colour = Color.FromArgb(A, R, G, B);
            backgroundColour = Colour;
        }

        public void SetBackgroundColour(Color Colour)
        {
            backgroundColour = Colour;
        }

        public Color GetBackgroundColour()
        {
            return backgroundColour;
        }
    }
}
