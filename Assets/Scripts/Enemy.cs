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
