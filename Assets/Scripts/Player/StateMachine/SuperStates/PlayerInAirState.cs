using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    private float _time;
    private bool _glideable = false;
    private bool _isGrounded { get { return Ctx.Character.isGrounded; } }
    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    { 
        IsRootState = true;
        InitialiseSubState();
    }
    public override void CheckSwitchStates()
    {
        if (_isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsJumping && _glideable)
        {
            SwitchState(Factory.Glide());
        }

    }

    public override void EnterState()
    {
        _time = Ctx.GlideDelay;
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
        Ctx.Timer(SetGlideState, ref _time);
    }

    public override void FixedUpdateState()
    {
        Gravity();
        CheckSwitchStates();
    }

    private void Gravity()
    {
        if (Mathf.Abs(Ctx.VerticalVelocity) < (Ctx.TerminalVelocity * Ctx.GravityCoef))
            Ctx.VerticalVelocity -= Time.deltaTime * Ctx.GravityCoef;

        Ctx.Character.Move(new Vector3(0, Ctx.VerticalVelocity, 0));
    }

    private void SetGlideState()
    {
        _glideable = true;
    }
}
