using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace GameEngine
{
    public class InvalidObjectException : Exception
    {
        public InvalidObjectException(string message)
           : base(message)
        {}
    }


    public partial class EngineClass
    {
        public Graphics GraphicsSettings;
        public float CurrentFPS;
        public int CurrentMS;
        public debugType debug = debugType.Debug;
        public bool showFps = false;
        public int FPS = 30;
        public List<Keys> PressedKeys = new List<Keys>();
        public Color DefaultColour = Color.FromArgb(200, 0, 255);
        public int FPSTick = 100;
        public int GameResolutionWidth = 640;
        public int GameResolutionHeight = 360;
        public int GameCanvasWidth = 640;
        public int GameCanvasHeight = 360;
        public int WindowWidth = 1280;
        public int WindowHeight = 720;
        public BufferedGraphics BufferedGFX;


        private BufferedGraphicsContext context;
        private Form window;
        private bool running;
        private Thread GameThread;
        private Thread FpsThread;
        private List<GameObject> GameObjects = new List<GameObject>();
        private List<GameObject> UIObjects = new List<GameObject>();
        private Color backgroundColour = Color.Black;
        private bool HasError = false;
        private Stopwatch Calculate_FPS_StopWatch = new Stopwatch();
        private TimeSpan timeSpanFPS;
        private bool resizing = true;
        private bool isFirstFrame = true;
        private GameObject FPSText;
        public int CameraX;
        public int CameraY;



        public EngineClass(Form win)
        {
            window = win;
            //GameResolutionWidth = window.Width;
            //GameResolutionHeight = window.Height;
            GraphicsSettings = win.CreateGraphics();

            

            //Console.WriteLine(win.Width);
            WindowWidth = win.Width;
            WindowHeight = win.Height;
            GameCanvasWidth = win.Width;
            GameCanvasHeight = win.Height;

            resizeGameCanvas(WindowWidth, WindowHeight);

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(WindowWidth, WindowHeight);
            BufferedGFX = context.Allocate(window.CreateGraphics(),
                 new Rectangle(0, 0, WindowWidth, WindowHeight));
            Rescale();
        }

        public int GetCanvasWidth()
        {
            return GameResolutionWidth;
        }

        public int GetCanvasHeight()
        {
            return WindowHeight;
        }

        public virtual void GameLogic()
        {
            if (FPSText != null)
            {
                TextComponent FPSTEXTComp = (TextComponent)FPSText.GetComponent("TextComponent");
                FPSTEXTComp.SetText("FPS: " + CurrentFPS + ", MS: " + CurrentMS);
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Tick();
            }

            //GameResolutionWidth = Camera.x;
            //GameResolutionHeight = Camera.y;
        }

        public void FpsTick()
        {
            while (window.Visible & !HasError)
            {
                try
                {
                    CurrentMS = timeSpanFPS.Milliseconds;
                    CurrentFPS = 1000 / timeSpanFPS.Milliseconds; // Set FPS
                    string elapsedTime = String.Format("{0}ms, {1}FPS", timeSpanFPS.Milliseconds, CurrentFPS);
                    if (showFps) { PrintText(debugType.Debug, elapsedTime); }
                }
                catch(DivideByZeroException)
                { }
                
                Thread.Sleep(FPSTick);
            }
            
        }
    }
}