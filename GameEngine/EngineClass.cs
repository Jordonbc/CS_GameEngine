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
    public struct ObjectComponent
    {
        public string name;
        public int index;
    }

    public abstract class Component
    {
        // Constructor
        private GameObjectV2 parentObject;
        public Component()
        {

        }

        public abstract void Render();

    }

    class Box : Component
    {
        private int x;
        private int y;
        private Color colour;
        private int width;
        private int height;
        private int _x;

        public Box(int x, int y, Color colour, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.colour = colour;
            this.width = width;
            this.height = height;
        }


        // GETTERS AND SETTERS
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public Color Colour { get => colour; set => colour = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public override void Render()
        {
            SolidBrush brush = new SolidBrush(colour);
            //BufferedGFX.Graphics.FillRectangle(brush, x, y, width, height);
        }
    }


    public abstract class GameObjectV2
    {
        private int x = 0;
        private int y = 0;
        private List<Component> Components = new List<Component>();

        // Constructor
        public GameObjectV2()
        {

        }

        public void Render()
        {
            // Render all components
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Render();
            }
        }
        public void SetX(int newX)
        {
            x = newX;
        }
        public void SetY(int newY)
        {
            y = newY;
        }
        public void AddComponent(Component c)
        {
            Components.Add(c);
        }
    }

    public class Player : GameObjectV2
    {
        public Player() : base()
        {
            Box b = new Box(0,0, Color.Blue, 10, 10);
            AddComponent(b);
        }
    }

    public struct GameObject
    {
        public string name;
        public int index;
        public float x, y, SizeX, SizeY;
        public Color colour;
        public List<ObjectComponent> Components;
    }


    public class InvalidObjectException : Exception
    {
        public InvalidObjectException(string message)
           : base(message)
        {
        }
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
        private int GameWindowWidth = 640;
        private int GameWindowHeight = 360;

        private int NewGameWindowWidth = 640;
        private int NewGameWindowHeight = 360;

        private Stopwatch Calculate_FPS_StopWatch = new Stopwatch();
        private TimeSpan timeSpanFPS;
        private bool resizing = true;
        private bool isFirstFrame = true;

        public enum debugType
        {
            Debug,
            Info,
            Warning,
            Error
        };

        public float CurrentFPS;
        public debugType debug = debugType.Debug;
        public bool showFps = true;
        public int FPS = 30;
        public bool lockFPS = false;
        public List<Keys> PressedKeys = new List<Keys>();
        public Color DefaultColour = Color.FromArgb(200, 0, 255);


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
            if (debug == debugType.Debug) { printText(debugType.Debug, "resizing game canvas to " + Width + "x" + Height); }
            NewGameWindowWidth = Width;
            NewGameWindowHeight = Height;

            resizing = true;
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
                if (debug == debugType.Warning) { printText(EngineClass.debugType.Warning, "'BufferedGFX' is NULL!"); ; }
                context = BufferedGraphicsManager.Current;
                context.MaximumBuffer = new Size(NewGameWindowWidth, NewGameWindowHeight);

                BufferedGFX = context.Allocate(window.CreateGraphics(),
                     new Rectangle(0, 0, NewGameWindowWidth, NewGameWindowHeight)); // CREATE BUFFER
                if (debug == debugType.Debug) { printText(EngineClass.debugType.Warning, "'BufferedGFX' = " + BufferedGFX.ToString());}
                BufferedGFX.Graphics.ScaleTransform((float)NewGameWindowWidth/GameWindowWidth, (float)NewGameWindowHeight/GameWindowHeight);
                resizing = false;
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

            if (debug == debugType.Debug) { printText(debugType.Debug, "setting canvas colour to " + backgroundColour.Name); }
            BufferedGFX.Graphics.Clear(backgroundColour);
            // START RENDER CODE

            if (debug == debugType.Debug) { printText(debugType.Debug, "Drawing objects to internal buffer"); }
            if (GameObjects.Count > 0)
            {
                foreach (GameObject gameObj in GameObjects)
                {
                    SolidBrush brush = new SolidBrush(gameObj.colour);

                    BufferedGFX.Graphics.FillRectangle(brush, gameObj.x, gameObj.y, gameObj.SizeX, gameObj.SizeY);
                }
                
            }

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

        public void SetObjectColourByID(int ID, int R, int G, int B, int A)
        {
            if (ID < GameObjects.Count)
            {
                GameObject Go = GameObjects[ID];
                Color Colour = Color.FromArgb(A, R, G, B);
                SetObjectByID(ID, Go);
            }
            else
            {
                throw new InvalidObjectException("Cannot Find Specified Game Object '"+ ID.ToString() +"'");
            }

        }

        public GameObject SetObjectColour(GameObject Object, int R, int G, int B, int A)
        {
            GameObject Obj = Object;
            Color Colour = Color.FromArgb(A, R, G, B);
            Obj.colour = Colour;

            return Obj;
        }

        public void SetObjectColourByName(string Name, int R, int G, int B, int A)
        {
            SetObjectColourByID(GetObjectIDByName(Name), R, G, B, A);
            printText(debugType.Debug, "Setting Object Colour");
        }

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
                if (GameObjects[i].name == Name)
                {
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
                if (GameObjects[i].name == Name)
                {
                    GameObjects[i] = gameObj;
                    break;
                }
            }
            //GameObjects[index] = gameObj;
        }

        public void AddComponentToObjectByID(ObjectComponent component, int ID)
        {
            GameObjects[ID].Components.Add(component);
        }

        public void AddComponentToObjectByName(ObjectComponent component, string Name)
        {
            
            GameObjects[GetObjectIDByName(Name)].Components.Add(component);
        }

        public void SetObjectByID(int ID, GameObject Object)
        {
            GameObject GameObj;
            GameObj.name = Object.name;
            GameObj.index = Object.index;
            GameObj.x = Object.x;
            GameObj.y = Object.y;
            GameObj.SizeX = Object.SizeX;
            GameObj.SizeY = Object.SizeY;
            GameObj.colour = Object.colour;
            GameObj.Components = Object.Components;

            GameObjects[ID] = GameObj;
        }
        public GameObject CreateObject(GameObject Object)
        {

            GameObject GameObj;
            GameObj.name = Object.name;
            GameObj.index = GameObjects.Count;
            GameObj.x = Object.x;
            GameObj.y = Object.y;
            GameObj.SizeX = Object.SizeX;
            GameObj.SizeY = Object.SizeY;
            GameObj.colour = Object.colour;
            GameObj.Components = new List<ObjectComponent>();

            GameObjects.Add(GameObj);

            return GameObj;
        }

        public GameObject CreateObject(int x, int y, int sizeX, int sizeY, Color? colour = null, string name = "Default Object Name")
        {
            if(colour == null)
            {
                colour = DefaultColour;
            }
            GameObject GameObj;
            GameObj.name = name;
            GameObj.index = GameObjects.Count;
            GameObj.x = x;
            GameObj.y = y;
            GameObj.SizeX = sizeX;
            GameObj.SizeY = sizeY;
            GameObj.colour = (Color)colour;
            GameObj.Components = new List<ObjectComponent>();

            GameObjects.Add(GameObj);

            return GameObj;
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
            //Thread.CurrentThread.Join();
        }
    }
}