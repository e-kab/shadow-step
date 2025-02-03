using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    // Idle frames
    public Sprite upIdle;
    public Sprite downIdle;
    public Sprite leftIdle;
    public Sprite rightIdle;

    // Walking animation frames
    public Sprite[] upFrames;
    public Sprite[] downFrames;
    public Sprite[] leftFrames;
    public Sprite[] rightFrames;
    public float framesPerSecond = 5;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 lastDirection = Vector2.down; // Default facing direction

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(NPCMovementRoutine());
    }

    void Update()
    {
        HandleAnimation();
    }

    IEnumerator NPCMovementRoutine()
    {
        while (true)
        {
            // Pick a random direction (up, down, left, right, or stop)
            movementDirection = GetRandomDirection();

            // Move for 2 seconds
            yield return new WaitForSeconds(2f);

            // Stop moving for 2 seconds (idle)
            movementDirection = Vector2.zero;
            yield return new WaitForSeconds(2f);
        }
    }

    Vector2 GetRandomDirection()
    {

        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            case 3: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    void HandleAnimation()
    {
        rb2d.linearVelocity = movementDirection * speed;

        if (movementDirection != Vector2.zero)
        {
            lastDirection = movementDirection;
            spriteRenderer.sprite = GetAnimationFrame(lastDirection);
        }
        else
        {
            spriteRenderer.sprite = GetIdleSprite(lastDirection);
        }
    }

    void Detect()
    {
        // Right Hit Detection
        Vector3 origin = transform.position;
        Vector3 rightBlast = transform.position + (new Vector3(1, 0, 0));
        Vector3 rightDirection = (rightBlast - origin).normalized;
        float rightDistance = Vector3.Distance(origin, rightBlast);

        RaycastHit2D rightHit = Physics2D.Raycast(origin, rightDirection, rightDistance);


        // Left Hit Detection
        Vector3 leftBlast = transform.position - (new Vector3(1, 0, 0));
        Vector3 leftDirection = (leftBlast - origin).normalized;
        float leftDistance = Vector3.Distance(origin, leftBlast);

        RaycastHit2D leftHit = Physics2D.Raycast(origin, leftDirection, leftDistance);

        // Up Hit Detection
        Vector3 upBlast = transform.position + (new Vector3(0, 1, 0));
        Vector3 upDirection = (upBlast - origin).normalized;
        float upDistance = Vector3.Distance(origin, upBlast);

        RaycastHit2D upHit = Physics2D.Raycast(origin, upDirection, upDistance);

        // Down Hit Detection
        Vector3 downBlast = transform.position - (new Vector3(0, 1, 0));
        Vector3 downDirection = (downBlast - origin).normalized;
        float downDistance = Vector3.Distance(origin, downBlast);

        RaycastHit2D downHit = Physics2D.Raycast(origin, downDirection, downDistance);

        if (rightHit.collider != null)
        {
            Debug.Log("Right Side Hit");

        }

        if (leftHit.collider != null)
        {
            Debug.Log("Left Side Hit");

        }

        if (upHit.collider != null)
        {
            Debug.Log("Up Side Hit");
            
        }

        if (downHit.collider != null)
        {
            Debug.Log("Down Side Hit");
            
        }
    }

    Sprite GetAnimationFrame(Vector2 direction)
    {
        Sprite[] frames;
        if (direction == Vector2.up)
            frames = upFrames;
        else if (direction == Vector2.down)
            frames = downFrames;
        else if (direction == Vector2.left)
            frames = leftFrames;
        else
            frames = rightFrames;

        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        return frames[index];
    }

    Sprite GetIdleSprite(Vector2 direction)
    {
        if (direction == Vector2.up)
            return upIdle;
        if (direction == Vector2.down)
            return downIdle;
        if (direction == Vector2.left)
            return leftIdle;
        return rightIdle;
    }
}
