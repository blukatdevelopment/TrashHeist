namespace Menu
{
    using Godot;
    using Global;
    using Actor;
    
    public class WinMenu : Container
    {
        public WinMenu(MenuManager manager)
        {
            manager.AddChild(this);
            GD.Print("Level complete.");

            Button loseMessage = new Button(
                this,
                "You win!",
                new Vector2(200f, 200f),
                new Vector2(0f, 0f),
                null
            );
            loseMessage.Disabled = true;

            Button mainMenu = new Button(
                this,
                "Main Menu",
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