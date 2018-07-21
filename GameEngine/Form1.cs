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
        //int playerSpeed = 5;

        List<GameObject> PlayerSegments = new List<GameObject>();

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

            localEngine.printText(EngineClass.debugType.Debug, "'localEngine' Defined!");

            localEngine.printText(EngineClass.debugType.Debug, "'player' Defined!");

            //localEngine.SetBackgroundColour(255/2, 255/5, 255/2);

            //localEngine.printText(EngineClass.debugType.Debug, "player.Index = " + player.index);

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

    internal class Game : EngineClass
    {
        public Game(Form win) : base(win)
        {
            debug = debugType.Error;
            CreateObject(win.Width/2 - 40, win.Height/2 - 40, 40, 40, colour: Color.DarkGreen, name: "Player");
        }

        public override bool GameLogic()
        {
            // This runs every frame and handles game logic
            GameObject player = GetObjectByName("Player");

            // Handle Key Presses

            if (PressedKeys.Count > 0)
            {
                for (int i = 0; i < PressedKeys.Count; i++)
                {
                    //Keys currentKeyCode = PressedKeys[i];

                    //printText(EngineClass.debugType.Debug, PressedKeys[i].ToString());

                    if (PressedKeys[i] == Keys.W)
                    {
                        player.y -= 5;
                    }
                    else if (PressedKeys[i] == Keys.S)
                    {
                        player.y += 5;
                    }

                    if (PressedKeys[i] == Keys.A)
                    {
                        player.x -= 5;
                    }
                    else if (PressedKeys[i] == Keys.D)
                    {
                        player.x += 5;
                    }

                    //printText(EngineClass.debugType.Debug, player.x.ToString());
                    //printText(EngineClass.debugType.Debug, player.y.ToString());
                }
                PressedKeys.Clear();

                // Handle Collision
                if (player.x + player.SizeX > GetCanvasWidth())
                {
                    player.x = GetCanvasWidth() - player.SizeX;
                }

                else if (player.x < 0)
                {
                    player.x = 0;
                }

                if (player.y + player.SizeY > GetCanvasHeight())
                {
                    player.y = GetCanvasHeight() - player.SizeY;
                }

                else if (player.y < 0)
                {
                    player.y = 0;
                }

                SetObjectByName("Player", player); // Commit changes back to the engine
            }
            return true; // we return true to notify the engine that we have finnished game logic pass successfully
        }
    }
}
