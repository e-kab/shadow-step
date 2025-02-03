using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    private LineRenderer flashlight; // Flashlight visual
    public float flashlightLength = 1f; // Max length of the flashlight beam



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

        // Add a LineRenderer component if not already attached
        flashlight = gameObject.AddComponent<LineRenderer>();
        flashlight.startWidth = 0.05f; // Thin beam
        flashlight.endWidth = 0.5f;
        flashlight.positionCount = 2; // Start and end points
        flashlight.material = new Material(Shader.Find("Sprites/Default")); // Use basic sprite shader
        flashlight.startColor = Color.red;
        flashlight.endColor = new Color(1f, 1f, 0f, 0.5f); // Fades slightly

        movementCoroutine = StartCoroutine(NPCMovementRoutine());  // Start movement routine

        frameTimer = (1f / framesPerSecond);
        currentFrameIndex = 0;

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
        Vector3 endPoint = origin + (Vector3)direction * flashlightLength;

        RaycastHit2D hit = new RaycastHit2D();

        hit = Physics2D.Raycast(origin, direction, flashlightLength, ~LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            endPoint = hit.point; // Shrink flashlight if it hits something
        }

        // Update LineRenderer
        flashlight.SetPosition(0, origin);   // Start at enemy's position
        flashlight.SetPosition(1, endPoint); // End at raycast hit or max range

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

        // Create a layer mask that ignores both "Enemy" and "Player" layers
        int ignoreLayers = ~LayerMask.GetMask("Enemy", "Player");

        if (direction == Vector2.up)
        {
            hit = Physics2D.Raycast(origin, Vector2.up, 0.55f, ignoreLayers);

        }
        else if (direction == Vector2.down)
        {
            hit = Physics2D.Raycast(origin, Vector2.down, 0.55f, ignoreLayers);

        }
        else if (direction == Vector2.left)
        {
            hit = Physics2D.Raycast(origin, Vector2.left, 0.5f, ignoreLayers);
        }
        else if (direction == Vector2.right)
        {
            hit = Physics2D.Raycast(origin, Vector2.right, 0.5f, ignoreLayers);
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