using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wizards.Effects;
using Wizards.Interactive;
using Wizards.People;

namespace AbilityExample.abilities;

public class FPush : AbilitySO
{
	private void OnDisable()
	{
		ClearStreak();
	}

	public override bool CanTargetPositionFrom(GridPosition _targetPosition, GridPosition _fromPosition)
	{
		return SwappableObjectAtPosition(_targetPosition) != null && _targetPosition != _fromPosition && IsAvailable();;
	}

	public override void CommitAbility(GridPosition _position)
	{
		TargetableObject targetableObject = Dev.SolidTargetAtPosition(_position, false);
		targetableObject.TakeDamageAndKnockback(damage, knockback, user.gridPosition, user, false);
		base.CommitAbility(_position);
	}

	protected virtual GameObject SwappableObjectAtPosition(GridPosition position)
	{
		TargetableObject targetableObject = Dev.SolidTargetAtPosition(position, false);
		bool flag = user.HasSpecialPerk("SwapWithObjects");
		if (targetableObject == null || !targetableObject.CanSwap)
		{
			if (flag)
			{
				TakeableCoverInteractible takeableCoverInteractible = Dev.InteractibleAtPosition<TakeableCoverInteractible>(position);
				if (takeableCoverInteractible != null)
				{
					return takeableCoverInteractible.gameObject;
				}
			}
		}
		else
		{
			Person person = targetableObject as Person;
			if ((!(person == null) && !person.conditions.HasCondition("Inorganic")) || flag)
			{
				return targetableObject.gameObject;
			}
		}
		return null;
	}

	public override void StartAction(GridPosition _position)
	{
		base.StartAction(_position);
		TriggerAnimation();
		user.PlaySound(immediateSound);
		CreateMuzzleStartEffect();
		ScreenshakeOnUse();
	}

	private void ClearStreak()
	{
		if (streakFX != null)
		{
			streakFX.Stop(true);
			streakFX = null;
		}
	}

	private ContinuousParticleEffect streakFX;
}