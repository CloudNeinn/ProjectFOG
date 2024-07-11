using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool HookInput { get; private set; }
    public bool CrouchInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool BlockInput { get; private set; }
    public bool SwitchRealmsInput { get; private set; }
    public bool DiveInput { get; private set; }
    public bool Use1Input { get; private set; }
    public bool Use2Input { get; private set; }
    public bool MenuToggleInput { get; private set; }


    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _hookAction;
    private InputAction _crouchAction;
    private InputAction _attackAction;
    private InputAction _blockAction;
    private InputAction _switchRealmsAction;
    private InputAction _diveAction;
    private InputAction _use1Action;
    private InputAction _use2Action;
    private InputAction _menuToggleAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        _playerInput = GetComponent<PlayerInput>();


        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _dashAction = _playerInput.actions["Dash"];
        _hookAction = _playerInput.actions["Hook"];
        _crouchAction = _playerInput.actions["Crouch"];
        _attackAction = _playerInput.actions["Attack"];
        _blockAction = _playerInput.actions["Block"];
        _switchRealmsAction = _playerInput.actions["SwitchRealms"];
        _diveAction = _playerInput.actions["Dive"];
        _use1Action = _playerInput.actions["Use1"];
        _use2Action = _playerInput.actions["Use2"];
        _menuToggleAction = _playerInput.actions["MenuToggle"];
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        JumpInput = _jumpAction.WasPressedThisFrame();
        DashInput = _dashAction.WasPressedThisFrame();
        HookInput = _hookAction.WasPressedThisFrame();
        CrouchInput = _crouchAction.WasPressedThisFrame();
        AttackInput = _attackAction.WasPressedThisFrame();
        BlockInput = _blockAction.WasPressedThisFrame();
        SwitchRealmsInput = _switchRealmsAction.WasPressedThisFrame();
        DiveInput = _diveAction.WasPressedThisFrame();
        Use1Input = _use1Action.WasPressedThisFrame();
        Use2Input = _use2Action.WasPressedThisFrame();
        MenuToggleInput = _menuToggleAction.WasPressedThisFrame();
    }
}
