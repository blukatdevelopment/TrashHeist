namespace Menu
{
	using Godot;
	using Global;
	using Actor;

	public class PauseMenu : Container
	{
		public PauseMenu(MenuManager manager)
		{
			manager.AddChild(this);
			GD.Print("Pausemenu loaded.");

			Button returnButton = new Button(
				this,
				"Return",
				new Vector2(200f, 200f),
				new Vector2(),
				() => 
				{
					Actor player = Main.player;
					RaccoonBody body = player.body as RaccoonBody;
					body.Resume();
				}
			);

			Button mainMenu = new Button(
				this,
				"MainMenu",
				new Vector2(200f, 200f),
				new Vector2(0f, 200f),
				() => 
				{
					Main.Game.MainMenu();
				}
			);
		}

	}
}
