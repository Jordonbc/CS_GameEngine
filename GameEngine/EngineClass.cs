using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GameEngine;
using System.Diagnostics;

namespace GameEngine
{

    public struct Vector2D
    {
        public int x, y;
    }

    public struct GameObject
    {
        public int index;
        public float x, y, SizeX, SizeY;
        public Color colour;
    }


    class EngineClass
    {

        private BufferedGraphics BufferedGFX;
        private BufferedGraphicsContext context;
        private Form window;
        private bool running;
        private Thread GameThread;
        private Thread FpsThread;
        private List<GameObject> GameObjects = new List<GameObject>();
        private Color backgroundColour = Color.Black;
        private bool HasError = false;

        private Stopwatch Calculate_FPS_StopWatch = new Stopwatch();
        private TimeSpan timeSpanFPS;

        public enum debugType
        {
            Debug,
            Info,
            Warning,
            Error
        };

        public float CurrentFPS;
        public bool debug = false;
        public bool showFps = true;
        public int FPS = 30;
        public bool lockFPS = false;

        private int GameWindowWidth = 640;
        private int GameWindowHeight = 360;


        public EngineClass(Form win)
        {
            window = win;
            GameWindowWidth = window.Width;
            GameWindowHeight = window.Height;
        }

        public void printText(debugType USERdebugType, string str)
        {
            ConsoleColor old = Console.BackgroundColor;
            switch (USERdebugType)
            {
                case debugType.Error:
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\n IT LOOKS LIKE WE HAVE DETECTED AN ERROR! \n\n");

                        break;
                    }
                case debugType.Warning:
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        break;
                    }
                case debugType.Info:
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    }
                case debugType.Debug:
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        break;
                    }
                default:
                    {
                        Console.BackgroundColor = old;
                        break;
                    }
            }
            Console.WriteLine(USERdebugType.ToString()+":"+" "+str);
            Console.BackgroundColor = old;
        }
        public void resizeGameCanvas(int Width, int Height)
        {
            if (debug) { printText(debugType.Debug, "resizing game canvas to " + Width + "x" + Height); }
            GameWindowWidth = Width;
            GameWindowHeight = Height;

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(GameWindowWidth, GameWindowHeight);

            BufferedGFX = context.Allocate(window.CreateGraphics(),
                     new Rectangle(0, 0, GameWindowWidth, GameWindowHeight)); // CREATE BUFFER

            //if (debug) { printText(debugType.Debug, "scaling canvas to " + window.Width + "x" + window.Height); }
            //g.Graphics.ScaleTransform(window.Width, window.Height);
        }

        public void render()
        {
            if (BufferedGFX == null)
            {
                if (debug) { printText(EngineClass.debugType.Warning, "'BufferedGFX' is NULL!"); ; }
                context = BufferedGraphicsManager.Current;
                context.MaximumBuffer = new Size(GameWindowWidth, GameWindowHeight);

                BufferedGFX = context.Allocate(window.CreateGraphics(),
                     new Rectangle(0, 0, GameWindowWidth, GameWindowHeight)); // CREATE BUFFER
                if (debug) { printText(EngineClass.debugType.Warning, "'BufferedGFX' = " + BufferedGFX.ToString()); ; }
            }

            if (lockFPS)
            {
                if (CurrentFPS > 30 && CurrentFPS < 60)
                {
                    FPS = 30;
                }
                else
                {
                    FPS = 60;
                }
            }

            if (debug) { printText(debugType.Debug, "setting canvas colour to " + backgroundColour.Name); }
            BufferedGFX.Graphics.Clear(backgroundColour);
            // START RENDER CODE

            if (debug) { printText(debugType.Debug, "Drawing objects to internal buffer"); }
            if (GameObjects.Count > 0)
            {
                foreach (GameObject gameObj in GameObjects)
                {
                    SolidBrush brush = new SolidBrush(gameObj.colour);

                    BufferedGFX.Graphics.FillRectangle(brush, gameObj.x, gameObj.y, gameObj.SizeX, gameObj.SizeY);
                }
                
            }

            // END RENDER CODE
            if (debug) { printText(debugType.Debug, "rendering buffer to game window"); }
            BufferedGFX.Render();
        }

        public Color getBackgroundColour()
        {
            return backgroundColour;
        }
        public void setBackgroundColour(Color colour)
        {
            backgroundColour = colour;
        }

        public GameObject GetGraphics(int index)
        {
            GameObject GameObj = GameObjects[index];
            return GameObj;
        }


        public void SetObject(int index, GameObject gameObj)
        {
            GameObjects[index] = gameObj;
        }

        public void SetGraphics(int index, int x, int y, int sizeX, int sizeY, Color colour)
        {
            GameObject GameObj;
            GameObj.index = index;
            GameObj.x = x;
            GameObj.y = y;
            GameObj.SizeX = sizeX;
            GameObj.SizeY = sizeY;
            GameObj.colour = colour;

            GameObjects[index] = GameObj;
        }

        public GameObject AddGraphics(int x, int y, int sizeX, int sizeY, Color colour)
        {

            GameObject GameObj;
            GameObj.index = GameObjects.Count;
            GameObj.x = x;
            GameObj.y = y;
            GameObj.SizeX = sizeX;
            GameObj.SizeY = sizeY;
            GameObj.colour = colour;

            GameObjects.Add(GameObj);

            return GameObj;
        }

        private void GameLoop()
        {

            while (window.Visible & !HasError)
            {
                Calculate_FPS_StopWatch.Start();
                try
                {
                    render();
                    if (1000 / FPS >= 0)
                    { Thread.Sleep(1000 / FPS); }
                }
                catch(Exception e)
                {
                    HasError = true;
                    printText(debugType.Error, e.Message + "\n\n Stacktrace: \n\n" + e.StackTrace);
                }

                Calculate_FPS_StopWatch.Stop();
                timeSpanFPS = Calculate_FPS_StopWatch.Elapsed;
                Calculate_FPS_StopWatch.Reset();

                CurrentFPS = 1000 / timeSpanFPS.Milliseconds;
            }
        }

        public void FpsTick()
        {
            while (window.Visible & !HasError)
            {
                try
                {
                    string elapsedTime = String.Format("{0}ms, {1}FPS", timeSpanFPS.Milliseconds, 1000 / timeSpanFPS.Milliseconds);
                    if (showFps) { printText(debugType.Debug, elapsedTime); }
                }
                catch(DivideByZeroException)
                { }
                
                Thread.Sleep(100);
            }
            
        }

        public bool startGame()
        {
            if (!running)
            {
                running = true;
                GameThread = new Thread(GameLoop);
                GameThread.Start();
                FpsThread = new Thread(FpsTick);
                FpsThread.Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void stopGame()
        {
            if (running)
            {
                running = false;
                try
                {
                    GameThread.Join();
                    FpsThread.Join();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message + "\n\n Stacktrace: \n\n" + error.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
            }
        }
    }
}