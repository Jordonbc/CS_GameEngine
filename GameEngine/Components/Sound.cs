using System.Media;
using System.Threading;

namespace GameEngine.Components
{
    public class Sound
    {
        public SoundPlayer player;
        public bool looping;
        private Thread t;

        public Sound(string SoundFile)
        {
            player = new SoundPlayer(SoundFile);
        }
        public void PlaySound()
        {
            t = new Thread(PS);
            t.Start();
        }

        public void PS()
        {
            if (looping)
            {
                player.PlayLooping();
            }
            else
            {
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
