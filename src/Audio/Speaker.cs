namespace Audio
{
	using Godot;
	using Global;
	using System.Collections.Generic;

	public class Speaker : AudioStreamPlayer3D
	{
		public Speaker(Node parent)
		{
			parent.AddChild(this);
		}

		public void PlaySound(SoundEnum sound, bool loop = false, float volumeDb = 0f)
		{
			GD.Print($"Playing {sound}. Loop? {loop} volumeDb? {volumeDb} ");
			Dictionary<SoundEnum, string> sounds = Constants.SoundEffects();
			string soundPath = sounds[sound];
			AudioStreamOGGVorbis stream = (AudioStreamOGGVorbis)GD.Load(soundPath);
			stream.Loop = loop;
			UnitDb = volumeDb;
			MaxDb = volumeDb;
			Stream = stream;
			Playing = true;
		}

		public void StopSound()
		{
			Playing = false;
		}
	}
}
