using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Collider2D _collider;
    Rigidbody2D _rigidbody;
    Animator _animator;
    [SerializeField] FSM _fsm;


    // ================= Props ================-
    [SerializeField] FacingDirection direction;
    [SerializeField] float moveSpeed = 5.0f;
    bool isInteract = false;

    private InputAction toggleInventoryAction;
    private InputAction closeInventoryAction;

    // ================= value INPUT =================

    Vector2 input = new Vector2(0, 0);
    bool canMove = true;
    bool isUsingRemote = false;

    public static event Action<bool> IsRemoteUsed;

    #region Getter-Setter
    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }
    public FSM Fsm
    {
        get { return _fsm; }
        set { _fsm = value; }
    }
    #endregion

    private void OnEnable()
    {
        toggleInventoryAction.Enable();
        closeInventoryAction.Enable();
    }

    private void OnDisable()
    {
        toggleInventoryAction.Disable();
        closeInventoryAction.Disable();
    }

    void toggleInventory(bool state)
    {
        Time.timeScale = state ? 0f : 1f;
        InventoryUIController.instance.showInventoryPanel(state);
    }
    void Awake()
    {
        CacheComponent();
        toggleInventoryAction = new InputAction(binding: "<Keyboard>/tab");
        toggleInventoryAction.started += ctx => toggleInventory(true);
        closeInventoryAction = new InputAction(binding: "<Keyboard>/q");
        closeInventoryAction.started += ctx => toggleInventory(false);
    }
    void CacheComponent()
    {
        _collider = GetComponentInChildren<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _fsm = GetComponent<FSM>();
        if (!_collider || !_rigidbody || !_animator || _fsm)
        {
            Debug.Log("Null references in Player Controller");
        }

    }
    void Start()
    {
        CacheComponent();
        _fsm.ChangeState(new IdleState());
    }

    private void Update()
    {
        //HandleAnimated(_fsm.currentState);
    }
    public void UpdateFacing(Vector2 input)
    {
        if (input == Vector2.zero)
            return;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            direction = input.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        else
            direction = input.y > 0 ? FacingDirection.Up : FacingDirection.Down;
    }

    // ================= MOVEMENT =================
    public bool HandleMoving()
    {
        if (Camera.main != null)
        {
            float halfHeight = Camera.main.orthographicSize;
            float halfWidth = halfHeight * Camera.main.aspect;
            float padding = 0.2f;
            Vector3 camPos = Camera.main.transform.position;
            float minX = camPos.x - halfWidth + padding;
            float maxX = camPos.x + halfWidth - padding;
            float minY = camPos.y - halfHeight + padding;
            float maxY = camPos.y + halfHeight - padding;

            Vector2 playerPos = _rigidbody.position;
            if (playerPos.x <= minX && input.x < 0) input.x = 0;
            if (playerPos.x >= maxX && input.x > 0) input.x = 0;
            if (playerPos.y <= minY && input.y < 0) input.y = 0;
            if (playerPos.y >= maxY && input.y > 0) input.y = 0;
        }
        if (Mathf.Abs(input.x) >= 0.1f || Mathf.Abs(input.y) >= 0.1f)
        {
            _rigidbody.linearVelocity = new Vector2(input.x, input.y) * moveSpeed;

            if (_fsm.currentState is not RunState)
                _fsm.ChangeState(new RunState());

            return true;
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero; 
        }

        return false;
    }
    public void HandleAnimated(FSMState state)
    {
        string animationName = state.ToString().Replace("State", "") + direction.ToString();

        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);

        if (!currentState.IsName(animationName))
        {
            _animator.Play(animationName);
            //Debug.Log(animationName);
        }
    }

    // ========= INPUT EVENTS =========
    public void OnMove(InputValue movementvalue)
    {
        if (CanMove == false || isUsingRemote == true) return;
        input = movementvalue.Get<Vector2>();
    }
    public void OnInteract(InputValue isInteract)
    {
        if(isUsingRemote == true) return;
        if (isInteract.isPressed && _fsm.currentState is not PickUpState)
        {
            this.IsInteract = isInteract.isPressed;
            _fsm.ChangeState(new PickUpState());
        }
    }

    public void OnSwitchMode(InputValue isSwitch)
    {
        if(isSwitch.isPressed /*&& InventorySystem.instance.getInventory(0).amount > 0*/ )
        {
            isUsingRemote = !isUsingRemote;
            IsRemoteUsed.Invoke(isUsingRemote);
        }    
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public FacingDirection Facing { get; set; }
    public bool IsInteract { get => isInteract; set => isInteract = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
}
