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


    public GameObject bombPrefab;

    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteSide;

    void HandleMovement()
    {
        float inputX = 0;
        float inputY = 0;

        if (Input.GetKey(inputUp))
        {
            inputY = 1;
            spriteRenderer.sprite = spriteUp;

        }
        if (Input.GetKey(inputDown))
        {
            inputY = -1;
            spriteRenderer.sprite = spriteDown;

        }
        if (Input.GetKey(inputLeft))
        {
            inputX = -1;
            spriteRenderer.sprite = spriteSide;
            spriteRenderer.flipX = true;
        }
        if (Input.GetKey(inputRight))
        {
            inputX = 1;
            spriteRenderer.sprite = spriteSide;
            spriteRenderer.flipX = false;
        }

        Vector2 direction = new Vector2(inputX, inputY);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        rb2d.linearVelocity = direction * speed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void HandlePlaceBomb()
    {
        if (Input.GetKeyDown(placeBomb))
        {
            Vector3 bombPosition = transform.position;
            bombPosition = new Vector3(Mathf.Round(bombPosition.x), Mathf.Round(bombPosition.y), 0f);
            Instantiate(bombPrefab, bombPosition, Quaternion.identity);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandlePlaceBomb();
    }
}
