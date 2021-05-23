namespace Audio
{
    using Global;
    using Godot;
    using System.Collections.Generic;

    public class JukeBox : AudioStreamPlayer
    {
        public List<string> songs;
        public int currentSong;

        public JukeBox(Node parent)
        {
            parent.AddChild(this);
            songs = Constants.Songs();
            Connect("finished", this, nameof(PlayNext));
        }

        public void PlaySong(int selectedSong = 0)
        {
            currentSong = selectedSong;
            string songName = songs[selectedSong];
            AudioStreamOGGVorbis stream = (AudioStreamOGGVorbis)GD.Load(songName);
            stream.Loop = false;
            Stream = stream;
            Playing = true;
        }

        public void PlayNext()
        {
            GD.Print("PlayNext");
            currentSong++;
            if(currentSong >= songs.Count)
            {
                currentSong = 0;
            }
            PlaySong(currentSong);
        }
    }
}