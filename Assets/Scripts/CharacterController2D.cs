using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float _speed = 2;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float _walkAcceleration = 8;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float _airAcceleration = 4;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float _groundDeceleration = 8;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float _jumpHeight = 1.1f;

    private BoxCollider2D _boxCollider;

    private Vector2 _velocity;

    public Vector2 Velocity
    {
        get { return _velocity; }
    }

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool _isGrounded;
    private bool _isJumping = false;
    private bool _isFacingRight = true;

    public delegate void OnJumpingCallback(bool isJumping);
    public event OnJumpingCallback OnJumping;
    public delegate void OnCollisionCallback(Collider2D coll2d
        , ColliderDistance2D collDist2d);
    public event OnCollisionCallback OnCollision;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Flip(float moveInput)
    {
        Vector3 scale = transform.localScale;
        if ((scale.x > 0.0f && moveInput < 0) || (scale.x < 0.0f && moveInput > 0))
        {
            scale.x *= -1;
            transform.localScale = scale;
            _isFacingRight = (scale.x > 0);
        }
    }

    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        float moveInput = Input.GetAxisRaw("Horizontal");

        if ((moveInput > 0 && _isFacingRight == false) || (moveInput < 0 && _isFacingRight == true))
            Flip(moveInput);

        if (_isGrounded)
        {
            _velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                // Calculate the velocity required to achieve the target jump height.
                _velocity.y = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }

        float acceleration = _isGrounded ? _walkAcceleration : _airAcceleration;
        float deceleration = _isGrounded ? _groundDeceleration : 0;

        if (moveInput != 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.deltaTime);
        }

        _velocity.y += Physics2D.gravity.y * Time.deltaTime;

        transform.Translate(_velocity * Time.deltaTime);

        _isGrounded = false;
        bool isJumping = true;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == _boxCollider)
                continue;

            isJumping = false;
            ColliderDistance2D colliderDistance = hit.Distance(_boxCollider);

            OnCollision(hit, colliderDistance);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && _velocity.y < 0)
                {
                    _isGrounded = true;
                }
            }
        }

        if (isJumping != _isJumping)
        {
            _isJumping = isJumping;
            OnJumping(isJumping);
        }
    }
}
