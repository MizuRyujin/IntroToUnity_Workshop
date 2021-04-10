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
        _pActions.InGame.Jump.performed += ctx => Jump();
    }

    private void Update()
    {
        if (Paused) return;

        //* Get player input every frame
        GetInput();
        ChangeColliders();
        if (!_physicsMovement)
        {
            KinematicMovement();
        }
    }

    private void FixedUpdate()
    {
        if (Paused) return;

        if (_physicsMovement)
        {
            PhysicsMovement();
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
                    auxMove.y = Physics.gravity.y * _rb.gravityScale;
                    break;
            }
            auxMove.x = _moveDir.x * _baseStats.Speed;

            //* Create jump method
            _moveDir = auxMove;
            // _rb.transform.position += (Vector3)auxMove * _baseStats.Speed * Time.deltaTime;
        }
        _rb.MovePosition(_rb.transform.position + (Vector3)_moveDir * Time.deltaTime);
    }

    private void PhysicsMovement()
    {
    }

    private void Jump()
    {
        if (IsGrounded)
        {
            Vector2 auxMove = _moveDir;
            auxMove.y += 1f * _baseStats.Speed;
            print(auxMove);

            _moveDir = auxMove;
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
        if (_moveDir.x > 0)
        {
            _pVisuals.transform.Rotate(transform.up, 180, Space.Self);
        }
        else if (_moveDir.x < 0)
        {
            _pVisuals.transform.Rotate(transform.up, -180, Space.Self);
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
