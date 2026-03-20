using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export] public float MaxSpeed = 200f;
	[Export] public float Acceleration = 1200f;
	[Export] public float Friction = 1400f;

	[Export] public float DashSpeed = 500f;
	[Export] public float DashDuration = 0.2f;
	[Export] public float DashCooldown = 0.5f;

	[Export] public int AttackDamage = 1;
	[Export] public float AttackOffset = 15f;

	private Area2D _attackArea;

	private bool _isDashing = false;
	private float _dashTimer = 0f;
	private float _dashCooldownTimer = 0f;

	private Vector2 _dashDirection = Vector2.Zero;
	private Vector2 _facingDirection = Vector2.Right;
	private Vector2 _inputDirection = Vector2.Zero;

	public override void _Ready()
	{
		_attackArea = GetNode<Area2D>("AttackArea");
		UpdateAttackAreaPosition();
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;

		ReadInput();
		UpdateFacingDirection();
		UpdateTimers(deltaF);
		HandleDash(deltaF);
		HandleAttack();

		MoveAndSlide();
	}

	private void ReadInput()
	{
		_inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
	}

	private void UpdateFacingDirection()
	{
		if (_inputDirection != Vector2.Zero)
		{
			_facingDirection = GetCardinalDirection(_inputDirection);
			UpdateAttackAreaPosition();
		}
	}

	private void UpdateTimers(float delta)
	{
		if (_dashCooldownTimer > 0f)
		{
			_dashCooldownTimer -= delta;
		}

		if (_isDashing)
		{
			_dashTimer -= delta;

			if (_dashTimer <= 0f)
			{
				_isDashing = false;
			}
		}
	}

	private void HandleDash(float delta)
	{
		if (_isDashing)
		{
			Velocity = _dashDirection * DashSpeed;
			return;
		}

		if (CanStartDash())
		{
			StartDash();
			Velocity = _dashDirection * DashSpeed;
			return;
		}

		ApplyMovement(delta);
	}

	private bool CanStartDash()
	{
		return Input.IsActionJustPressed("dash")
			&& _dashCooldownTimer <= 0f
			&& _inputDirection != Vector2.Zero;
	}

	private void StartDash()
	{
		_isDashing = true;
		_dashTimer = DashDuration;
		_dashCooldownTimer = DashCooldown;
		_dashDirection = _inputDirection;
	}

	private void ApplyMovement(float delta)
	{
		if (_inputDirection != Vector2.Zero)
		{
			Vector2 targetVelocity = _inputDirection * MaxSpeed;
			Velocity = Velocity.MoveToward(targetVelocity, Acceleration * delta);
		}
		else
		{
			Velocity = Velocity.MoveToward(Vector2.Zero, Friction * delta);
		}
	}

	private void HandleAttack()
	{
		if (Input.IsActionJustPressed("attack"))
		{
			PerformAttack();
		}
	}

	private void PerformAttack()
	{
		var bodies = _attackArea.GetOverlappingBodies();

		foreach (Node body in bodies)
		{
			if (body is Enemy enemy)
			{
				enemy.TakeDamage(AttackDamage);
			}
		}

		GD.Print("Attack!");
	}

	private void UpdateAttackAreaPosition()
	{
		_attackArea.Position = _facingDirection * AttackOffset;
	}

	private Vector2 GetCardinalDirection(Vector2 inputDirection)
	{
		if (Mathf.Abs(inputDirection.X) > Mathf.Abs(inputDirection.Y))
		{
			return inputDirection.X > 0 ? Vector2.Right : Vector2.Left;
		}

		return inputDirection.Y > 0 ? Vector2.Down : Vector2.Up;
	}
}
