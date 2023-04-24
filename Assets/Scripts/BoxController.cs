using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KaveUtil;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxController : MonoBehaviour
{
    public float _speed = 1;
    [Range(0, 360)]
    public float _velocityDegree = 0;

    private BoxCollider2D _boxCollider;
    private float _acceleration = 1;
    private Vector2 _curVelocity = new Vector2(0,0);
    private Vector2 _maxVelocity = new Vector2(1, -1);
    private int _numCollision = 0;
    private bool _isGrounded = false;
    private bool _isInAir = false;

    //static Vector2 Rotate(Vector2 v, float delta)
    //{
    //    return new Vector2(
    //        v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
    //        v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
    //    );
    //}

    //static void DrawTextInSceneView(Vector3 worldPos, string text, Color? colour = null)
    //{
    //    UnityEditor.Handles.BeginGUI();
    //    {
    //        if (colour.HasValue) GUI.color = colour.Value;
    //        SceneView view = UnityEditor.SceneView.currentDrawingSceneView;
    //        Vector3 viewPos = view.camera.WorldToScreenPoint(worldPos);
    //        viewPos.y += 24; // Value changed
    //        Vector2 screenPos = new Vector2(viewPos.x, -viewPos.y + view.position.height);
    //        GUI.Box(new Rect(screenPos, new Vector2(32, 32)), text);
    //    }
    //    UnityEditor.Handles.EndGUI();
    //}

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        UpdateMaxVelocity();
        
    }

    void Update()
    {
        _curVelocity = Vector2.MoveTowards(_curVelocity, _maxVelocity, _acceleration * Time.deltaTime);
        transform.Translate(_curVelocity * Time.deltaTime);

        _isGrounded = false;
        bool isInAir = true;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0);

        _numCollision = hits.Length;

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == _boxCollider)
                continue;

            isInAir = false;
            ColliderDistance2D colliderDistance = hit.Distance(_boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && _curVelocity.y < 0)
                {
                    _isGrounded = true;
                }
            }
        }

        if (isInAir != _isInAir)
        {
            _isInAir = isInAir;
        }
    }

    private void OnDrawGizmos()
    {
        // draw velocity
        //
        UpdateMaxVelocity();
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _maxVelocity);

        // draw state
        //
        string text = string.Format("{0}", _numCollision);
        
        Util.DrawTextInSceneView(transform.position, text, Color.white);
        Gizmos.color = oldColor; // restore original Gizmos color
    }

    void UpdateMaxVelocity()
    {
        _maxVelocity = Util.Rotate(Vector2.right, _velocityDegree * Mathf.Deg2Rad) * _speed;
    }
}
