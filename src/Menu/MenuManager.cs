namespace Menu
{
    using Godot;

    // Lots a stuff crammed into here, because I don't want a robust menu framework
    public class MenuManager : Control
    {
        public enum Menus
        {
            None,
            Start,
            Settings,
            Win,
            Next,
            Lose,
            Pause
        }

        public Container activeMenu;

        public MenuManager(Node parent)
        {
            parent.AddChild(this);
        }

        public void SwitchMenu(Menus newMenu)
        {
            if(activeMenu != null)
            {
                activeMenu.QueueFree();
                activeMenu = null;
            }
            switch(newMenu)
            {
                case Menus.None:
                break;
                case Menus.Start:
                    activeMenu = new StartMenu(this);
                break;

                case Menus.Settings:
                break;

                case Menus.Win:
                    activeMenu = new WinMenu(this);
                break;

                case Menus.Next:
                    activeMenu = new NextMenu(this);
                break;

                case Menus.Lose:
                    activeMenu = new LoseMenu(this);
                break;

                case Menus.Pause:
                    activeMenu = new PauseMenu(this);
                break;
            }
        }
    }
}