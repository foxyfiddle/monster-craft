using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export]
	public float MaxSpeed = 200f;
	
	[Export]
	public float Acceleration = 1200f;
	
	[Export]
	public float Friction = 1400f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Vector2.Zero;

		if (Input.IsActionPressed("ui_right"))
			inputDirection.X += 1;

		if (Input.IsActionPressed("ui_left"))
			inputDirection.X -= 1;

		if (Input.IsActionPressed("ui_down"))
			inputDirection.Y += 1;

		if (Input.IsActionPressed("ui_up"))
			inputDirection.Y -= 1;

		inputDirection = inputDirection.Normalized();
		
		if (inputDirection != Vector2.Zero)
		{
			Vector2 targetVelocity = inputDirection * MaxSpeed;
			Velocity = Velocity.MoveToward(targetVelocity, Acceleration * (float)delta);
		}
		else
		{
			Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
		}
		
		MoveAndSlide();
	}
}
