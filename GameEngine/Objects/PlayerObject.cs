using System;
using System.Collections.Generic;
using System.Drawing;


namespace GameEngine
{
    public class PlayerObject : GameObject
    {
        // Constructor
        public PlayerObject(EngineClass engine, string name) : base(engine, name)
        {
            isPlayer = true;
        }
        public override void Tick()
        {
            base.Tick();

        }

        public void moveRight(int Amount)
        {
            SetX(x + Amount);
        }

        public void moveLeft(int Amount)
        {
            SetX(x - Amount);
        }

        public void moveUp(int Amount)
        {
            SetY(y - Amount);
        }

        public void moveDown(int Amount)
        {
            SetY(y + Amount);
        }
    }
}