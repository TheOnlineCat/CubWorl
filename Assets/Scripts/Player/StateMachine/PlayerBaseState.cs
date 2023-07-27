using System.Collections;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    #region Getters and Setters

    protected bool IsRootState { set { _isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return _ctx; } }
    protected PlayerStateFactory Factory { get { return _factory; } }

    #endregion

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitialiseSubState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public void UpdateStates() 
    {
        UpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.FixedUpdateStates();
        }
    }


    protected void SwitchState(PlayerBaseState newState) { 
        //Current State Exit
        ExitState();

        //Enter New State
        newState.EnterState();

        if (_isRootState)
        {
            //Switch Current State to New State
            _ctx.CurrentState = newState;
        } 
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }


    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        newSubState.EnterState();
    }


    protected IEnumerator StateCoroutine(IEnumerator enterCoroutine, IEnumerator exitCoroutine)
    {
        yield return Ctx.StartCoroutine(enterCoroutine);
        enterCoroutine = null;
        if (exitCoroutine != null)
        {
            Ctx.StartCoroutine(exitCoroutine);
            exitCoroutine = null;
        }
    }
}
