namespace Input
{
  using Godot;
  using System.Collections.Generic;
  using Input;

  public class InputConstants
  {
    public const float LongPressDuration = 0.5f;

    public static List<KeyMapping> DefaultKeyMappings()
    {
      return new List<KeyMapping>
      {
        new KeyMapping(KeyList.W, InputModeEnum.PressStart, ActionEnum.MoveUp, ActionEnum.MoveUpEnd),
        new KeyMapping(KeyList.W, InputModeEnum.PressEnd, ActionEnum.MoveUpEnd, ActionEnum.MoveUp),
        new KeyMapping(KeyList.S, InputModeEnum.PressStart, ActionEnum.MoveDown, ActionEnum.MoveDownEnd),
        new KeyMapping(KeyList.S, InputModeEnum.PressEnd, ActionEnum.MoveDownEnd, ActionEnum.MoveDown),
        new KeyMapping(KeyList.D, InputModeEnum.PressStart, ActionEnum.MoveRight, ActionEnum.MoveRightEnd),
        new KeyMapping(KeyList.D, InputModeEnum.PressEnd, ActionEnum.MoveRightEnd, ActionEnum.MoveRight),
        new KeyMapping(KeyList.A, InputModeEnum.PressStart, ActionEnum.MoveLeft, ActionEnum.MoveLeftEnd),
        new KeyMapping(KeyList.A, InputModeEnum.PressEnd, ActionEnum.MoveLeftEnd, ActionEnum.MoveLeft),
        new KeyMapping(KeyList.Space, InputModeEnum.PressStart, ActionEnum.Fire, ActionEnum.FireEnd),
        new KeyMapping(KeyList.Space, InputModeEnum.PressEnd, ActionEnum.FireEnd, ActionEnum.Fire),
        new KeyMapping(KeyList.Escape, InputModeEnum.PressEnd, ActionEnum.TogglePlayerPause)
      };
    }

    public static List<AxisMapping> DefaultAxisMappings()
    {
        return new List<AxisMapping>
      {
        new AxisMapping(AxisEnum.MouseX, 1f, ActionEnum.AimHorizontal, 0.1f),
        new AxisMapping(AxisEnum.MouseY, 1f, ActionEnum.AimVertical, 0.1f)
      };
    }
  }
}