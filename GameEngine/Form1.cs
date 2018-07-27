using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace GameEngine
{
    public partial class Form1 : Form
    {

        Game localEngine;
        //GameObject player;
        GameObject Player = new GameObject("Player");

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
            //localEngine.debug = EngineClass.debugType.Debug;

            

            localEngine.printText(EngineClass.debugType.Debug, "'localEngine' Defined!");

            localEngine.printText(EngineClass.debugType.Debug, "'player' Defined!");

            localEngine.SetBackgroundColour(255/2, 255/5, 255/2);

            //localEngine.printText(EngineClass.debugType.Debug, "player.Index = " + player.index);

            //localEngine.CreateObject(GameObj: new Player());

            localEngine.GraphicsSettings.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            localEngine.GraphicsSettings.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //                       engine reference      obj ref
            //BoxComponent c = new BoxComponent(localEngine, Player, 20 , 0, Color.Red, 50, 50);
            //Player.AddComponent(c);

            //BoxComponent b = new BoxComponent(localEngine, Player, 0, 0, Color.Blue, 20, 20);
            //Player.AddComponent(b);

            Player.x = this.Width / 2 - Player.width / 2;
            Player.y = this.Height / 2 - Player.height / 2;

            SpriteComponent spr = new SpriteComponent(localEngine, Player, 0, 0, 50, 50, Image.FromFile("Mario.jpg"));
            //spr.resizeImage(50, 50);
            spr.MakeTransparent();
            Player.AddComponent(spr);

            localEngine.CreateObject(Player);

            localEngine.printText(EngineClass.debugType.Debug, "Starting Engine");
            localEngine.startGame();

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
    }



    //public class Player : GameObject
    //{
    //    public Player() : base()
    //    {
    //        Box b = new Box(0, 0, Color.Blue, 10, 10);
    //        AddComponent(b);
    //    }
    //}

    internal class Game : EngineClass
    {
        int playerSpeed = 20;

        public Game(Form win) : base(win)
        {
            debug = debugType.Error;
            FPSTick = 50;
            //CreateObject(win.Width/2 - 40, win.Height/2 - 40, 40, 40, colour: Color.DarkGreen, name: "Player");
        }


        public override bool GameLogic()
        {
            // This runs every frame and handles game logic
            GameObject player = GetObjectByName("Player");

            SpriteComponent playerSpriteComp = (SpriteComponent)player.GetComponent("SpriteComponent");

            // Handle Key Presses

            if (PressedKeys.Count > 0)
            {
                for (int i = 0; i < PressedKeys.Count; i++)
                {
                    //Keys currentKeyCode = PressedKeys[i];

                    //printText(EngineClass.debugType.Debug, PressedKeys[i].ToString());

                    if (PressedKeys[i] == Keys.W)
                    {
                        player.moveUp(playerSpeed);
                        //playerSpriteComp.SetImageRotation(Rotation.Up);
                    }
                    else if (PressedKeys[i] == Keys.S)
                    {
                        player.moveDown(playerSpeed);
                        //playerSpriteComp.SetImageRotation(Rotation.Down);
                    }

                    if (PressedKeys[i] == Keys.A)
                    {
                        player.moveLeft(playerSpeed);
                        playerSpriteComp.SetImageRotation(HRotation.Left, VRotation.Up);
                    }
                    else if (PressedKeys[i] == Keys.D)
                    {
                        player.moveRight(playerSpeed);
                        playerSpriteComp.SetImageRotation(HRotation.Right, VRotation.Up);
                    }

                    //printText(EngineClass.debugType.Debug, player.x.ToString());
                    //printText(EngineClass.debugType.Debug, player.y.ToString());
                }
                PressedKeys.Clear();

                // Handle Collision
                if (player.x + player.width > GetCanvasWidth())
                {
                    player.x = GetCanvasWidth() - player.width;
                }

                else if (player.x < 0)
                {
                    player.x = 0;
                }

                if (player.y + player.height > GetCanvasHeight())
                {
                    player.y = GetCanvasHeight() - player.height;
                }

                else if (player.y < 0)
                {
                    player.y = 0;
                }

                //SetObjectByName("Player", player); // Commit changes back to the engine
            }
            return true; // we return true to notify the engine that we have finnished game logic pass successfully
        }
    }
}
