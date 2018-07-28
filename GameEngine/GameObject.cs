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
        public int x = 0;
        public int y = 0;
        public int width;
        public int height;

        private List<BaseComponent> Components = new List<BaseComponent>();
        private BufferedGraphics Buffer;


        bool disposed = false;

        // Constructor
        public GameObject(string name)
        {
            Name = name;
        }

        public BufferedGraphics BufferedGFX { get => Buffer; set => Buffer = value; }

        public void moveRight(int Amount)
        {
            x = x + Amount;
        }

        public void moveLeft(int Amount)
        {
            x = x - Amount;
        }

        public void moveUp(int Amount)
        {
            y = y - Amount;
        }

        public void moveDown(int Amount)
        {
            y = y + Amount;
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
