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

        EngineClass localEngine;
        GameObject player;
        int playerSpeed = 5;

        List<GameObject> PlayerSegments = new List<GameObject>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();

            Console.WriteLine("Debug Console Initialized");

            localEngine = new EngineClass(this);

            localEngine.printText(EngineClass.debugType.Debug, "'localEngine' Defined!");

            player = localEngine.AddGraphics(this.Width/2 - 10, this.Height/2 - 10, 20, 20, Color.Green);

            localEngine.printText(EngineClass.debugType.Debug, "'player' Defined!");

            localEngine.printText(EngineClass.debugType.Debug, "player.Index = " + player.index);

            localEngine.printText(EngineClass.debugType.Debug, "Starting Engine");
            localEngine.startGame();

            localEngine.FPS = 60;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread stopThread = new Thread(localEngine.stopGame);
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                player.y -= playerSpeed;
                localEngine.SetObject(player.index, player);
            }
            else if (e.KeyCode == Keys.Down)
            {
                player.y += playerSpeed;
                localEngine.SetObject(player.index, player);
            }
            if (e.KeyCode == Keys.Left)
            {
                player.x -= playerSpeed;
                localEngine.SetObject(player.index, player);
            }
            else if (e.KeyCode == Keys.Right)
            {
                player.x += playerSpeed;
                localEngine.SetObject(player.index, player);
            }

            if (e.KeyCode == Keys.T)
            {
                localEngine.lockFPS = true;
                localEngine.FPS = 45;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            localEngine.resizeGameCanvas(this.Width, this.Height);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.T)
            //{
            //    localEngine.lockFPS = false;
            //    localEngine.FPS = 60;
            //}
        }
    }
}
