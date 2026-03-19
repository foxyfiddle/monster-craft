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

	[Export]
	public float DashSpeed = 500f;

	[Export]
	public float DashDuration = 0.2f;

	[Export]
	public float DashCooldown = 0.5f;

	private bool isDashing = false;
	private float dashTimer = 0f;
	private float DashCooldownTimer = 0f;
	private Vector2 dashDirection =Vector2.Zero;


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

		// Update timers
		DashCooldownTimer -= (float)delta;

		if (isDashing)
		{
			dashTimer -= (float)delta;
			Velocity = dashDirection * DashSpeed;

			if (dashTimer <= 0)
			{
				isDashing = false;
			}
		}
		else
		{
			// Start dash
			if (Input.IsActionJustPressed("dash") && DashCooldownTimer <= 0 && inputDirection != Vector2.Zero)
			{
				isDashing = true;
				dashTimer = DashDuration;
				DashCooldownTimer = DashCooldown;
				dashDirection = inputDirection;
			}

			// Normal movement
			if (inputDirection != Vector2.Zero)
			{
				Vector2 targetVelocity = inputDirection * MaxSpeed;
				Velocity = Velocity.MoveToward(targetVelocity, Acceleration * (float)delta);
			}
			else
			{
				Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
			}
		}
		
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
