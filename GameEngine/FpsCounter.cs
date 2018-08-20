using System;
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
                FPSText = new FPSCounterObject("FPSText", this);

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

    public class FPSCounterObject : GameObject
    {
        TextComponent FPSTEXTComp;

        public FPSCounterObject(string name, EngineClass engine) : base(name, engine)
        {
            FPSTEXTComp = new TextComponent(engine, this, 0, 0, Color.White, "Text", new FontFamily("Arial"), 12, FontStyle.Regular);
            AddComponent(FPSTEXTComp);
        }

        public override void Tick()
        {
            FPSTEXTComp.SetText("FPS: " + Engine.CurrentFPS + ", MS: " + Engine.CurrentMS);
        }
    }
}
