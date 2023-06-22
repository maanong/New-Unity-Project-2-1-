using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [Header("RayCast Collision")]
    [SerializeField]
    private LayerMask collisionLayer;

    [Header("RayCast")]
    [SerializeField]
    private int horizontalRayCount = 4;
    [SerializeField]
    private int verticalRayCount = 4;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce = 10;
    private float gravity = -20.0f;

    private Vector3 velocity;
    private readonly float skinWidth = 0.015f;

    private Collider2D collider2D;
    private ColliderCorner colliderCorner;
    private CollisionChecker collisionChecker;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CalculateRaySpacing();
        UpdateColliderCorner();
        collisionChecker.Reset();

        UpdateMovement();

        if (collisionChecker.up || collisionChecker.down)
        {
            velocity.y = 0;
        }
        JumpTo();
    }

    private void UpdateMovement()
    {
        velocity.y += gravity * Time.deltaTime;

        Vector3 currentVelocity = velocity * Time.deltaTime;

        if( currentVelocity.x != 0)
        {
            RaycastsHorizontal(ref currentVelocity);
        }
        if(currentVelocity.y != 0)
        {
            RaycastsVertical(ref currentVelocity);
        }

        transform.position += currentVelocity;
    }

    public void Moveto(float x)
    {
        velocity.x = x * moveSpeed;
    }

    public void JumpTo()
    {
        if(collisionChecker.down)
        {
            velocity.y = jumpForce;
        }
    }

    private void RaycastsHorizontal(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.x);
        float distance = Mathf.Abs(velocity.x);
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;

        for (int i = 0; i < horizontalRayCount; ++i )
        {
            rayPosition = (direction == 1) ? colliderCorner.bottomRight : colliderCorner.bottomLeft;
            rayPosition += Vector2.up * (horizontalRaySpacing * i);

            hit = Physics2D.Raycast(rayPosition, Vector2.right * direction, distance, collisionLayer);

            if(hit)
            {
                velocity.x = (hit.distance - skinWidth) * direction;
                distance = hit.distance;

                collisionChecker.left = direction == -1;
                collisionChecker.right = direction == 1;
            }

            Debug.DrawLine(rayPosition, rayPosition + Vector2.right * direction * distance, Color.yellow);
        }
    }
    private void RaycastsVertical(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float distance = Mathf.Abs(velocity.y) + skinWidth;
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;

        for (int i = 0; i < horizontalRayCount; ++i)
        {
            rayPosition = (direction == 1) ? colliderCorner.topLeft : colliderCorner.bottomLeft;
            rayPosition += Vector2.up * (verticalRaySpacing * i + velocity.x);

            hit = Physics2D.Raycast(rayPosition, Vector2.up * direction, distance, collisionLayer);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * direction;
                distance = hit.distance;

                collisionChecker.down = direction == -1;
                collisionChecker.up = direction == 1;
            }

            Debug.DrawLine(rayPosition, rayPosition + Vector2.up * direction * distance, Color.yellow);
        }
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = collider2D.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
    private void UpdateColliderCorner()
    {
        Bounds bounds = collider2D.bounds;
        bounds.Expand(skinWidth * -2);
        colliderCorner.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        colliderCorner.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        colliderCorner.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    private struct ColliderCorner
    {
        public Vector2 topLeft;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    public struct CollisionChecker
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public void Reset()
        {
            up = false;
            down = false;
            left = false;
            right = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < horizontalRayCount; ++i)
        {
            Vector2 position = Vector2.up * horizontalRaySpacing * i;
            Gizmos.DrawSphere(colliderCorner.bottomRight + position, 0.1f);
            Gizmos.DrawSphere(colliderCorner.bottomLeft + position, 0.1f);
        }

        for (int i = 0; i < verticalRayCount; ++i)
        {
            Vector2 position = Vector2.right * verticalRaySpacing * i;
            Gizmos.DrawSphere(colliderCorner.topLeft + position, 0.1f);
            Gizmos.DrawSphere(colliderCorner.bottomLeft + position, 0.1f);
        }
    }

}
