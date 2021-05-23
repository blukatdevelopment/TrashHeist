namespace AI 
{
    using Input;
    using Godot;
    using System.Collections.Generic;
    using Global;
    using Actor;

    // Yes, it's tightly coupled to the human. Not making a robust AI framework in this gamejam.
    // Gonna cram a bunch of stuff in this one file. If you're using this as a reference, break it out
    // and completely decouple it.
    public class HumanAI
    {
        public enum States
        {
            Idle,
            Pursuit
        }
        private States currentState;

        private HumanBody body;
        private Body target;

        public HumanAI(HumanBody body)
        {
            this.body = body;
            currentState = States.Idle;
        }

        public void Update(float delta)
        {
            switch(currentState)
            {
                case States.Idle:
                    LookForTarget();
                break;
                case States.Pursuit:
                    Pursue();
                break;
            }
        }

        private void PerformAction(ActionEnum action)
        {
            body.QueueActions(new List<ActionEvent>
                                {
                                    new ActionEvent(action)
                                });
        }

        public void Transition(States nextState)
        {
            if(currentState == States.Idle && nextState == States.Pursuit)
            {
                GD.Print("Pursuing!");
                PerformAction(ActionEnum.MoveUp);
            }
            if(currentState == States.Pursuit && nextState == States.Idle)
            {
                GD.Print("Ending pursuit");
                PerformAction(ActionEnum.MoveUpEnd);
            }
            currentState = nextState;
        }

        private void LookForTarget()
        {
            target = Main.Player.body;
            if(target == null)
            {
                return;
            }
            float distance = target.Transform.origin.DistanceTo(body.Transform.origin);
            if(distance < Constants.HumanSightRadius)
            {
                Transition(States.Pursuit);
            }
        }

        private void Pursue()
        {
            if(target == null)
            {
                Transition(States.Idle);
                return;
            }

            AimAt(target.Transform.origin);
            CatchRaccoon();
            PerformAction(ActionEnum.Jump);
        }

        private void AimAt(Vector3 point)
        {
            body.Transform = body.Transform.LookingAt(point, Constants.Up());            
        }

        private void CatchRaccoon()
        {
            float distance = target.Transform.origin.DistanceTo(body.Transform.origin);
            if(distance < Constants.HumanCatchRadius)
            {
                Main.Game.GameOver();
            }
        }
    }
}