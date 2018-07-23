using System;
using System.Collections.Generic;
using System.Drawing;


namespace GameEngine
{
    public class GameObject
    {
        public string Name;
        public int x = 0;
        public int y = 0;
        public int width;
        public int height;
        private List<BaseComponent> Components = new List<BaseComponent>();
        private BufferedGraphics Buffer;

        // Constructor
        public GameObject(string name)
        {
            Name = name;
        }

        public BufferedGraphics BufferedGFX { get => Buffer; set => Buffer = value; }

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
    }
}
