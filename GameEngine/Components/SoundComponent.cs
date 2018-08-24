using System.Media;
using System.Threading;

namespace GameEngine.Components
{
    public class SoundComponent
    {
        public SoundPlayer player;
        public bool looping;
        private Thread t;

        public SoundComponent(string SoundFile)
        {
            player = new SoundPlayer(SoundFile);
        }
        public void PlaySound()
        {
            t = new Thread(PS);
            t.Start();
        }
        

        private void PS()
        {
            if (looping)
            {
                player.PlayLooping();
            }
            else
            {
                // TODO: Find a way to play multiple sounds at same time
                player.Play();
            }
        }
        public void StopSound()
        {
            player.Stop();
            t.Join();
        }
    }
}
