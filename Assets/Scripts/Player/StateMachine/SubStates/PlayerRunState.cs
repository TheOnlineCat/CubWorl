using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    private float _curSmoothVelocity;
    private Vector2 _movementInput { get { return Ctx.playerInput.Movement; } }
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    { }

    public override void CheckSwitchStates()
    {
        if(!Ctx.IsMoving)
        {
            SwitchState(Factory.Idle());
        }
        if(Ctx.playerInput.Clicked)
        {
            SwitchState(Factory.Attack());
        }
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {

    }

    public override void InitialiseSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        MovementControl();
    }

    void MovementControl()
    {
        if (_movementInput.magnitude != 0)
        {
            float angleDirection = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg;
            angleDirection += Ctx.cam.transform.eulerAngles.y;

            float angleSmooth = Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, angleDirection, ref _curSmoothVelocity, Ctx.TurnSmoothTime);
            Ctx.transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);

            Vector3 movement = Quaternion.Euler(0f, angleDirection, 0f) * Vector3.forward;
            movement *= Ctx.Speed;
            movement *= Time.deltaTime;

            Ctx.Character.Move(movement + (Vector3.down * Time.deltaTime));
            
        }
    }
}
