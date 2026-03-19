using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public int MaxHealth = 3;

	private int currentHealth;

	public override void _Ready()
	{
		currentHealth = MaxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		GD.Print($"Enemy took {damage} damage. Health: {currentHealth}");

		if (currentHealth <= 0)
		{
			QueueFree();
		}
	}
}
