using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStateMachine : MonoBehaviour
{
    public Vector3 vel;
    public string dump;
    public string curState;
    //Reference to Camera;
    public Camera cam;

    //Player Attributes
    [Header("Jump And Fall Behaviour")]
    public float GravityCoef = 2;
    public float JumpForce = 0.7f;
    public float TerminalVelocity = 2;
    
    [Header("Move Behaviour")]
    public float Speed = 20.0f;
    public float TurnSmoothTime = 0.1f;

    [Header("Attack Behaviour")]
    public float RecoveryCoef = 0.5f;
    public float RecoveryTime = 0.5f;

    [Header("Glide Behaviour")]
    public float GlideDelay = 1;
    public float GlideSpeed = 50;
    public float GlideTurnSpeed = 90;
    public float GlideTerminalCoef = 0.2f;

    //StateMachine
    internal PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    //Components
    private Rigidbody _rigidbody;
    private CharacterController _character;
    private Collider _collider;

    //StateMachine Variables
    private bool _isMoving;
    private bool _isJumping;
    [SerializeField]
    internal float _verticalVelocity;
    private int _currentCombo;

    [Header("Dependencies")]
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

    public void Timer(Action callback, ref float time)
    {
        time -= Time.deltaTime;
        if (time <= 0) callback?.Invoke();

    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _character = GetComponent<CharacterController>();
        _collider = GetComponent<Collider>();
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        InputHandler();
        
        _currentState.UpdateStates();
        curState = _currentState.GetType().Name;
        dump = Character.velocity.y.ToString();
        
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
        vel = Character.velocity;
    }

    private void InputHandler()
    {
        _isMoving = playerInput.Movement.magnitude > 0;
        _isJumping = playerInput.Jump;
    }

    public Boolean IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y + 0.1f);
    }
}
