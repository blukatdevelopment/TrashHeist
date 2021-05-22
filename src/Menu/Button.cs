namespace Menu
{
    using Godot;
    using System;

    public class Button : Godot.Button
    {
        public Action onClick;
        
        public Button(Node parent, string text, Vector2 size, Vector2 position, Action onClick)
        {
            parent.AddChild(this);
            this.Text = text;
            this.onClick = onClick;
            this.SetPosition(position);
            this.SetSize(size);
        }

        public override void _Ready()
        {
        }
        
        public void SetOnClick(Action onClick)
        {
          this.onClick = onClick;
        }
        
        public override void _Pressed()
        {
          if(onClick != null)
          {
              onClick();
          }
        }
    }
}