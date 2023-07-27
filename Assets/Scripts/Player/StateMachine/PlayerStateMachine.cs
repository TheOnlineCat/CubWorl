using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStateMachine : MonoBehaviour
{
    public float dump;
    public string curState;
    //Reference to Camera;
    public Camera cam;

    //Player Attributes
    public float GravityCoef = 2;
    public float TerminalVelocity = 2;
    public float JumpForce = 0.7f;
    public float TurnSmoothTime = 0.1f;
    public float Speed = 20.0f;
    public float RecoveryCoef = 0.5f;
    public float RecoveryTime = 0.5f;

    //StateMachine
    internal PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    //Components
    private Rigidbody _rigidbody;
    private CharacterController _character;

    //StateMachine Variables
    private bool _isMoving;
    private bool _isJumping;
    private float _verticalVelocity;
    [SerializeField]
    internal int _currentCombo;

    [SerializeField]
    internal PlayerInput playerInput;



    #region Getter Setter

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public CharacterController Character { get { return _character; } }

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public bool IsMoving { get { return _isMoving; } }
    public bool IsJumping { get { return _isJumping; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }
    public int CurrentCombo { get { return _currentCombo; } set { _currentCombo = value; } }

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _character = GetComponent<CharacterController>();
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        _currentState.UpdateStates();
        curState = _currentState.GetType().Name;
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    void InputHandler()
    {
        _isMoving = playerInput.Movement.magnitude > 0;
        _isJumping = playerInput.Jump > 0;
    }
}
