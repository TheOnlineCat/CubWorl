using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;

public class PlayerGlideState : PlayerBaseState
{
    private float _tilt = 30;

    private float _initSpeed;
    private float _initVertical;
    private float _glideTerminalVelocity;

    private Vector3 _charRot { get { return (Ctx.transform.eulerAngles); } }
    private Vector2 _movementInput { get { return Ctx.playerInput.Movement; } }

    #region refs
    private float _curLerp;
    private float _curSpeedVelocity;
    private float _curTurnVelocity;
    private float _curTiltVelocity;
    #endregion
    
    public PlayerGlideState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded())
        {
            SwitchState(Factory.Grounded());
        }
        if(Ctx.IsJumping)
        {
            SwitchState(Factory.InAir());
        }
    }

    public override void EnterState()
    {
        _initSpeed = Ctx.Rigidbody.velocity.magnitude;
        _initVertical = Ctx.VerticalVelocity;
        _glideTerminalVelocity = -Ctx.TerminalVelocity * Ctx.GlideTerminalCoef;
        Ctx.playerInput.Jump = false;
    }

    public override void ExitState()
    {
        Ctx.transform.rotation = Quaternion.Euler(0f, Ctx.transform.rotation.y, 0f);
        Ctx.playerInput.Jump = false;
    }

    public override void FixedUpdateState()
    {
        Gravity();
        Glide();
        _curLerp += Time.deltaTime;
    }

    public override void InitialiseSubState()
    {
        if (Ctx.IsMoving)
        {   

        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    private void Glide()
    {
        float angleDirection = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg;
            
        if (_movementInput.y > 0)
        {
            angleDirection += Ctx.cam.transform.eulerAngles.y;
        }
        else
        {
            angleDirection += _charRot.y;
        }
        float turnSmooth = Mathf.SmoothDampAngle(_charRot.y, angleDirection, ref _curTurnVelocity, 0.3f, Ctx.GlideTurnSpeed);

        float tiltVelocity = _movementInput.magnitude * (-_curTurnVelocity/Ctx.GlideTurnSpeed) * _tilt;
        float tiltSmooth = Mathf.SmoothDampAngle(_charRot.z, tiltVelocity, ref _curTiltVelocity, 0.3f, Ctx.GlideTurnSpeed);

        Ctx.transform.rotation = Quaternion.Euler(0f, turnSmooth, 0f) * Quaternion.Euler(0, 0, tiltSmooth);
            

        Vector3 movement = Quaternion.Euler(0f, turnSmooth, 0f) * Vector3.forward;
        float curSpeed = Mathf.Lerp(_initSpeed, Ctx.GlideSpeed, Mathf.Pow(_curLerp, 0.9f) );
        movement *= curSpeed;
        movement *= Time.deltaTime;

        MovePosition(Ctx.transform.position + movement);
        //Ctx.Character.Move(movement + (Vector3.down * Time.deltaTime));

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

    private void Gravity()
    {
        Ctx.VerticalVelocity = Mathf.SmoothStep(_initVertical, _glideTerminalVelocity, Mathf.Log(_curLerp, 3f)+1);

        Ctx.Rigidbody.AddForce(new Vector3(0, Ctx.VerticalVelocity, 0), ForceMode.VelocityChange);
    }
}
