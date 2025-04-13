using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_ChargeState : ChargeState
{
    private Enemy3 enemy;
    private bool isJumping;

    public E3_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enemy3 enemy)
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        isJumping = true;

        // Calculate the direction to jump toward the player
        Vector2 jumpDirection = CalculateJumpDirection();

        // Apply the velocity to the Rigidbody2D
        enemy.SetVelocity(stateData.chargeSpeed, jumpDirection, enemy.facingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isJumping)
        {
            // Check if the enemy has landed
            if (enemy.CheckGround())
            {
                isJumping = false;
            }
        }
        else
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if (!isDetectingLedge || isDetectingWall)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
            else if (isChargeTimeOver)
            {
                if (isPlayerInMinAgroRange)
                {
                    stateMachine.ChangeState(enemy.playerDetectedState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.lookForPlayerState);
                }
            }
        }
    }

    private Vector2 CalculateJumpDirection()
    {
        // Get the player's position
        Vector2 playerPosition = enemy.GetPlayerPosition();

        // Calculate the direction vector toward the player
        Vector2 direction = (playerPosition - (Vector2)enemy.transform.position).normalized;

        // Scale the horizontal and vertical components
        float horizontalSpeed = stateData.chargeSpeed; // Horizontal speed toward the player
        float verticalSpeed = stateData.jumpHeight;   // Fixed vertical jump height

        // Combine horizontal and vertical components
        Vector2 jumpDirection = new Vector2(direction.x * horizontalSpeed, verticalSpeed);

        Debug.Log($"Jump Direction: {jumpDirection}, Player Position: {playerPosition}, Enemy Position: {enemy.transform.position}");
        return jumpDirection;
    }
}
