using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    
    

    private Vector3 _movementInput;
    private float _curSmoothVelocity;

    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    { }

    public override void CheckSwitchStates()
    {
        if(!Ctx.IsMoving)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Moving");
    }

    public override void ExitState()
    {

    }

    public override void InitialiseSubState()
    {

    }

    public override void UpdateState()
    {
        _movementInput = Ctx.playerInput.movement;
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        //Debug.Log("Moving");
        MovementControl();
    }

    void MovementControl()
    {
        Vector3 direction = new Vector3(_movementInput.x, 0, _movementInput.z);

        if (direction.magnitude != 0)
        {
            float angleDirection = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            angleDirection += Ctx.cam.transform.eulerAngles.y;

            float angleSmooth = Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, angleDirection, ref _curSmoothVelocity, Ctx.TurnSmoothTime);
            Ctx.transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);

            Vector3 movement = Quaternion.Euler(0f, angleDirection, 0f) * Vector3.forward;
            movement *= Ctx.Speed;
            movement *= Time.deltaTime;

            Ctx.Character.Move(movement);
            //MovePosition(Ctx.Rigidbody.position + movement);


        }
    }
    void MovePosition(Vector3 position)
    {
        Vector3 oldVel = Ctx.Rigidbody.velocity;
        //Get the position offset
        Vector3 delta = position - Ctx.Rigidbody.position;
        //Get the speed required to reach it next frame
        Vector3 vel = delta / Time.fixedDeltaTime;

        //If you still want gravity, you can do this
        vel.y = oldVel.y;

        //If you want your rigidbody to not stop easily when hit
        //This is however untested, and you should probably use a damper system instead, like using Smoothdamp but only keeping the velocity component
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = Mathf.Abs(oldVel.z) > Mathf.Abs(vel.z) ? oldVel.z : vel.z;

        Ctx.Rigidbody.velocity = vel;
    }
}
