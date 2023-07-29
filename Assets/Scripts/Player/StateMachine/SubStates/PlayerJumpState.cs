using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }
    public override void CheckSwitchStates()
    {

    }

    public override void EnterState()
    {
        //Ctx.Rigidbody.AddForce(new Vector3(0, Ctx.playerInput.movement.y * Ctx.JumpForce, 0), ForceMode.Impulse);
        Ctx.VerticalVelocity = Ctx.JumpForce;
        Ctx.playerInput.Jump = false;
    }

    public override void ExitState()
    {

    }

    public override void InitialiseSubState()
    {

    }

    public override void UpdateState()
    {
        SwitchState(Factory.InAir());
    }

    public override void FixedUpdateState()
    {
    }
}
