using System;
using System.Threading;
using System.Windows.Forms;

namespace GameEngine
{
    partial class EngineClass
    {
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
