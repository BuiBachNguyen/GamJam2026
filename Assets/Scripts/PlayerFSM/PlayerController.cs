using Unity.VisualScripting;
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

    // ================= value INPUT =================

    Vector2 input = new Vector2(0, 0);

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

    void Awake()
    {
        CacheComponent();
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
        if (Mathf.Abs(input.x) >= 0.1f || Mathf.Abs(input.y) >= 0.1f)
        {
            _rigidbody.linearVelocity = new Vector2(input.x, input.y) * moveSpeed;

            if (_fsm.currentState is not RunState)
                _fsm.ChangeState(new RunState());

            return true;
        }
        else
        {
            _rigidbody.linearVelocity = new Vector2(0f, 0f);
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
        input = movementvalue.Get<Vector2>();
    }
    public void OnInteract(InputValue isInteract)
    {
        this.isInteract = isInteract.isPressed;
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

    public FacingDirection Facing { get; private set; }
}
