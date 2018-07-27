using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    partial class EngineClass
    {
        public void resizeGameCanvas(int Width, int Height)
        {
            if (debug == debugType.Debug) { PrintText(debugType.Debug, "resizing game canvas to " + Width + "x" + Height); }
            WindowWidth = Width;
            WindowHeight = Height;

            resizing = true;
        }
    }
}
