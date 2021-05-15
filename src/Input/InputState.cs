namespace Input
{
  using Godot;
  using System.Collections.Generic;
  using Global;
  using System;

  public class InputState : Node2D
  {
    private List<IActionSubscriber> subscribers;
    public List<KeyMapping> keyMappings;
    public List<AxisMapping> axisMappings;
    public ActionEnum actionToRemap;
    private bool paused;
    private Vector2 mousePosition;
    private Vector2 mouseMovement;

    public InputState()
    {
      this.keyMappings = new List<KeyMapping>();
      mousePosition = new Vector2();
      mouseMovement = new Vector2();
      actionToRemap = ActionEnum.None;
    }

    public InputState(List<KeyMapping> keyMappings, List<AxisMapping> axisMappings)
    {
      this.paused = false;
      this.keyMappings = keyMappings;
      this.axisMappings = axisMappings;
      subscribers = new List<IActionSubscriber>();
      mousePosition = new Vector2();
      mouseMovement = new Vector2();
      actionToRemap = ActionEnum.None;
    }

    public override void _Input(Godot.InputEvent evt)
    {
      InputEventMouseMotion motion = evt as InputEventMouseMotion;
      if(motion != null)
      {
        mousePosition = motion.GlobalPosition;
        mouseMovement = motion.Relative;
      }
    }

    public void RemapAction(ActionEnum action)
    {
      actionToRemap = action;
    }

    public override void _Process(float delta)
    {
      if(paused)
      {
        return;
      }

      if(actionToRemap != ActionEnum.None)
      {
        KeyList key = GetFirstKeyDown();
        if(key != KeyList.Unknown)
        {
          UpdateMapping(actionToRemap, key);
          actionToRemap = ActionEnum.None;
        }
        return;
      }

      List<ActionEvent> actionEvents = new List<ActionEvent>();
      foreach(KeyMapping keyMapping in keyMappings)
      {
        actionEvents.AddRange(keyMapping.GetEvents(delta));
      }

      foreach(AxisMapping axisMapping in axisMappings)
      {
        actionEvents.AddRange(GetAxisEvents(axisMapping));
      }
      mouseMovement = new Vector2();

      foreach(IActionSubscriber subscriber in subscribers)
      {
        subscriber.QueueActions(actionEvents);
      }
    }

    public List<ActionEvent> GetAxisEvents(AxisMapping mapping)
    {
      List<ActionEvent> events = new List<ActionEvent>();
      switch(mapping.axis)
      {
        case AxisEnum.MouseY:
          if(Utility.Abs(mouseMovement.y) > mapping.minimumInput)
          {
            events.Add(new ActionEvent(mapping.action, mapping.sensitivity * mouseMovement.y));
          }
          break;
        case AxisEnum.MouseX:
          if(Utility.Abs(mouseMovement.x) > mapping.minimumInput)
          {
            events.Add(new ActionEvent(mapping.action, mapping.sensitivity * mouseMovement.x));
          }
          break;
      }

      return events;
    }

    public void Pause()
    {
      paused = true;
      Godot.Input.SetMouseMode(Godot.Input.MouseMode.Visible);
    }

    public void Resume()
    {
      paused = false;
    }

    public void Subscribe(IActionSubscriber subscriber)
    {
      subscribers.Add(subscriber);
    }

    public void Unsubscribe(IActionSubscriber subscriber)
    {
      subscribers.Remove(subscriber);
    }

    private KeyList GetFirstKeyDown()
    {
      foreach(KeyList key in Enum.GetValues(typeof(KeyList)))
      {
        if(Input.IsKeyPressed((int)key))
        {
          return key;
        }
      }
      return KeyList.Unknown;
    }

    private void UpdateMapping(ActionEnum action, KeyList key)
    {

      foreach(KeyMapping mapping in keyMappings)
      {
        if(mapping.action == action)
        {
          mapping.key = key;
          GD.Print("Remapped " + action + " to " + key);
        }
        if(mapping.inverseAction == action)
        {
          mapping.key = key;
          GD.Print("Remapped inverse of " + action + " to " + key);
        }
      }
    }
  }
}
