namespace Menu
{
    using Godot;
    using Global;
    using Actor;

    public class LoseMenu : Container
    {
        public LoseMenu(MenuManager manager)
        {
            manager.AddChild(this);
            GD.Print("Game over.");

            Button loseMessage = new Button(
                this,
                "Game Over",
                new Vector2(200f, 200f),
                new Vector2(0f, 0f),
                null
            );
            loseMessage.Disabled = true;

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
