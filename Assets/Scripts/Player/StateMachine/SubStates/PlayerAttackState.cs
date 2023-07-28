using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAttackState : PlayerBaseState
{
    private bool _refreshed = true;
    private float _oldSpeed;
    private float _time;
    private float _curSmoothVelocity;
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        InitialiseSubState();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsMoving)
        {
            SwitchState(Factory.Run());
        }
        else
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void EnterState()
    {
        _oldSpeed = Ctx.Speed;
    }

    public override void ExitState()
    {
        _oldSpeed = Ctx.Speed;

    }

    public override void FixedUpdateState()
    {
        if (!_refreshed)
        {
            float angleSmooth = Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, Ctx.cam.transform.eulerAngles.y, ref _curSmoothVelocity, 0.025f);
            Ctx.transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);
        }
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
        if (Ctx.playerInput.Clicked)
        {
            Attack();
        }
        Ctx.Timer(OnTimeOut, ref _time);
    }

    private void OnTimeOut()
    {
        Ctx.CurrentCombo = 0;
        CheckSwitchStates();
    }
    private void Attack()
    {
        Animator animator = Ctx.GetComponent<Animator>();
        if (Ctx.playerInput.Clicked && _refreshed)
        {
            _time = 3;
            Ctx.CurrentCombo += 1;
            animator.SetTrigger("Attack");
            Ctx.playerInput.Clicked = false;
            Ctx.StartCoroutine(Recover());
        }
        else Ctx.playerInput.Clicked = false;
    }

    private IEnumerator Recover()
    {
        _refreshed = false;
        Ctx.Speed = _oldSpeed * Ctx.RecoveryCoef;
        yield return new WaitForSeconds(Ctx.RecoveryTime);
        _refreshed = true;
        Ctx.Speed = _oldSpeed;
    }

}
