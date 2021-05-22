namespace Global
{
	using Godot;
	using System;
	using System.Collections.Generic;
	using Input;
	using Global;
	using Actor;
	using Menu;
	using AI;

	public class Main : Spatial
	{
		public static Main Game;
		public static InputState inputState;
		public static Actor player;
		public static int TrashGathered;
		public List<Actor> activeActors;
		public MenuManager menuManager;
		public Spatial activeMap;

		public override void _Ready()
		{
			Game = this;
			activeActors = new List<Actor>();
			menuManager = new MenuManager(this);
			menuManager.SwitchMenu(MenuManager.Menus.Start);
		}

		public void StartGame()
		{
			menuManager.SwitchMenu(MenuManager.Menus.None);
			inputState = new InputState(InputConstants.DefaultKeyMappings(), InputConstants.DefaultAxisMappings());
			AddChild(inputState);

			player = Constants.RaccoonActor(true, Constants.SpawnPoint());
			AddActor(player);

			AddActor(Constants.HumanActor(Constants.SpawnPoint() + new Vector3(0, 0, 25)), false);
		}

		public void MainMenu()
		{

			menuManager.SwitchMenu(MenuManager.Menus.Start);
			ClearActiveLevel();
		}

		public void ClearActiveLevel()
		{
			List<Actor> clonedList = new List<Actor>();
			foreach(Actor actor in activeActors)
			{
				clonedList.Add(actor);
			}

			foreach(Actor actor in clonedList)
			{
				RemoveActor(actor);
			}
			player = null;
		}

		public void AddActor(Actor actor, bool useInputState = true)
		{
			activeActors.Add(actor);
			if(useInputState)
			{
				inputState.Subscribe(actor.body);
			}
			AddChild(actor.body);
		}

		public void RemoveActor(Actor actor)
		{
			activeActors.Remove(actor);
			inputState.Unsubscribe(actor.body);
			if(actor.body != null)
			{
				actor.body.QueueFree();
			}
		}

		public void GameOver()
		{
			GD.Print("You were caught");
			inputState.Pause();
			foreach(Actor actor in activeActors)
			{
				actor.body.Pause();
			}
			menuManager.SwitchMenu(MenuManager.Menus.Lose);
		}

		public void AlertHumans()
		{
			foreach(Actor actor in activeActors)
			{
				HumanBody body = actor.body as HumanBody;
				if(body != null)
				{
					body.agent.Transition(HumanAI.States.Pursuit);
				}
			}
		}

		public void ShowExitZone()
		{

		}

		public void ExitLevel()
		{
			GD.Print("You exited the level");
		}
	}
}
