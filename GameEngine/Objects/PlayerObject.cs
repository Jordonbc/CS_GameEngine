using System;

namespace GameEngine
{
    public class PlayerObject : GameObject
    {
        public PlayerObject(string PlayerName, EngineClass engine) : base(PlayerName, engine)
        {

        }

        public virtual void OnDeath()
        {

        }

        public void moveRight(int Amount)
        {
            rectangle.X = rectangle.X + Amount;
        }

        public void moveLeft(int Amount)
        {
            rectangle.X = rectangle.X - Amount;
        }

        public void moveUp(int Amount)
        {
            //Console.WriteLine(IsOnGround());
            if (IsOnGround())
            {
                rectangle.Y = rectangle.Y - Amount;
            }
        }

        public void moveDown(int Amount)
        {
            if (IsInAir())
            {
                rectangle.Y = rectangle.Y + Amount;
            }
        }

        public override void Tick()
        {
            base.Tick();
        }
    }
}
