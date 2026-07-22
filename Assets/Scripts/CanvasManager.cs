using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas MainMenuCanvas;
    [SerializeField] Canvas LevelSelectCanvas;
    [SerializeField] Canvas ProfileCanvas;
    [SerializeField] GameObject SelectionCanvas;
    [SerializeField] GameObject ProfileScrollView;

    GameManager gameManager;
    Button[] LevelButtons;
    int LevelCount;

    private void Awake()
    {
        LevelButtons = LevelSelectCanvas.GetComponentsInChildren<Button>();
        LevelCount = LevelButtons.Length - 1;
    }

    void Start()
    {
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        foreach (GameManager g in gameManagers)
        {
            if (g.IsGameStarted)
            {
                gameManager = g;
                break;
            }
            else
            {
                gameManager = gameManagers[0];
            }
        }

        if (gameManager.IsGameStarted)
        {
            ActivateMainMenu();
            Debug.Log("IsGameStared");
            ProfileCanvas.gameObject.SetActive(false);
            SelectionCanvas.SetActive(false);
            ProfileScrollView.SetActive(false);
        }
        else
        {
            ProfileScrollView.SetActive(true);
            Debug.Log("NotGameStared");
            LevelSelectCanvas.enabled = false;
            MainMenuCanvas.enabled = false;
            ProfileCanvas.gameObject.SetActive(true);
            SelectionCanvas.SetActive(true);
        }
    }

    public void ChangeProfile()
    {
        gameManager.LevelsCompleted = 0;
        MainMenuCanvas.enabled = false;
        ProfileCanvas.gameObject.SetActive(true);
        SelectionCanvas.SetActive(true);
        ProfileScrollView.SetActive(true);
    }

    public void DeleteProfile()
    {
        MainMenuCanvas.enabled = false;
        SelectionCanvas.SetActive(true);
        ProfileScrollView.SetActive(true);
        gameManager.CheckIfDeletingProfile();
    }

    public void CreateProfile()
    {
        ProfileCanvas.transform.Find("Name").gameObject.SetActive(true);
        SelectionCanvas.SetActive(false);
        ProfileScrollView.SetActive(false);
    }

    public void OnSelectingProfile(string s)
    {
        ActivateMainMenu();
        gameManager.setActiveProfile(s);
        ProfileCanvas.gameObject.SetActive(false);
        SelectionCanvas.SetActive(false);
        ProfileScrollView.SetActive(false);
    }

    public void NameOk()
    {
        string txt = ProfileCanvas.GetComponent<ProfileSelector>().ProfileNameField.text;
        if (string.IsNullOrEmpty(txt) || ProfileCanvas.GetComponent<ProfileSelector>().NameChecker(txt))
        {
            return;
        }
        ProfileData newProfile = new ProfileData(txt, 0);
        gameManager.profilesList.Add(newProfile);
        SaveSystem.SaveData(gameManager);
        ProfileCanvas.transform.Find("Name").gameObject.SetActive(false);
        ProfileCanvas.GetComponent<ProfileSelector>().AddProfile();
        SelectionCanvas.SetActive(true);
        ProfileScrollView.SetActive(true);
    }

    public void NameCancel()
    {
        ProfileCanvas.transform.Find("Name").gameObject.SetActive(false);
        ProfileScrollView.SetActive(true);
        SelectionCanvas.SetActive(true);
    }

    public void LoadLevel(TextMeshProUGUI buttontext)
    {
        gameManager.IsGameStarted = true;
        Debug.Log(buttontext);
        int levelToLoad = int.Parse(buttontext.text);
        SceneManager.LoadScene(levelToLoad);
    }

    public void PressBack()
    {
        ActivateMainMenu();
    }

    public void PressPlay()
    {
        gameManager = FindObjectOfType<GameManager>();
        ActivateLevelSelectCanvas();
        DeactivateButtons();
        ActivateButtons(gameManager.LevelsCompleted);
    }

    public void ResetLevels()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.LevelsCompleted = 0;
        SaveSystem.SaveData(gameManager);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ActivateMainMenu()
    {
        MainMenuCanvas.enabled = true;
        LevelSelectCanvas.enabled = false;
    }

    public void ActivateLevelSelectCanvas()
    {
        MainMenuCanvas.enabled = false;
        LevelSelectCanvas.enabled = true;
    }

    public void DeactivateButtons()
    {
        for (int i = 0; i < LevelCount; i++)
        {
            LevelButtons[i].interactable = false;
        }
    }

    public void ActivateButtons(int LevelsCompleted)
    {
        for (int i = 0; i <= LevelsCompleted; i++)
        {
            if (i == SceneManager.sceneCountInBuildSettings - 1)
            {
                break;
            }
            LevelButtons[i].interactable = true;
        }
    }
}
