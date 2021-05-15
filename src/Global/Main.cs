namespace Global
{
	using Godot;
	using System;
	using System.Collections.Generic;
	using Input;
	using Global;
	using Actor;

	public class Main : Spatial
	{
		public static InputState inputState;
		public List<Actor> activeActors;

		public override void _Ready()
		{
			activeActors = new List<Actor>();
			inputState = new InputState(InputConstants.DefaultKeyMappings(), InputConstants.DefaultAxisMappings());
			AddChild(inputState);
			AddActor(Constants.RaccoonActor(true, Constants.SpawnPoint()));
		}

		public void AddActor(Actor actor)
		{
			activeActors.Add(actor);
			inputState.Subscribe(actor.body);
			AddChild(actor.body);
		}

		public void RemoveActor(Actor actor)
		{
			activeActors.Remove(actor);
			inputState.Unsubscribe(actor.body);
		}
		
	}
}
