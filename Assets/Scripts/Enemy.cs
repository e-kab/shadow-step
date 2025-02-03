using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;


    public KeyCode inputLeft = KeyCode.LeftArrow;
    public KeyCode inputRight = KeyCode.RightArrow;
    public KeyCode inputUp = KeyCode.UpArrow;
    public KeyCode inputDown = KeyCode.DownArrow;
    public KeyCode placeBomb = KeyCode.LeftShift;


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

    private Vector2 lastDirection = Vector2.down;
    void HandleMovement()
    {
        float inputX = 0;
        float inputY = 0;

        if (Input.GetKey(inputUp))
        {
            inputY = 1;
            lastDirection = Vector2.up;

        }
        if (Input.GetKey(inputDown))
        {
            inputY = -1;
            lastDirection = Vector2.down;

        }
        if (Input.GetKey(inputLeft))
        {
            inputX = -1;
            lastDirection = Vector2.left;
        }
        if (Input.GetKey(inputRight))
        {
            inputX = 1;
            lastDirection = Vector2.right;
        }

        Vector2 direction = new Vector2(inputX, inputY);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        rb2d.linearVelocity = direction * speed;

        // Set sprite based on movement
        if (direction.magnitude > 0)
        {
            spriteRenderer.sprite = GetAnimationFrame(lastDirection);
        }
        else
        {
            // If not moving, set to idle frame
            spriteRenderer.sprite = GetIdleSprite(lastDirection);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
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
