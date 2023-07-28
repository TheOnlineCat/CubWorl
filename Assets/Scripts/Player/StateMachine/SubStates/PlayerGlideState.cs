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

    private float _initVelocity;
    private float _initVertical;

    private Vector3 _charRot { get { return (Ctx.transform.eulerAngles); } }
    private Vector2 _movementInput { get { return Ctx.playerInput.Movement; } }
    private bool _isGrounded { get { return Ctx.Character.isGrounded; } }

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
        if (_isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        if(Ctx.IsJumping)
        {
            //SwitchState(Factory.InAir());
        }
    }

    public override void EnterState()
    {
        //_initVelocity = (new Vector2(Ctx.Character.velocity.x, Ctx.Character.velocity.z).magnitude * 10) + Ctx.VerticalVelocity;
        _initVertical = Ctx.VerticalVelocity;
        Debug.Log(_initVelocity);
    }

    public override void ExitState()
    {
        Ctx.transform.rotation = Quaternion.Euler(0f, Ctx.transform.rotation.y, 0f);
    }

    public override void FixedUpdateState()
    {
        Gravity();
        Glide();
        _curLerp += Time.deltaTime * 0.5f;
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

        float tiltDirection = _movementInput.magnitude * -Mathf.Sign(_curTurnVelocity) * _tilt;
        float tiltSmooth = Mathf.SmoothDampAngle(_charRot.z, tiltDirection, ref _curTiltVelocity, 0.3f, Ctx.GlideTurnSpeed);

        Ctx.transform.rotation = Quaternion.Euler(0f, turnSmooth, 0f) * Quaternion.Euler(0, 0, tiltSmooth);
            

        Vector3 movement = Quaternion.Euler(0f, turnSmooth, 0f) * Vector3.forward;
        float curSpeed = Mathf.Lerp(Ctx.InitialVelocity.magnitude, Ctx.GlideSpeed, Mathf.Pow(_curLerp, 3f) );
        movement *= curSpeed;
        movement *= Time.deltaTime;

        Ctx.Character.Move(movement + (Vector3.down * Time.deltaTime));

    }
    private void Gravity()
    {
        Ctx.VerticalVelocity = Mathf.SmoothStep(_initVertical, -Ctx.GlideCoef, Mathf.Pow(_curLerp, 2f));

        Ctx.Character.Move(new Vector3(0, Ctx.VerticalVelocity, 0));
    }
}
