using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    GameManager gameManager;
    public bool Key=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Key)
            {
                gameManager = FindAnyObjectByType<GameManager>();
                Debug.Log("You Won");
                if(SceneManager.GetActiveScene().buildIndex>gameManager.LevelsCompleted)
                {
                    gameManager.IncrementLevel();
                }
                LoadNextLevel();
            }
            else
            {
                Debug.Log("Key Not Collected");
            }
        }
    }
    public void ObtainedKey()
    {
        Key = true;
    }
    void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == (SceneManager.sceneCountInBuildSettings - 1))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
