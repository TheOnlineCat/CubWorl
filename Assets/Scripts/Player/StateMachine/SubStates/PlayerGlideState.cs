using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerGlideState : PlayerBaseState
{
    public PlayerGlideState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsMoving)
        {

        }
        
    }

    public override void EnterState()
    {
       
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void InitialiseSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
