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
        protected int x = 0;
        protected int y = 0;
        public string Name;
        private int CameraX;
        private int CameraY;
        private EngineClass engine;
        public bool isPlayer = false;

        private List<BaseComponent> Components = new List<BaseComponent>();


        bool disposed = false;

        // Constructor
        public GameObject(EngineClass engine, string name)
        {
            Name = name;
            CameraX = engine.CameraX;
            CameraY = engine.CameraY;
            this.engine = engine;
        }

        public int GetX()
        {
            if (!isPlayer)
            {
                return CameraX + x;
            }
            else
            {
                return x;
            }
            
        }

        public int GetY()
        {
            if (!isPlayer)
            {
                return CameraY + y;
            }
            else
            {
                return y;
            }
        }

        public void SetX(int newX)
        {
            if (!isPlayer)
            {
                x = CameraX + newX;
            }
            else
            {
                x = newX;
            }
        }

        public void SetY(int newY)
        {
            if (!isPlayer)
            {
                y = CameraY + newY;
            }
            else
            {
                y = newY;
            }
        }

        public Rectangle CollisionBox()
        {
            return new Rectangle(x, y, width, height);
        }


        public BufferedGraphics BufferedGFX { get; set; }
        public int width { get; set; } = 1;
        public int height { get; set; } = 1;

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

        public virtual void Tick()
        {
            // update camera offsets
            CameraX = engine.CameraX;
            CameraY = engine.CameraY;
        }
        
        public void recalculateSize()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                width = width + Components[i].width;
                height = height + Components[i].height;
            }
        }

        public void AddComponent(BaseComponent c)
        {
            Components.Add(c);
            recalculateSize();
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
