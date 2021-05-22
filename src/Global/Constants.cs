namespace Global
{
	using Godot;
	using Actor;

	public static class Constants
	{
		public const float ActionHandlingDelay = 0.03f;
		public const float BaseRaccoonSpeed = 1f;
		public const float BaseHumanSpeed = 0.4f;
		public const float MaxRaccoonLookAngle = 46f;
		public const float MinRaccoonLookAngle = -30f;
		public const float TerminalVelocity = -15f;
		public const float GravityAcceleration = 0.2f;
		public const float MinimumJump = 0.1f;
		public const float JumpDecay = 0.75f;
		public const float InteractDelay = 0.1f;
		public const float InteractDistance = 3f;

		public const float HumanSightRadius = 20f;
		public const float HumanCatchRadius = 5f;
		public const string TrashCanGroup = "TrashCan";
		public const string ActorHeadPath = "HeadSpatial";
		public const string ActorTorsoPath = "TorsoCShape";
		public const string RaccoonScene = "res://Assets//Scenes/Raccoon.tscn";
		public const string HumanScene = "res://Assets//Scenes/Human.tscn";
		

		public static Vector3 SpawnPoint()
		{
			return new Vector3(0f, 10f, 0f);
		}

		public static Vector3 Up()
		{
			return new Vector3(0f, 1f, 0f);
		}

		public static Vector3 CameraPosition()
		{
			return new Vector3(0f, 10f, 10f);
		}

		public static Vector3 Gravity()
		{
		  // Should be meters squared per second, but whatevs
		  return new Vector3(0f, -0.5f, 0f);
		}

		public static Vector3 JumpThrust()
		{
		  return new Vector3(0f, 2.5f, 0f);
		}

		public static Actor RaccoonActor(bool addCamera, Vector3 position)
		{
			Actor actor = new Actor();
			PackedScene packedBodyScene = (PackedScene)ResourceLoader.Load(RaccoonScene);
			RaccoonBody body = packedBodyScene.Instance() as RaccoonBody;
			body.AssignActor(actor, addCamera, ActorHeadPath, ActorTorsoPath, CameraPosition());
			body.movementSpeed = BaseRaccoonSpeed;
			body.Translation = position;
			actor.body = body;
			
			return actor;
		}

		public static Actor HumanActor(Vector3 position)
		{
			Actor actor = new Actor();
			PackedScene packedBodyScene = (PackedScene)ResourceLoader.Load(HumanScene);
			HumanBody body = packedBodyScene.Instance() as HumanBody;
			body.AssignActor(actor, ActorHeadPath, ActorTorsoPath);
			body.movementSpeed = BaseHumanSpeed;
			body.Translation = position;
			actor.body = body;
			
			return actor;
		}
	}
}
