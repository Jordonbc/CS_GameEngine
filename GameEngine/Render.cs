using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEngine
{
    partial class EngineClass
    {
        public void Render()
        {
            if (isFirstFrame)
            {
                resizeGameCanvas(WindowWidth, WindowHeight);
                isFirstFrame = false;
            }

            if (BufferedGFX == null || resizing)
            {
                if (BufferedGFX != null) BufferedGFX.Dispose();

                if (debug == debugType.Warning) { PrintText(debugType.Warning, "'BufferedGFX' is NULL!"); ; }
                context = BufferedGraphicsManager.Current;
                context.MaximumBuffer = new Size(WindowWidth, WindowHeight);

                BufferedGFX = context.Allocate(window.CreateGraphics(),
                     new Rectangle(0, 0, WindowWidth, WindowHeight)); // CREATE BUFFER

                if (debug == debugType.Debug) { PrintText(debugType.Warning, "'BufferedGFX' = " + BufferedGFX.ToString()); }
                //Console.WriteLine("RRR");

                Rescale();
            }
            RenderAll();
        }
        private void Rescale()
        {
            BufferedGFX.Graphics.ScaleTransform((float)WindowWidth / GameResolutionWidth, (float)WindowHeight / GameResolutionHeight);
            resizing = false;
        }
        private void RenderBackground()
        {
            if (debug == debugType.Debug) { PrintText(debugType.Debug, "setting canvas colour to " + backgroundColour.Name); }
            BufferedGFX.Graphics.Clear(backgroundColour);
        }
        private void RenderObjects()
        {
            // START RENDER CODE

            if (debug == debugType.Debug) { PrintText(debugType.Debug, "Drawing objects to internal buffer"); }
            if (GameObjects.Count > 0)
            {
                foreach (GameObject gameObj in GameObjects)
                {
                    if (debug == debugType.Debug) { PrintText(debugType.Warning, "Drawing object: " + gameObj.GetType().Name); }
                    //SolidBrush brush = new SolidBrush(gameObj.colour);
                    gameObj.BufferedGFX = BufferedGFX;
                    gameObj.Render();
                }

            }

            // END RENDER CODE
        }
        private void RenderUI()
        {
            // START RENDER CODE

            if (debug == debugType.Debug) { PrintText(debugType.Debug, "Drawing UI to internal buffer"); }
            if (GameObjects.Count > 0)
            {
                foreach (GameObject UIObj in UIObjects)
                {
                    if (debug == debugType.Debug) { PrintText(debugType.Warning, "Drawing object: " + UIObj.GetType().Name); }
                    //SolidBrush brush = new SolidBrush(gameObj.colour);
                    UIObj.BufferedGFX = BufferedGFX;
                    UIObj.Render();
                }

            }

            // END RENDER CODE
            
        }
        private void RenderAll(bool SetNeedsToRender = true)
        {
            BufferedGFX.Graphics.SmoothingMode = GraphicsSettings.SmoothingMode;
            BufferedGFX.Graphics.InterpolationMode = GraphicsSettings.InterpolationMode;

            RenderBackground();
            RenderObjects();
            RenderUI();

            if (debug == debugType.Debug) { PrintText(debugType.Debug, "rendering buffer to game window"); }
            BufferedGFX.Render();
        }
    }
}
