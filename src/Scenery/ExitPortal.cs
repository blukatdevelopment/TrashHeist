namespace Scenery 
{
	using Godot;
	using Global;
	using Actor;

	public class ExitPortal : StaticBody
	{
		private const string MeshName = "CSGMesh";
		private const string CollisionShape = "CollisionShape";

		public override void _Process(float delta)
		{
			if(Main.TrashGathered > 0 && IsPlayerInRange())
			{
					Main.Game.ExitLevel();
			}
		}

		private bool IsPlayerInRange()
		{
			if(Main.Player == null || Main.Player.body == null)
			{
				return false;
			}
			Body target = Main.Player.body;
			
			float distance = target.Transform.origin.DistanceTo(Transform.origin);
			return distance < Constants.ExitRadius;
		}
	}
}
