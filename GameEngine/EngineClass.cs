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
    public class InvalidObjectException : Exception
    {
        public InvalidObjectException(string message)
           : base(message)
        {
        }
    }


    class EngineClass
    {

        public BufferedGraphics BufferedGFX;
        private BufferedGraphicsContext context;
        private Form window;
        private bool running;
        private Thread GameThread;
        private Thread FpsThread;
        private List<GameObject> GameObjects = new List<GameObject>();
        private Color backgroundColour = Color.Black;
        private bool HasError = false;
        private int GameWindowWidth = 640;
        private int GameWindowHeight = 360;

        private int NewGameWindowWidth = 640;
        private int NewGameWindowHeight = 360;

        private Stopwatch Calculate_FPS_StopWatch = new Stopwatch();
        private TimeSpan timeSpanFPS;
        private bool resizing = true;
        private bool isFirstFrame = true;
        private GameObject FPSText;

        public Graphics GraphicsSettings;

        public enum debugType
        {
            Debug,
            Info,
            Warning,
            Error
        };

        public float CurrentFPS;
        public debugType debug = debugType.Debug;
        public bool showFps = false;
        public int FPS = 30;
        public bool lockFPS = false;
        public List<Keys> PressedKeys = new List<Keys>();
        public Color DefaultColour = Color.FromArgb(200, 0, 255);
        public int FPSTick = 100;




        public EngineClass(Form win)
        {
            window = win;
            GameWindowWidth = window.Width;
            GameWindowHeight = window.Height;
            GraphicsSettings = win.CreateGraphics();

            AddFPSCounter();
            
        }

        public void AddFPSCounter()
        {
            if(FPSText == null)
            {
                //          ADDS A FPS COUNTER
                FPSText = new GameObject("FPSText");
                TextComponent tc = new TextComponent(this, FPSText, 0, 0, Color.White, "FPS TEXT", new FontFamily("Arial"), 12, FontStyle.Bold);
                FPSText.AddComponent(tc);
                FPSText.x = 1;
                FPSText.y = 1;

                CreateObject(FPSText);
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
            if (debug == debugType.Debug) { printText(debugType.Debug, "resizing game canvas to " + Width + "x" + Height); }
            NewGameWindowWidth = Width;
            NewGameWindowHeight = Height;

            resizing = true;
        }

        public void DrawBox(int x, int y, int width, int height, Color colour)
        {
            SolidBrush brush = new SolidBrush(colour);
            BufferedGFX.Graphics.FillRectangle(brush, x, y, width, height);
        }

        public void render()
        {
            if (isFirstFrame)
            {
                resizeGameCanvas(GameWindowWidth, GameWindowHeight);
                isFirstFrame = false;
            }


            if (BufferedGFX == null || resizing)
            {   
                if (BufferedGFX != null) BufferedGFX.Dispose();

                if (debug == debugType.Warning) { printText(EngineClass.debugType.Warning, "'BufferedGFX' is NULL!"); ; }
                context = BufferedGraphicsManager.Current;
                context.MaximumBuffer = new Size(NewGameWindowWidth, NewGameWindowHeight);

                BufferedGFX = context.Allocate(window.CreateGraphics(),
                     new Rectangle(0, 0, NewGameWindowWidth, NewGameWindowHeight)); // CREATE BUFFER
                if (debug == debugType.Debug) { printText(EngineClass.debugType.Warning, "'BufferedGFX' = " + BufferedGFX.ToString());}
                BufferedGFX.Graphics.ScaleTransform((float)NewGameWindowWidth/GameWindowWidth, (float)NewGameWindowHeight/GameWindowHeight);
                resizing = false;
            }

            BufferedGFX.Graphics.SmoothingMode = GraphicsSettings.SmoothingMode;
            BufferedGFX.Graphics.InterpolationMode = GraphicsSettings.InterpolationMode;

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

            if (debug == debugType.Debug) { printText(debugType.Debug, "setting canvas colour to " + backgroundColour.Name); }
            BufferedGFX.Graphics.Clear(backgroundColour);
            // START RENDER CODE

            if (debug == debugType.Debug) { printText(debugType.Debug, "Drawing objects to internal buffer"); }
            if (GameObjects.Count > 0)
            {
                foreach (GameObject gameObj in GameObjects)
                {
                    if (debug == debugType.Debug) { printText(EngineClass.debugType.Warning, "Drawing object: " + gameObj.GetType().Name); }
                    //SolidBrush brush = new SolidBrush(gameObj.colour);
                    gameObj.BufferedGFX = BufferedGFX;
                    gameObj.Render();
                    //BufferedGFX.Graphics.FillRectangle(brush, gameObj.x, gameObj.y, gameObj.SizeX, gameObj.SizeY);
                }
                
            }

            //BufferedGFX.Graphics.DrawString("test", new Font(new FontFamily("Arial"), 12f, FontStyle.Bold), new SolidBrush(Color.White), GameWindowWidth / 2, GameWindowHeight / 2);
            // END RENDER CODE
            if (debug == debugType.Debug) { printText(debugType.Debug, "rendering buffer to game window"); }
            BufferedGFX.Render();
        }
        
        public int GetCanvasWidth()
        {
            return GameWindowWidth;
        }

        public int GetCanvasHeight()
        {
            return NewGameWindowHeight;
        }

        //public void SetObjectColourByID(int ID, int R, int G, int B, int A)
        //{
        //    if (ID < GameObjects.Count)
        //    {
        //        GameObject Go = GameObjects[ID];
        //        Color Colour = Color.FromArgb(A, R, G, B);
        //        SetObjectByID(ID, Go);
        //    }
        //    else
        //    {
        //        throw new InvalidObjectException("Cannot Find Specified Game Object '"+ ID.ToString() +"'");
        //    }

        //}

        //public GameObject SetObjectColour(GameObject Object, int R, int G, int B, int A)
        //{
        //    GameObject Obj = Object;
        //    Color Colour = Color.FromArgb(A, R, G, B);
        //    Obj.colour = Colour;

        //    return Obj;
        //}

        //public void SetObjectColourByName(string Name, int R, int G, int B, int A)
        //{
        //    SetObjectColourByID(GetObjectIDByName(Name), R, G, B, A);
        //    printText(debugType.Debug, "Setting Object Colour");
        //}

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

        public GameObject GetObject(int index)
        {
            GameObject GameObj = GameObjects[index];
            return GameObj;
        }

        public GameObject GetObjectByName(String Name)
        {
            return GameObjects[GetObjectIDByName(Name)];
        }

        public GameObject GetObjectByID(int ID)
        {
            if (ID < GameObjects.Count)
            {
                return GameObjects[ID];
            }
            else
            {
                printText(debugType.Error, "Cannot find specified object '" + ID.ToString() + "'");
                return GameObjects[0];
            }
        }
        public int GetObjectIDByName(string Name)
        {
            bool found = false;
            int GameObjID = 0;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                //Console.WriteLine("OBJECT: '" + GameObjects[i].Name + "', ID: " + i.ToString());
                if (GameObjects[i].Name == Name)
                {
                    if (debug == debugType.Debug) { printText(EngineClass.debugType.Debug, "Object Found!"); }
                    found = true;
                    GameObjID = i;
                    break;
                }
            }
            if (found)
            {
                return GameObjID;
            }
            else
            {
                throw new InvalidObjectException("Cannot Find Specified Game Object '" + Name + "'");
            }
        }

        public void SetObject(int index, GameObject gameObj)
        {
            GameObjects[index] = gameObj;
        }

        public void SetObjectByName(string Name, GameObject gameObj)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == Name)
                {
                    GameObjects[i] = gameObj;
                    break;
                }
            }
            //GameObjects[index] = gameObj;
        }

        //public void AddComponentToObjectByID(ObjectComponent component, int ID)
        //{
        //    GameObjects[ID].Components.Add(component);
        //}

        //public void AddComponentToObjectByName(ObjectComponent component, string Name)
        //{

        //    GameObjects[GetObjectIDByName(Name)].Components.Add(component);
        //}

        //public void SetObjectByID(int ID, GameObject Object)
        //{
        //    GameObject GameObj;
        //    GameObj.name = Object.name;
        //    GameObj.index = Object.index;
        //    GameObj.x = Object.x;
        //    GameObj.y = Object.y;
        //    GameObj.SizeX = Object.SizeX;
        //    GameObj.SizeY = Object.SizeY;
        //    GameObj.colour = Object.colour;
        //    GameObj.Components = Object.Components;

        //    GameObjects[ID] = GameObj;
        //}
        //public GameObject CreateObject(GameObject Object)
        //{

        //    GameObject GameObj;
        //    GameObj.name = Object.name;
        //    GameObj.index = GameObjects.Count;
        //    GameObj.x = Object.x;
        //    GameObj.y = Object.y;
        //    GameObj.SizeX = Object.SizeX;
        //    GameObj.SizeY = Object.SizeY;
        //    GameObj.colour = Object.colour;
        //    GameObj.Components = new List<ObjectComponent>();

        //    GameObjects.Add(GameObj);

        //    return GameObj;
        //}

        public GameObject CreateObject(GameObject GameObj)
        {
            GameObjects.Add(GameObj);
            return GameObj;
        }

        public void DestroyObjectByName(string Name)
        {
            GameObjects.RemoveAt(GetObjectIDByName(Name));
        }

        public void DestroyObjectByID(int ID)
        {
            GameObjects.RemoveAt(ID);
        }

        public virtual Boolean GameLogic()
        {
            return true;
        }

        private void GameLoop()
        {

            while (window.Visible & !HasError)
            {
                Calculate_FPS_StopWatch.Start();
                try
                {
                    if (FPSText != null)
                    {
                        TextComponent FPSTEXTComp = (TextComponent)FPSText.GetComponent("TextComponent");
                        FPSTEXTComp.SetText("FPS: " + CurrentFPS);
                    }

                    // Only update the frame if it has successfully executed game logic
                    if (GameLogic())
                    {
                        render();
                    }

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
            }
        }

        public void FpsTick()
        {
            while (window.Visible & !HasError)
            {
                try
                {
                    CurrentFPS = 1000 / timeSpanFPS.Milliseconds; // Set FPS
                    string elapsedTime = String.Format("{0}ms, {1}FPS", timeSpanFPS.Milliseconds, 1000 / timeSpanFPS.Milliseconds);
                    if (showFps) { printText(debugType.Debug, elapsedTime); }
                }
                catch(DivideByZeroException)
                { }
                
                Thread.Sleep(FPSTick);
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
            //Thread.CurrentThread.Join();
        }
    }
}