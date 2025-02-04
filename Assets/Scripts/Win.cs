using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{

    public Button buttonScript; // Reference to the Button script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.GetComponent<Player>() != null)
            {

                if (buttonScript.buttonHit)
                {
                    ResetGame();
                }
            }
        }
    }


    private void ResetGame()
    {
        // Reset the game or load the scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
