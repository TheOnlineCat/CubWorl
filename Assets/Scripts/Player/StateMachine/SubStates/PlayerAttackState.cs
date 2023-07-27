using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAttackState : PlayerBaseState
{
    bool _refreshed = false;
    private float _curSmoothVelocity;
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        InitialiseSubState();
    }

    public override void CheckSwitchStates()
    {
        /*if (Ctx.IsMoving)
        {
            SwitchState(Factory.Run());
        }
        else
        {
            SwitchState(Factory.Idle());
        }*/
    }

    public override void EnterState()
    {
        Animator animator = Ctx.GetComponent<Animator>();
        if (Ctx.playerInput.Clicked)
        {
            animator.SetTrigger("Attack");
            Ctx.playerInput.Clicked = false;
        }
        Ctx.CurrentCombo += 1;
        Ctx.StartCoroutine(AttackStreak());
        Ctx.StartCoroutine(Recover());
    }

    public override void ExitState()
    {
        //Ctx.EnterCoroutine = Slow();
        //Ctx.ExitCoroutine = Recover();
        //Ctx.EnterCoroutine = AttackStreak();
        
    }

    public override void FixedUpdateState()
    {
        float angleSmooth = Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, Ctx.cam.transform.eulerAngles.y, ref _curSmoothVelocity, 0.025f);
        Ctx.transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);
        //Ctx.transform.rotation = Quaternion.Euler(0, Ctx.cam.transform.eulerAngles.y, 0);
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

    private IEnumerator AttackStreak()
    {
        yield return new WaitForSecondsRealtime(3);
    }

    private IEnumerator Recover()
    {
        Ctx.Speed *= Ctx.RecoveryCoef;
        yield return new WaitForSecondsRealtime(Ctx.RecoveryTime);
        Ctx.Speed *= 1 / Ctx.RecoveryCoef;
    }

}
