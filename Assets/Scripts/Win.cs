using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class Win : MonoBehaviour
{
    public Player playerScript;
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
                    playerScript.Win();
                    ReloadScene();
                }
            }
        }
    }
    public void ReloadScene()
    {
        StartCoroutine(ReloadAfterDelay(2f));
    }


    IEnumerator ReloadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
