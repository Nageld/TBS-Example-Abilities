using System;
using System.Collections.Generic;
using Wizards.Interactive;
using Wizards.People;
using Wizards.TileSprite;

namespace AbilityExample.abilities;

public class Roundhouse : AbilitySO
{
	
	public override bool CanTargetPositionFrom(GridPosition _targetPosition, GridPosition _fromPosition)
	{
		if (_targetPosition == _fromPosition)
		{
			return false;
		}

		if (Helpers.IsAdjacent(_targetPosition, _fromPosition))
		{
			return IsAvailable()  && Dev.CanMeleeFromAndTo(_fromPosition, _targetPosition);
		}

		return false;

	}

	public override void GameplayEffect(GridPosition _position)
	{

		user.PlaySound(impactSound);
		foreach (var target in Helpers.GetAdjacentPositions(user.gridPosition))
		{
			_position = target;	
			TargetableObject targetableObject = Dev.SolidTargetAtPosition(_position, false);
			if (targetableObject != null)
			{
				targetableObject.TakeDamageAndKnockback(damage, knockback, user.transform.position, user, false);
			}
			Managers.Time.ChangeTimeScale(0.005f, 0.5f, 7f, 0.05f);
			Person person = targetableObject as Person;
			if (conditionToApply != null && person != null)
			{
				person.conditions.AddCondition(conditionToApply.SaveName, 1);
			}
		}

		CreateImpactEffectAt(_position);
		Managers.Turn.UnitIsActingFor(0.15f);
		screenShakeAmount = (float)damage * 0.5f;
		ScreenshakeOnUse();
	}

	public override void StartAction(GridPosition _position)
	{
		base.StartAction(_position);
		user.InstantFacePosition(_position);
		PlayUseSound();
		TriggerAnimation();
		CreateMuzzleFlash();
	}
	
	public override void WizardPreviewUseOnPosition(GridPosition _targetPosition, GridPosition _sourcePosition)
	{
		foreach (var target in Helpers.GetAdjacentPositions(_sourcePosition))
		{
			_targetPosition = target;
			if (_targetPosition != null && _sourcePosition != null && CanTargetPositionFrom(_targetPosition, _sourcePosition))
			{
				Managers.TileSprite.PlaceTileSprite(TileSpriteType.Cursor, _targetPosition, null, null, 1f);
				Managers.Cursor.Set(CursorManager.CursorType.action);
				SignalAbilityUsePreview();
				TargetableObject targetableObject = Dev.SolidTargetAtPosition(_targetPosition, false);
				if (targetableObject != null)
				{
					targetableObject.PreviewDamageAndKnockback(damage, knockback, _sourcePosition);
					SignalRetaliationPreviewForAttackOn(targetableObject);
				}
			}
		}
		
	}

	public Condition conditionToApply;
}
