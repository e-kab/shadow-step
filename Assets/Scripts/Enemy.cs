using UnityEngine;
using System.Collections;

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

    // Death Handling
    public Sprite[] deathFrames;
    private bool isDead = false;
    int currentFrameIndex = 0;
    float frameTimer;

    // Coroutine reference for movement
    private Coroutine movementCoroutine;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        movementCoroutine = StartCoroutine(NPCMovementRoutine());  // Start movement routine

        frameTimer = (1f / framesPerSecond);
        currentFrameIndex = 0;

        Invoke("Die", 5f); // For testing: will call Death() after 5 seconds
    }

    void Update()
    {
        if (isDead)
        {
            HandleDeath();
        }
        else
        {
            HandleAnimation();
            HandleFlashlight(lastDirection);
        }
    }

    void HandleFlashlight(Vector2 direction)
    {
        Vector3 origin = transform.position;
        RaycastHit2D hit = new RaycastHit2D();

        hit = Physics2D.Raycast(origin, direction, 1f, LayerMask.GetMask("Player"));

        if (hit .collider != null)
        {
            Debug.Log("Player Spotted");
        }

    }

    IEnumerator NPCMovementRoutine()
    {
        while (!isDead)
        {
            // Pick a random direction (up, down, left, right)
            movementDirection = GetRandomDirection();
            while (!IsPathClear(movementDirection))
            {
                movementDirection = GetRandomDirection(); // Pick a new direction if blocked
            }

            // Move for 2 seconds if the path is clear
            float timeElapsed = 0f;
            while (timeElapsed < 2f)
            {
                if (!IsPathClear(movementDirection))
                {
                    movementDirection = Vector2.zero;
                    break; // Stop moving if path is blocked
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }

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

    void HandleDeath()
    {
        frameTimer -= Time.deltaTime;

        if (frameTimer <= 0)
        {
            currentFrameIndex++;
            if (currentFrameIndex >= deathFrames.Length)
            {
                Destroy(gameObject);
                return;
            }
            frameTimer = (1f / framesPerSecond);
            spriteRenderer.sprite = deathFrames[currentFrameIndex];
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

    bool IsPathClear(Vector2 direction)
    {
        Vector3 origin = transform.position;
        RaycastHit2D hit = new RaycastHit2D();

        if (direction == Vector2.up)
        {
            hit = Physics2D.Raycast(origin, Vector2.up, 0.55f, ~LayerMask.GetMask("Enemy"));

        }
        else if (direction == Vector2.down)
        {
            hit = Physics2D.Raycast(origin, Vector2.down, 0.55f, ~LayerMask.GetMask("Enemy"));

        }
        else if (direction == Vector2.left)
        {
            hit = Physics2D.Raycast(origin, Vector2.left, 0.5f, ~LayerMask.GetMask("Enemy"));
        }
        else if (direction == Vector2.right)
        {
            hit = Physics2D.Raycast(origin, Vector2.right, 0.5f, ~LayerMask.GetMask("Enemy"));
        }

        // Return true if no obstacle was hit
        return (hit.collider == null);
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            // Stop movement and animation routines
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
            }

        }
    }
}
