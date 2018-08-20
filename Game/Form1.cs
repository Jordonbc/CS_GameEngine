﻿using GameEngine.Components;
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

namespace GameEngine
{
    public partial class Form1 : Form
    {
        Game localEngine;
        //GameObject player;
        
        Sound GMusic = new Sound("Music.wav");
        Sound MClick = new Sound("laser7.wav");

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
            localEngine.Gravity = 1;

            Player Player = new Player("Player", localEngine);

            GameObject Ground = new GameObject("Ground", localEngine);

            BoxComponent b = new BoxComponent(localEngine, Ground, Color.Brown, new Rectangle(0, 0, localEngine.GameResolutionWidth, 50));
            Ground.AddComponent(b);
            Ground.SetX(0);
            Ground.SetY(localEngine.GameResolutionHeight - 100);
            Ground.GravityScale = 0;
            Console.WriteLine(Ground.GetY());

            localEngine.CreateObject(Ground);

            //Player.CanTick = false;
            Player.GravityScale = 1;

            GameObject GameRenderTextObj = new GameObject("GameRenderTextObj", localEngine);
            GameObject WindowSizeTextObj = new GameObject("WindowSizeTextObj", localEngine);

            localEngine.AddFPSCounter();

            TextComponent tc = new TextComponent(localEngine, GameRenderTextObj, 1, 20, Color.White, "Resolution Width", new FontFamily("Arial"), 12);
            GameRenderTextObj.AddComponent(tc);

            TextComponent tc2 = new TextComponent(localEngine, WindowSizeTextObj, 1, 50, Color.White, "Resolution Width", new FontFamily("Arial"), 12);
            WindowSizeTextObj.AddComponent(tc2);


            localEngine.CreateUIObject(GameRenderTextObj);
            localEngine.CreateUIObject(WindowSizeTextObj);

            localEngine.PrintText(debugType.Debug, "'localEngine' Defined!");

            localEngine.PrintText(debugType.Debug, "'player' Defined!");

            localEngine.SetBackgroundColour(255/2, 255/5, 255/2);

            //localEngine.printText(EngineClass.debugType.Debug, "player.Index = " + player.index);

            //localEngine.CreateObject(GameObj: new Player());

            localEngine.GraphicsSettings.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            localEngine.GraphicsSettings.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //                       engine reference      obj ref
            //BoxComponent c = new BoxComponent(localEngine, Player, 20 , 0, Color.Blue, 50, 50);
            //Player.AddComponent(c);

            //BoxComponent b = new BoxComponent(localEngine, Player, 0, 0, Color.Blue, 20, 20);
            //Player.AddComponent(b);

            Player.SetX(localEngine.GameResolutionWidth / 2 - Player.GetWidth() / 2);
            Player.SetY(localEngine.GameResolutionHeight / 2 - Player.GetHeight() / 2);

            SpriteComponent spr = new SpriteComponent(localEngine, Player, 0, 0, 50, 50, Image.FromFile("Mario.png"));
            //spr.resizeImage(50, 50);
            //spr.MakeTransparent();
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

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            MClick.PlaySound();
        }
    }

    internal class Player : PlayerObject
    {
        public Player(String PlayerName, EngineClass engine) : base(PlayerName, engine)
        {
            
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }

        public override void Tick()
        {
            base.Tick();
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
            // This runs every frame and handles game logic
            Player player = (Player)GetObjectByName("Player");
            SpriteComponent playerSpriteComp = (SpriteComponent)player.GetComponent("SpriteComponent");

            GameObject GameResTextObj = GetUIObjectByName("GameRenderTextObj");
            TextComponent GameResTextObjComp = (TextComponent)GameResTextObj.GetComponent("TextComponent");

            GameResTextObjComp.SetText("Render Width: " + GameResolutionWidth.ToString() + ", Render Height: " + GameResolutionHeight.ToString());

            GameObject WindowSizeTextObj = GetUIObjectByName("WindowSizeTextObj");
            TextComponent WindowSizeTextObjComp = (TextComponent)WindowSizeTextObj.GetComponent("TextComponent");

            WindowSizeTextObjComp.SetText("Window Width: " + WindowWidth.ToString() + ", Render Height: " + WindowHeight.ToString());

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

                    // REMOVED S to go down because gravity pulls the player down
                    //else if (PressedKeys[i] == Keys.S || PressedKeys[i] == Keys.Down)
                    //{
                    //    player.moveDown(playerSpeed);
                    //    //playerSpriteComp.SetImageRotation(Rotation.Down);
                    //}

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
                //if (player.GetX() + player.GetWidth() > GetCanvasWidth())
                //{
                //    player.x = GetCanvasWidth() - player.width;
                //}

                //else if (player.x < 0)
                //{
                //    player.x = 0;
                //}

                //if (player.y + player.height > GetCanvasHeight())
                //{
                //    player.y = GetCanvasHeight() - player.height;
                //}

                //else if (player.y < 0)
                //{
                //    player.y = 0;
                //}
            }
        }
    }
}
