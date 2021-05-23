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
	using Audio;

	public class Main : Spatial
	{
		public static Main Game;
		public static InputState inputState;
		public static Actor Player;
		public static int TrashGathered;
		public List<Actor> activeActors;
		public MenuManager menuManager;
		public Spatial activeMap;
		private int activeLevel;
		private Spatial levelSpatial;
		private JukeBox jukeBox;

		public override void _Ready()
		{
			Game = this;
			activeActors = new List<Actor>();
			menuManager = new MenuManager(this);
			menuManager.SwitchMenu(MenuManager.Menus.Start);
			jukeBox = new JukeBox(this);
			jukeBox.PlaySong();
		}

		public void StartGame()
		{
			menuManager.SwitchMenu(MenuManager.Menus.None);
			activeLevel = 0;
			LoadLevel();
		}

		public void RestartLevel()
		{
			LoadLevel();
		}

		public void LoadLevel()
		{
			ClearActiveLevel();
			string levelPath = GetLevel(activeLevel);
			GD.Print($"Loading level {levelPath}");

			inputState = new InputState(InputConstants.DefaultKeyMappings(), InputConstants.DefaultAxisMappings());
			AddChild(inputState);

			PackedScene packedLevel = (PackedScene)ResourceLoader.Load(levelPath);
			levelSpatial = (Spatial)packedLevel.Instance();
			AddChild(levelSpatial);

			Spatial spawnPointSpatial = (Spatial)GetNode(new NodePath(levelSpatial.Name + "/" + Constants.SpawnPointName));
			Vector3 spawnPoint = spawnPointSpatial.Transform.origin;
			Player = Constants.RaccoonActor(true, spawnPoint);
			AddActor(Player);
			
			foreach(Node node in levelSpatial.GetChildren())
			{
				if(node.IsInGroup(Constants.HumanSpawnName))
				{
					Spatial spawnSpatial = node as Spatial;
					AddActor(Constants.HumanActor(spawnSpatial.Transform.origin), false);
				}
			}
		}

		public void MainMenu()
		{

			menuManager.SwitchMenu(MenuManager.Menus.Start);
			ClearActiveLevel();
		}

		public void ClearActiveLevel()
		{
			TrashGathered = 0;
			List<Actor> clonedList = new List<Actor>();
			foreach(Actor actor in activeActors)
			{
				clonedList.Add(actor);
			}

			foreach(Actor actor in clonedList)
			{
				RemoveActor(actor);
			}
			Player = null;
			if(levelSpatial != null)
			{
				levelSpatial.QueueFree();
				levelSpatial = null;
			}
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
			Pause();
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

		public void ExitLevel()
		{
			Pause();
			ClearActiveLevel();
			if(GetLevel(activeLevel + 1) == null)
			{
				menuManager.SwitchMenu(MenuManager.Menus.Win);
				return;
			}
			menuManager.SwitchMenu(MenuManager.Menus.Next);			
		}

		public string GetLevel(int levelId)
		{
			List<string> levels = Constants.Levels();
			if(levels.Count <= levelId || levelId < 0)
			{
				return null;
			}
			return levels[levelId];
		}

		public void NextLevel()
		{
			activeLevel++;
			LoadLevel();
		}

		public void Pause()
		{
			inputState.Pause();
			foreach(Actor actor in activeActors)
			{
				actor.body.Pause();
			}
		}
	}
}
