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
        new KeyMapping(KeyList.Space, InputModeEnum.PressStart, ActionEnum.Jump, ActionEnum.JumpEnd),
        new KeyMapping(KeyList.Space, InputModeEnum.PressEnd, ActionEnum.JumpEnd, ActionEnum.Jump),
        new KeyMapping(KeyList.Escape, InputModeEnum.PressEnd, ActionEnum.TogglePlayerPause),
        new KeyMapping(KeyList.E, InputModeEnum.PressStart, ActionEnum.Interact),
        new KeyMapping(KeyList.E, InputModeEnum.PressEnd, ActionEnum.InteractEnd),
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