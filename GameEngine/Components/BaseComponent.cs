using System;
using System.Drawing;

namespace GameEngine
{
    public abstract class BaseComponent : IDisposable
    {
        // Constructor
        public GameObject parentObject;
        public Rectangle rectangle;

        private bool disposed;


        protected BaseComponent(GameObject parentObject)
        {
            this.parentObject = parentObject;
        }

        public abstract void Render();

        public abstract void Tick();

        /// <summary>
        /// Destructor
        /// </summary>
        ~BaseComponent()
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
                }

                // Dispose unmanaged resources here.
            }

            disposed = true;
        }
    }
}
