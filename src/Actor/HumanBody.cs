namespace Actor
{
	using Input;
	using Godot;
	using Global;
	using AI;

	public class HumanBody: Body
	{
		[Export]
		public float movementSpeed;
		[Export]
		public Vector3 movement;
		[Export]
		public Vector3 aimMagnitude;
		[Export]
		public Vector3 jumpThrust;
		[Export]
		public float gravityStrength;
		[Export]
		public bool jumpReady;

		public CollisionShape torso;
		public HumanAI agent;

		public void AssignActor(Actor actor, string headNodePath, string torsoPath)
		{
		  this.actor = actor;
		  jumpThrust = new Vector3();
		  head = FindNode(headNodePath) as Spatial;
		  torso = FindNode(torsoPath) as CollisionShape;
		  agent = new HumanAI(this);
		  Resume();
		}

		public override void HandleAction(ActionEvent actionEvent)
		{
			aimMagnitude = new Vector3();
			HandlePause(actionEvent);

			if(!paused)
			{
				HandleStrafing(actionEvent);
				HandleAiming(actionEvent);
				HandleJumping(actionEvent);
			}
		}
	
		public override void PostEventsUpdate()
		{
			if(!paused)
			{
				Move(movement);
				Move(jumpThrust);
				ApplyGravity();
				UpdateJumpThrust();
				Aim(aimMagnitude);
				aimMagnitude = new Vector3();
				agent.Update(delay);
			}
		}
	
		public void Aim(Vector3 aimMagnitude)
		{
			Vector3 bodyRotation = RotationDegrees;
			// X axis is reversed for whatever reason
			bodyRotation.y -= aimMagnitude.x;
			RotationDegrees = bodyRotation;
	  
			Vector3 headRotation = head.RotationDegrees;
	  
			headRotation.x -= aimMagnitude.y;
			headRotation.x = Utility.Clamp(headRotation.x, Constants.MinRaccoonLookAngle, Constants.MaxRaccoonLookAngle);
			head.RotationDegrees = headRotation;
		}
	
		public override void Move(Vector3 direction)
		{
			Transform current = Transform;
			Transform destination = current;
			destination.Translated(direction);
	  
			Vector3 delta = destination.origin - current.origin;
			KinematicCollision collision = MoveAndCollide(delta);
			if(collision != null && collision.Position.y < this.Transform.origin.y)
			{
				jumpReady = true;
			}
		}
	
		public override void Pause()
		{
			paused = true;
			Input.SetMouseMode(Input.MouseMode.Visible);
		}
	
		public override void Resume()
		{
			paused = false;
			Input.SetMouseMode(Input.MouseMode.Captured);
		}
	
		protected void HandleStrafing(ActionEvent actionEvent)
		{
			switch(actionEvent.action)
			{
				case ActionEnum.MoveUp:
					movement = new Vector3(movement.x, movement.y, -movementSpeed);
				break;
				case ActionEnum.MoveUpEnd:
					movement = new Vector3(movement.x, movement.y, 0f);
				break;
				case ActionEnum.MoveDown:
					movement = new Vector3(movement.x, movement.y, movementSpeed);
				break;
				case ActionEnum.MoveDownEnd:
					movement = new Vector3(movement.x, movement.y, 0f);
				break;
				case ActionEnum.MoveRight:
					movement = new Vector3(movementSpeed, movement.y, movement.z);
				break;
				case ActionEnum.MoveRightEnd:
					movement = new Vector3(0f, movement.y, movement.z);
				break;
				case ActionEnum.MoveLeft:
					movement = new Vector3(-movementSpeed, movement.y, movement.z);
				break;
				case ActionEnum.MoveLeftEnd:
					movement = new Vector3(0f, movement.y, movement.z);
				break;
			}
		}
	
		protected void HandleAiming(ActionEvent actionEvent)
		{
			switch(actionEvent.action)
			{
				case ActionEnum.AimHorizontal:
					aimMagnitude.x = actionEvent.magnitude;
				break;
				case ActionEnum.AimVertical:
					aimMagnitude.y = actionEvent.magnitude;
				break;
			}
		}
	
		protected void HandlePause(ActionEvent actionEvent)
		{
		  if(actionEvent.action == ActionEnum.TogglePlayerPause)
		  {
			if(paused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		  }
	
		}

		protected void HandleJumping(ActionEvent actionEvent)
		{
			Vector3 jump = Constants.JumpThrust();
			if(jumpReady && actionEvent.action == ActionEnum.Jump)
			{
				jumpThrust = jump;
				jumpReady = false;
				
			}
			if(actionEvent.action == ActionEnum.JumpEnd)
			{
				jumpThrust = new Vector3(0f, 0f, 0f);
			}
		}

		protected void ApplyGravity()
		{
			if(jumpThrust.y > 0f)
			{
				gravityStrength = 1f;
				return;
			}
			Vector3 velocity = Constants.Gravity() * gravityStrength;
			gravityStrength += Constants.GravityAcceleration;
			velocity.x = Utility.Clamp(velocity.x, Constants.TerminalVelocity, 0f);
			Move(velocity);
		}

		protected void UpdateJumpThrust()
		{
			if(jumpThrust.y == 0f)
			{
				return;
			}

			jumpThrust *= Constants.JumpDecay;
			if(jumpThrust.y <= Constants.MinimumJump)
			{
				jumpThrust = new Vector3(0f, 0f, 0f);
			}
		}
	}
}
