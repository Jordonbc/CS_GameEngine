using GameEngine.Components;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameEngine
{
    public partial class Form1 : Form
    {
        Game localEngine;
        //GameObject player;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Test_Game.exe";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();

            Console.WriteLine("Debug Console Initialized");

            localEngine = new Game(this);
            localEngine.GameResolutionWidth = 1280;
            localEngine.GameResolutionHeight = 720;

            localEngine.AddFPSCounter();

            Player Player = new Player(localEngine, "Player");

            GameObject Floor = new GameObject(localEngine, "Floor");

            GameObject GameRenderTextObj = new GameObject(localEngine, "GameRenderTextObj");
            GameObject WindowSizeTextObj = new GameObject(localEngine, "WindowSizeTextObj");
            Sound GMusic = new Sound("Music.wav");
            Sound MClick = new Sound("laser7.wav");

            TextComponent tc = new TextComponent(localEngine, GameRenderTextObj, 1, 20, Color.White, "Resolution Width", new FontFamily("Arial"), 12);
            GameRenderTextObj.AddComponent(tc);

            TextComponent tc2 = new TextComponent(localEngine, WindowSizeTextObj, 1, 50, Color.White, "Resolution Width", new FontFamily("Arial"), 12);
            WindowSizeTextObj.AddComponent(tc2);


            localEngine.CreateUIObject(GameRenderTextObj);
            localEngine.CreateUIObject(WindowSizeTextObj);

            BoxComponent b = new BoxComponent(localEngine, Floor, 0, 0, Color.Black, 200, 200);
            Floor.AddComponent(b);
            Floor.SetX(10);
            Floor.SetY(localEngine.GameResolutionHeight - 20);

            localEngine.CreateObject(Floor);

            localEngine.PrintText(debugType.Debug, "'localEngine' Defined!");

            localEngine.PrintText(debugType.Debug, "'player' Defined!");

            localEngine.SetBackgroundColour(255/2, 255/5, 255/2);

            localEngine.GraphicsSettings.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            localEngine.GraphicsSettings.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            Player.SetX(localEngine.GameResolutionWidth / 2 - Player.width / 2);
            Player.SetY(localEngine.GameResolutionHeight / 2 - Player.height / 2);

            SpriteComponent spr = new SpriteComponent(localEngine, Player, 0, 0, 50, 50, Image.FromFile("Mario.png"));

            Player.AddComponent(spr);

            localEngine.CreateObject(Player);

            localEngine.PrintText(debugType.Debug, "Starting Engine");
            localEngine.startGame();

            GMusic.PlaySound();

            localEngine.FPS = 60;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            localEngine.PressedKeys.Add(e.KeyCode); // Add key to list of pressed keys

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            localEngine.resizeGameCanvas(this.Width, this.Height);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //MClick.PlaySound();
        }
    }

    class Player : PlayerObject
    {
        public Player(EngineClass engine, string name) : base(engine, name)
        {

        }
        public override void Tick()
        {
            base.Tick();
            //Console.WriteLine("Working!!!");
        }
    }

    class Game : EngineClass
    {
        int playerSpeed = 20;

        public Game(Form win) : base(win)
        {
            debug = debugType.Error;
            FPSTick = 50;
        }


        public override void GameLogic()
        {
            base.GameLogic();
            // This runs every frame and handles game logic

            PlayerObject player = (Player)GetObjectByName("Player");
            SpriteComponent playerSpriteComp = (SpriteComponent)player.GetComponent("SpriteComponent");

            GameObject GameResTextObj = GetUIObjectByName("GameRenderTextObj");
            TextComponent GameResTextObjComp = (TextComponent)GameResTextObj.GetComponent("TextComponent");

            GameResTextObjComp.SetText("Render Width: " + GameResolutionWidth.ToString() + ", Render Height: " + GameResolutionHeight.ToString());

            GameObject WindowSizeTextObj = GetUIObjectByName("WindowSizeTextObj");
            TextComponent WindowSizeTextObjComp = (TextComponent)WindowSizeTextObj.GetComponent("TextComponent");

            WindowSizeTextObjComp.SetText("Window Width: " + WindowWidth.ToString() + ", Render Height: " + WindowHeight.ToString());

            // Update the camera position depending on the player pos
            CameraX = player.GetX();
            CameraY = player.GetY();

            // Handle Key Presses
            if (PressedKeys.Count > 0)
            {
                for (int i = 0; i < PressedKeys.Count; i++)
                {
                    //Keys currentKeyCode = PressedKeys[i];

                    //printText(EngineClass.debugType.Debug, PressedKeys[i].ToString());

                    if (PressedKeys[i] == Keys.W || PressedKeys[i] == Keys.Up)
                    {

                        player.moveUp(playerSpeed);
                        //playerSpriteComp.SetImageRotation(Rotation.Up);
                    }
                    else if (PressedKeys[i] == Keys.S || PressedKeys[i] == Keys.Down)
                    {
                        player.moveDown(playerSpeed);
                        //playerSpriteComp.SetImageRotation(Rotation.Down);
                    }

                    if (PressedKeys[i] == Keys.A || PressedKeys[i] == Keys.Left)
                    {
                        player.moveLeft(playerSpeed);
                        playerSpriteComp.SetImageRotation(HRotation.Left, VRotation.Up);
                    }
                    else if (PressedKeys[i] == Keys.D || PressedKeys[i] == Keys.Right)
                    {
                        player.moveRight(playerSpeed);
                        playerSpriteComp.SetImageRotation(HRotation.Right, VRotation.Up);
                    }

                    //printText(debugType.Debug, player.x.ToString());
                    //printText(debugType.Debug, player.y.ToString());
                }
                PressedKeys.Clear();

                // Handle Collision
                if (player.GetX() + player.width > GameResolutionWidth)
                {
                    player.SetX(GameResolutionWidth - player.width);
                }

                else if (player.GetX() < 0)
                {
                    player.SetX(0);
                }

                if (player.GetY() + player.height > GameResolutionHeight)
                {
                    player.SetY(GameResolutionHeight - player.height);
                }

                else if (player.GetY() < 0)
                {
                    player.SetY(0);
                }
            }
        }
    }
}
