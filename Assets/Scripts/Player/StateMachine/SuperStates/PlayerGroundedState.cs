using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private bool _isGrounded { get { return Ctx.Character.isGrounded; } }
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitialiseSubState();
    }

    public override void CheckSwitchStates()
    {
        if (!_isGrounded)
        {
            SwitchState(Factory.InAir());
        }
        if(Ctx.IsJumping)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void EnterState()
    {
        Ctx.VerticalVelocity = 0;
    }

    public override void ExitState()
    {
        
    }

    public override void InitialiseSubState()
    {
        if (Ctx.IsMoving)
        {
            SetSubState(Factory.Run());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
    }
}
