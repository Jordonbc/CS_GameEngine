using System.Drawing;

namespace GameEngine
{
    partial class EngineClass
    {
        public void AddFPSCounter()
        {
            if (FPSText == null)
            {
                //          ADDS A FPS COUNTER
                FPSText = new GameObject(this, "FPSText");
                TextComponent tc = new TextComponent(this, FPSText, 0, 0, Color.White, "FPS TEXT", new FontFamily("Arial"), 12, FontStyle.Bold);
                FPSText.AddComponent(tc);
                FPSText.SetX(1);
                FPSText.SetY(1);

                CreateUIObject(FPSText);
            }
        }

        public void RemoveFPSCounter()
        {
            if (FPSText != null)
            {
                DestroyObjectByName(FPSText.Name);
                FPSText = null;
            }
        }
    }
}
