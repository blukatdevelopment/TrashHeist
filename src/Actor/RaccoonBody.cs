namespace Actor
{
	using Input;
	using Godot;
	using Global;
	using Menu;
	using Audio;
	using System;

	public class RaccoonBody : Body
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
		public bool jumpReady;
		public float interactDelay;
		public CollisionShape torso;
		private Speaker speaker;
		private Speaker footSpeaker;
	
		public void AssignActor(Actor actor, bool addCamera, string headNodePath, string torsoPath, Vector3 cameraPosition)
		{
		  this.actor = actor;
		  jumpThrust = new Vector3();
		  head = FindNode(headNodePath) as Spatial;
		  if(addCamera)
		  {
			camera = new Camera();
			head.AddChild(camera);
			camera.Transform = camera.Transform.Translated(cameraPosition);
			camera.Far = 900f;
			speaker = new Speaker(camera);
			footSpeaker = new Speaker(camera);
		  }
		  torso = FindNode(torsoPath) as CollisionShape;
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
				HandleInteraction(actionEvent);
			}
		}
	
		public override void PostEventsUpdate()
		{
			if(!paused)
			{
				Move(movement);
				ApplyWalkSound();
				Move(jumpThrust);
				ApplyGravity();
				UpdateJumpThrust();
				Aim(aimMagnitude);
				AimCamera();
				aimMagnitude = new Vector3();
				UpdateInteractDelay();
			}
		}

		private void ApplyWalkSound()
		{
			bool moving = movement.x != 0f || movement.z != 0f;
			if(moving && !footSpeaker.Playing)
			{
				footSpeaker.PlaySound(SoundEnum.RaccoonWalk, true, Constants.SoundEffectsVolume);
			}
			else if (!moving && footSpeaker.Playing)
			{
				footSpeaker.StopSound();
			}
		}

		private void UpdateInteractDelay()
		{
			interactDelay += delay;
		}

		public void AimCamera()
		{
			camera.Transform = camera.Transform.LookingAt(torso.Transform.origin, Constants.Up());
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
			Node colliderNode = collision?.Collider as Node;
			if(colliderNode != null && colliderNode.IsInGroup(Constants.HazardGroup))
			{
				Main.Game.GameOver();
			}
		}
	
		public override void Pause()
		{
			paused = true;
			Input.SetMouseMode(Input.MouseMode.Visible);
			Main.Game.menuManager.SwitchMenu(MenuManager.Menus.Pause);
			speaker.StopSound();
			footSpeaker.StopSound();
		}
	
		public override void Resume()
		{
			paused = false;
			Input.SetMouseMode(Input.MouseMode.Captured);
			Main.Game.menuManager.SwitchMenu(MenuManager.Menus.None);
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

		protected void HandleInteraction(ActionEvent actionEvent)
		{
			if(actionEvent.action != ActionEnum.InteractEnd || interactDelay < Constants.InteractDelay)
			{
				return;
			}
			Vector3 startPoint = Transform.origin;
			Vector3 endPoint = ForwardPoint(Constants.InteractDistance);
			Node resultNode = Utility.Raycast(startPoint, endPoint, GetWorld()) as Node;
			if(resultNode == null)
			{
				return;
			}
			if(resultNode.IsInGroup(Constants.TrashCanGroup))
			{
				GD.Print("Interacting with trash can!");
				Main.TrashGathered++;
				Main.Game.AlertHumans();
			}
		}

		protected Vector3 ForwardPoint(float distance)
		{
			Vector3 start = Transform.origin;
			Vector3 end = -Transform.basis.z;
			end *= distance;
			end += start;
			return end;
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
				speaker.PlaySound(SoundEnum.RaccoonJump, false, Constants.SoundEffectsVolume);
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
