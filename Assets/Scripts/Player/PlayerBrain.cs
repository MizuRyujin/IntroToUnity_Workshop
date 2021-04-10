using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class that controls the functionalities of the player object. It requires a 
/// Rigidbody2D and a CapsuleCollider2D components, so using this automatically 
/// adds them to the game object using this script
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(BoxCollider2D))]
public class PlayerBrain : Entity
{
    /// <summary>
    /// Bool to switch in editor between the 2 movement types
    /// </summary>
    [SerializeField] private bool _physicsMovement;

    [SerializeField] private GameObject _pVisuals;
    [SerializeField] private GameObject _feet;

    /// <summary>
    /// This is a reference to the rigidbody so we can access functionalities
    /// in code
    /// </summary>
    private Rigidbody2D _rb;

    /// <summary>
    /// The reference to the capsule collider to be use 
    /// for hit detection while in the air
    /// </summary>
    private CapsuleCollider2D _airCol;

    /// <summary>
    /// The reference to the capsule collider to be use 
    /// for hit detection while on the ground
    /// </summary>
    private BoxCollider2D _groundCol;

    /// <summary>
    /// Unity's new input system with the player actions
    /// </summary>
    private PlayerActions _pActions;

    private Vector2 _moveDir;

    private bool _shouldJump;

    public bool IsGrounded
    {
        get
        {
            Collider2D cols = Physics2D.OverlapCircle(_feet.transform.position,
                                                1f, LayerMask.GetMask("Ground"));
            return cols != null;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _airCol = GetComponent<CapsuleCollider2D>();
        _airCol.enabled = false;
        _groundCol = GetComponent<BoxCollider2D>();
        _groundCol.enabled = true;

        _pActions = new PlayerActions();
        _pActions.InGame.Movement.started += ctx => RotateVisuals();
        _pActions.InGame.Jump.performed += _ => Jump();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {

    }

    private void Update()
    {
        if (Paused) return;

        //* Get player input every frame
        GetInput();
        ChangeColliders();
        RotateVisuals();
    }

    private void FixedUpdate()
    {
        if (Paused) return;

        switch (_physicsMovement)
        {
            case true:
                PhysicsMovement();
                break;

            default:
                KinematicMovement();
                break;
        }
    }

    private void GetInput()
    {
        _moveDir.x = _pActions.InGame.Movement.ReadValue<float>();
    }

    private void KinematicMovement()
    {
        if (_moveDir != Vector2.zero)
        {
            Vector2 auxMove = new Vector2();

            switch (IsGrounded)
            {
                case true:
                    auxMove.y = 0f;
                    break;

                default:
                    auxMove.y = Physics.gravity.y;
                    break;
            }
            auxMove.x = _moveDir.x * _baseStats.Speed;

            _moveDir = auxMove;
        }
        OnJump();
        _rb.transform.Translate((Vector3)_moveDir * Time.fixedDeltaTime);
        // transform.Translate((Vector3)_moveDir * Time.fixedDeltaTime);
    }

    private void PhysicsMovement()
    {
        //* Check if magnitude is higher than speed
        if (!(_rb.velocity.magnitude > _baseStats.Speed) && _moveDir != Vector2.zero)
        {
            //* if not, add force
            print("Apply force");
            _rb.AddForce(_moveDir * _baseStats.MoveForce * 2);

        }
    }

    private void Jump()
    {
        _shouldJump = true;
    }

    private void OnJump()
    {
        if (IsGrounded && _shouldJump)
        {
            print("Must jump");
            _rb.AddForce(new Vector2(0f, _baseStats.MoveForce), ForceMode2D.Impulse);
            _shouldJump = false;
        }
    }

    private void ChangeColliders()
    {
        switch (IsGrounded)
        {
            case true:
                _airCol.enabled = false;
                _groundCol.enabled = true;
                break;

            case false:
                _airCol.enabled = true;
                _groundCol.enabled = false;
                break;
        }
    }


    private void RotateVisuals()
    {
        if (_moveDir != Vector2.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(
                    new Vector3(_moveDir.x, 0f, 0f), _rb.transform.up);

            if (_moveDir.x < 0f || _moveDir.x > 0f)
            {
                _pVisuals.transform.rotation = lookRotation * Quaternion.Euler(0f, 90, 0f);
            }
        }

    }

    private void OnEnable()
    {
        _pActions.Enable();
        _pActions.InGame.Movement.Enable();
    }
    private void OnDisable()
    {
        _pActions.Disable();
        _pActions.InGame.Movement.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_feet.transform.position, 1f);
    }

}
