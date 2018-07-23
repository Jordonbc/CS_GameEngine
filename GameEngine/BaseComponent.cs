using System.Drawing;

namespace GameEngine
{
    public abstract class BaseComponent
    {
        // Constructor
        public GameObject parentObject;
        public int width = 0;
        public int height = 0;

        protected BaseComponent(GameObject parentObject)
        {
            this.parentObject = parentObject;
        }

        public abstract void Render();

    }
}
