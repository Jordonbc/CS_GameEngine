using System;
using System.Collections.Generic;
using System.Drawing;


namespace GameEngine
{
    public class InvalidComponentException : Exception
    {
        public InvalidComponentException(string message)
           : base(message)
        {
        }
    }

    public class GameObject : IDisposable
    {
        public string Name;
        public int GravityScale = 0;
        public bool isSolid = true;
        public bool CanTick = true;
        public bool isHit;
        
        protected EngineClass Engine;

        public Rectangle rectangle;

        private List<BaseComponent> Components = new List<BaseComponent>();


        private bool disposed = false;

        // Constructor
        public GameObject(string name, EngineClass engine)
        {
            Name = name;
            Engine = engine;
        }

        public void SetX(int x) => rectangle.X = x;

        public void SetY(int y) => rectangle.Y = y;

        public void SetWidth(int w) => rectangle.Width = w;

        public void SetHeight(int h) => rectangle.Height = h;


        public int GetX() => rectangle.X;

        public int GetY() => rectangle.Y;

        public int GetWidth() => rectangle.Width;

        public int GetHeight() => rectangle.Height;

        private void setRectangle(int x, int y, int w, int h)
        {
            rectangle = new Rectangle(x, y, w, h);
        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public bool IsOnGround()
        {
            GameObject OtherObj = CheckForHit();
            if (OtherObj != null) // If no object collision
            {
                //Console.WriteLine(GetHitAxis(OtherObj).ToString());
                if (HasHitBottom(OtherObj))
                {
                    return true;
                }
                else return false;
            }
            else return false; // No objects in collision
        }

        public bool IsInAir()
        {
            GameObject OtherObj = CheckForHit();
            if (OtherObj == null) // If no object collision
            {
                return true;
            }
            else return false; // objects in collision
        }

        public virtual void Tick()
        {
            if (CheckForHit() == null)
            {
                isHit = false;
            }
            else
            {
                isHit = true;
            }

            //Console.WriteLine(isHit);

            if (CanTick)
            {
                //Console.WriteLine(IsInAir());
                if (!IsOnGround())
                {
                    rectangle.Y = rectangle.Y + (Engine.Gravity * GravityScale);
                }
            }

            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Tick();
            }
        }

        public BufferedGraphics BufferedGFX { get; set; }

        public GameObject CheckForHit()
        {
            GameObject hitObj = null;

            if (Engine.GetGameObjects().Count > 0)
            {
                for (int index = 0; index < Engine.GetGameObjects().Count; index++)
                {
                    GameObject currentObj = Engine.GetGameObjects()[index];

                    if (currentObj != this) // dont check collision with itself
                    {
                        if (rectangle.IntersectsWith(currentObj.rectangle))
                        {
                            hitObj = currentObj;
                        }
                    }
                }
            }
            return hitObj;
        }

        public bool HasHitBottom(GameObject obj)
        {
            if (rectangle.Bottom >= obj.rectangle.Top)
            {
                return true;
            }
            else return false;
        }

        public bool HasHitTop(GameObject obj)
        {
            if (rectangle.Top <= obj.rectangle.Bottom)
            {
                return true;
            }
            else return false;
        }

        public bool HasHitRight(GameObject obj)
        {
            if (rectangle.Right >= obj.rectangle.Left)
            {
                return true;
            }
            else return false;
        }

        public bool HasHitLeft(GameObject obj)
        {
            if (rectangle.Left <= obj.rectangle.Right)
            {
                return true;
            }
            else return false;
        }


        public Axis GetHitAxis(GameObject obj)
        {
            if (obj != null) // always check for if it has been given null parameter
            {
                Console.WriteLine("OBJECT IS NOT NULL");
                if (rectangle.Bottom >= obj.rectangle.Top) // Hit ground?
                {
                    Console.WriteLine("RETURNING DOWN");
                    return Axis.DOWN;
                }
                if (rectangle.Top + rectangle.Y >= obj.rectangle.Top) // Hit a Roof?
                {
                    Console.WriteLine("RETURNING UP");
                    return Axis.UP;
                }
                if (obj.rectangle.Left <= rectangle.Right) // Hit something on right
                {
                    Console.WriteLine("RETURNING RIGHT");
                    return Axis.RIGHT;
                }
                if (obj.rectangle.Right >= rectangle.Left)
                {
                    Console.WriteLine("RETURNING LEFT");
                    return Axis.LEFT;
                }
                else
                {
                    Console.WriteLine("RETURNING NULL");
                    return Axis.NULL; // No collision was found maybe manually triggered?
                }
            }
            else return Axis.NULL; // Return null because there are no objects in the scene to check collision with
        }

        public BaseComponent GetComponent(string name)
        {
            BaseComponent selectedComp = null;
            for (int i = 0; i < Components.Count; i++)
            {
                //Console.WriteLine("Component: " + Components[i].GetType().Name);
                if (Components[i].GetType().Name == name)
                {
                    selectedComp = Components[i];
                    break;
                }
            }


            if (selectedComp == null)
                {
                throw new InvalidComponentException("Component '" + name + "' does not exist!");
            }

            return selectedComp;
        }

        

        public void Render()
        {
            // Render all components
            for (int i = 0; i < Components.Count; i++)
            {
                //Console.WriteLine("Rendering component: " + Components[i].GetType().Name);
                Components[i].Render();
            }
        }

        public void AddComponent(BaseComponent c)
        {
            Components.Add(c);

            rectangle.Width = rectangle.Width + c.rectangle.Width;
            rectangle.Height = rectangle.Height + c.rectangle.Height;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GameObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The dispose method that implements IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The virtual dispose method that allows
        /// classes inherithed from this one to dispose their resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                    for (int i = 0; i < Components.Count; i++)
                    {
                        Components[i].Dispose();
                    }
                }

                // Dispose unmanaged resources here.

            }

            disposed = true;
        }
    }
}
