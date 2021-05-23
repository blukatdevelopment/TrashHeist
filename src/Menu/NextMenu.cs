namespace Menu
{
    using Godot;
    using Global;
    using Actor;
    
    public class NextMenu : Container
    {
        public NextMenu(MenuManager manager)
        {
            manager.AddChild(this);
            GD.Print("Level complete.");

            Button loseMessage = new Button(
                this,
                "Level complete!",
                new Vector2(200f, 200f),
                new Vector2(0f, 0f),
                null
            );
            loseMessage.Disabled = true;

            Button mainMenu = new Button(
                this,
                "Next",
                new Vector2(200f, 200f),
                new Vector2(0f, 200f),
                () => 
                {
                    Main.Game.NextLevel();
                }
            );    
        }
        
    }
}