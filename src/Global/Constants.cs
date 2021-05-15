namespace Global
{
	using Godot;
	using Actor;

	public static class Constants
	{
		public const float ActionHandlingDelay = 0.03f;
		public const float BaseRaccoonSpeed = 1f;
		public const float MaxRaccoonLookAngle = 46f;
		public const float MinRaccoonLookAngle = -30f;
		public const string ActorHeadPath = "HeadSpatial";
		public const string ActorTorsoPath = "TorsoCShape";
		public const string RaccoonScene = "res://Assets//Scenes/Raccoon.tscn";

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
	}
}
