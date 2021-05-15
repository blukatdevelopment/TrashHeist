namespace Input
{
  using System.Collections.Generic;

  public interface IActionSubscriber
  {
    void QueueActions(List<ActionEvent> actionEvents);
  }
}