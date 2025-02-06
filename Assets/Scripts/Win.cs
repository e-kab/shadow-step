using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Win : MonoBehaviour
{
    public Player playerScript;
    public Button buttonScript; // Reference to the Button script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.GetComponent<Player>() != null)
        {
            if (buttonScript.buttonHit)
            {
                playerScript.Win();
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        StartCoroutine(LoadNextSceneAfterDelay(2f));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(nextSceneIndex);
    }
}
