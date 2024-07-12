using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance;

    [field: SerializeField] public Vector2 MoveInput { get; private set; }
    [field: SerializeField] public bool JumpInput { get; private set; }
    [field: SerializeField] public bool DashInput { get; private set; }
    [field: SerializeField] public bool HookInput { get; private set; }
    [field: SerializeField] public bool CrouchInput { get; private set; }
    [field: SerializeField] public bool AttackInput { get; private set; }
    [field: SerializeField] public bool BlockInput { get; private set; }
    [field: SerializeField] public bool SwitchRealmsInput { get; private set; }
    [field: SerializeField] public bool GlideInput { get; private set; }
    [field: SerializeField] public bool Use1Input { get; private set; }
    [field: SerializeField] public bool Use2Input { get; private set; }
    [field: SerializeField] public bool MenuToggleInput { get; private set; }


    public PlayerInput _playerInput { get; private set; }
    public InputAction _moveAction { get; private set; }
    public InputAction _jumpAction { get; private set; }
    public InputAction _dashAction { get; private set; }
    public InputAction _hookAction { get; private set; }
    public InputAction _crouchAction { get; private set; }
    public InputAction _attackAction { get; private set; }
    public InputAction _blockAction { get; private set; }
    public InputAction _switchRealmsAction { get; private set; }
    public InputAction _glideAction { get; private set; }
    public InputAction _use1Action { get; private set; }
    public InputAction _use2Action { get; private set; }
    public InputAction _menuToggleAction { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        _glideAction = _playerInput.actions["Glide"];
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
        GlideInput = _glideAction.WasPressedThisFrame();
        Use1Input = _use1Action.WasPressedThisFrame();
        Use2Input = _use2Action.WasPressedThisFrame();
        MenuToggleInput = _menuToggleAction.WasPressedThisFrame();
    }
}
