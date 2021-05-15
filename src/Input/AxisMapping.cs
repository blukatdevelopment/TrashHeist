namespace Input
{
  public class AxisMapping
  {
    public AxisEnum axis;
    public float sensitivity;
    public ActionEnum action;
    public float minimumInput;

    public AxisMapping(AxisEnum axis, float sensitivity, ActionEnum action, float minimumInput)
    {
      this.axis = axis;
      this.sensitivity = sensitivity;
      this.action = action;
      this.minimumInput = minimumInput;
    }
  }
}