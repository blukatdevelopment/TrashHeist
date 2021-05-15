namespace Input
{
  // Contains information about an action
  public class ActionEvent
  {
    public ActionEnum action;
    public float magnitude;
    
    public ActionEvent(ActionEnum action)
    {
      this.action = action;
    }

    public ActionEvent(ActionEnum action, float magnitude)
    {
      this.action = action;
      this.magnitude = magnitude;
    }
  }
}
