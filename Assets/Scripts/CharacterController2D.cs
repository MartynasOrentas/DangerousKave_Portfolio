using UnityEngine;
using KaveUtil;

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

    public delegate void OnJumpingCallback(bool isJumping);
    public event OnJumpingCallback OnJumping;
    public delegate void OnCollisionCallback(Collider2D coll2d
        , ColliderDistance2D collDist2d);
    public event OnCollisionCallback OnCollision;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool _isGrounded = false;
    private bool _isJumping = false;
    private bool _isFacingRight = true;
    private bool _isCeilCollision = false;
    private int _hitCount = 0;
    private int _hitCount2 = 0;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
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

        bool isGrounded = false;
        _isCeilCollision = false;
        bool isJumping = true;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0);
        _hitCount = hits.Length;

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == _boxCollider)
                continue;

            isJumping = false;
            ColliderDistance2D colliderDistance = hit.Distance(_boxCollider);

            if (OnCollision != null)
                OnCollision(hit, colliderDistance);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                if (hit.gameObject.CompareTag("Coin") == false)
                {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                }//if

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && _velocity.y <= 0)
                {
                    isGrounded = true;
                }
                if (Vector2.Angle(colliderDistance.normal, Vector2.down) < 90 && _velocity.y > 0)
                {
                    _isCeilCollision = true;
                    _velocity.y = 0;
                }
            }
        }

        _isGrounded = isGrounded;

        Vector2 groundPos = transform.position;
        groundPos.y -= 0.02f;
        if (LevelManager.IsOverlapWithTilemap(groundPos))
            _isGrounded = true;
        hits = Physics2D.OverlapPointAll(groundPos);
        _hitCount2 = hits.Length;
        if (_hitCount2 >= 1)
            _isGrounded = true;

        if (isJumping != _isJumping)
        {
            _isJumping = isJumping;
            OnJumping(isJumping);
        }
    }

    void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.magenta;
        string text = string.Format("{0} {1} {2} {3}", _hitCount, _hitCount2, _isGrounded, _isJumping);
        Util.DrawTextInSceneView(transform.position, text, Color.white);
        Gizmos.color = oldColor; // restore original Gizmos color
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
}//public class CharacterController2D : MonoBehaviour
