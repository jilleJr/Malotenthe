﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System;

public abstract class RingWalker : RingObject
{
	[HideInInspector]
    public bool isFacingRight;
	public bool Grounded { get; protected set; }

	[Header("Grounded")]
	public LayerMask groundedRayMask = 1;
	public Vector2 groundedOffset;
	[Range(0, 1)]
	public float groundedRadius = 0.25f;

	[Header("Health")]
	public int health = 20;
	public int maxHealth = 20;
	public bool Dead { get; private set; }
	public float HealthPercentage { get { return maxHealth == 0 ? 0 : (float)health / maxHealth; } }

	private Rigidbody m_Body;
	public Rigidbody Body { get { return m_Body; } }

	protected virtual void Awake() {
		m_Body = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// Raycasts and checks if the object is touching ground.
	/// </summary>
	protected bool IsGrounded()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(groundedOffset), groundedRadius, groundedRayMask, QueryTriggerInteraction.Ignore);
		foreach (Collider col in colliders) {
			if (col.transform.IsChildOf(transform) == false)
				return true;
		}
		return false;
	}

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = IsGrounded() ? Color.red : Color.cyan;
		Gizmos.DrawWireSphere(transform.TransformPoint(groundedOffset), groundedRadius);
	}

	protected virtual void FixedUpdate()
	{
		// Physics update
		Grounded = IsGrounded();

		Vector3 position = Body.position;

		// Set position but with interpolation
		Body.MovePosition(RingPosition(position));

		// Look towards the center
		Body.MoveRotation(RingRotation(position));
	}


    public static void CalculateRotation(float inAngle, out float outAngle, out bool facingRight)
    {
        facingRight = inAngle < 90 || inAngle > 270;
        outAngle = facingRight ? inAngle : inAngle - 180;
    }

	public virtual void Damage(int damage)
	{
		health -= damage;
		Dead = health <= 0;
	}

}
