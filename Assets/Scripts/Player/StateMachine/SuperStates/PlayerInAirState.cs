using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    //private float _distToGround;
    //private float verticalVelocity;
    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    { 
        IsRootState = true;
        InitialiseSubState();
    }
    public override void CheckSwitchStates()
    {
        if (IsGrounded())
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        //_distToGround = Ctx.GetComponent<Collider>().bounds.extents.y;
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
        if (Mathf.Abs(Ctx.VerticalVelocity) < Ctx.TerminalVelocity)
            Ctx.VerticalVelocity -= Time.deltaTime * Ctx.GravityCoef;
        
        Ctx.Character.Move( new Vector3(0, Ctx.VerticalVelocity, 0) );
    }

    Boolean IsGrounded()
    {
        return Ctx.Character.isGrounded;
        //return Physics.Raycast(Ctx.transform.position, Vector3.down, _distToGround + 0.1f);
    }
}
