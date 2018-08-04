﻿using System;
using System.Threading;

namespace GameEngine
{
    partial class EngineClass
    {
        private void GameLoop()
        {

            while (window.Visible & !HasError)
            {
                Calculate_FPS_StopWatch.Start();
                try
                {
                    GameLogic();

                    Render();

                    if (FPS > 0)
                    {
                        if (1000 / FPS > 0)
                        { Thread.Sleep(1000 / FPS); }
                    }
                }
                catch (Exception e)
                {
                    HasError = true;
                    PrintText(debugType.Error, e.Message + "\n\n Stacktrace: \n\n" + e.StackTrace);
                }

                Calculate_FPS_StopWatch.Stop();
                timeSpanFPS = Calculate_FPS_StopWatch.Elapsed;
                Calculate_FPS_StopWatch.Reset();
            }
        }
    }
}
