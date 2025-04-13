using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_StunState : StunState
{
    private Enemy3 enemy;

    public E3_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy3 enemy)
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // Apply knockback
        Vector2 knockbackDirection = new Vector2(enemy.lastDamageDirection, 1).normalized;
        enemy.SetVelocity(stateData.stunKnockbackSpeed, knockbackDirection, enemy.facingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.stunTime)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }
}
