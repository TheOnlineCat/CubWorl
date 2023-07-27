using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    //private float _distToGround;
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitialiseSubState();
    }

    public override void CheckSwitchStates()
    {
        if (!IsGrounded())
        {
            SwitchState(Factory.InAir());
        }
        if(Ctx.IsJumping)
        {
            Debug.Log("Jumping");
            SwitchState(Factory.Jump());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Grounded");
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

    public

    Boolean IsGrounded()
    {
        return Ctx.Character.isGrounded;
        //return Physics.Raycast(Ctx.transform.position, Vector3.down, _distToGround + 0.1f);
    }
}
