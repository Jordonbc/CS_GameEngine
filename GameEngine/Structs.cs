namespace GameEngine
{
    public struct Vector
    {
        public int x, y;

        public Vector(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    public struct Vector4D
    {
        public int width, height;

        public Vector Coord { get { return Coord; } set { Coord = value; } } = new Vector(0, 0);

        public Vector GetVector()
        {
            return Coord;
        }



        public Vector4D(int X, int Y, int width, int height)
        {
            Coord.x = X;
            Coord.y = Y;
            this.width = width;
            this.height = height;
        }
    }
}