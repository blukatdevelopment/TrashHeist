namespace Menu
{
    using Godot;
    using Global;

    public class StartMenu : Container
    {
        public StartMenu(MenuManager manager)
        {
            manager.AddChild(this);
            GD.Print("Start menu loaded.");

            Button newGame = new Button(
                this,
                "Play",
                new Vector2(200f, 200f),
                new Vector2(),
                () => 
                {
                    Main.Game.StartGame();
                }
            );
        }

    }
}