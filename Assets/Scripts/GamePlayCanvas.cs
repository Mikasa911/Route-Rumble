using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayCanvas : MonoBehaviour
{
    GameManager gameManager;
    PlayerMovement playerScript;

    [SerializeField] Canvas PauseCanvas;
    [SerializeField] Canvas UIButtons;

    public bool GamePaused = false;
    bool UndoIsPressed;
    private void Update()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerScript = FindAnyObjectByType<PlayerMovement>();
        PauseCanvas.gameObject.SetActive(false);
    }
    public void PauseButton()
    {
        UIButtons.gameObject.SetActive(false);
        PauseCanvas.gameObject.SetActive(true);
        GamePaused= true;
        Time.timeScale = 0f;
        gameManager.FPS();
    }
    public void Resume()
    {
        UIButtons.gameObject.SetActive(true);
        PauseCanvas.gameObject.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1f;
        gameManager.FPS();
    }
    public void LoadMenu()
    {
        gameManager.LoadMenu();
    }
    public bool CheckButtonClick()
    {
        if(Input.GetMouseButton(0))
        {
            UndoIsPressed = true;
        }
        else
        {
            UndoIsPressed = false;
        }
        return UndoIsPressed;
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
