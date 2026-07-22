using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<ProfileData> profilesList = new List<ProfileData>();
    public int LevelsCompleted = 0;
    public bool IsGameStarted = false;
    public TextMeshProUGUI fpsText;
    CanvasManager canvasManager;
    private float deltaTime = 0.0f;
    public int currentProfileIndex;
    public ProfileData selectedProfile;

    private void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        FPS();
        SaveData data = SaveSystem.LoadData();
        profilesList = data.profilesList;
    }

    public void FPS()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Awake()
    {
        int GameManagerCount = FindObjectsOfType<GameManager>().Length;
        if (GameManagerCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void setActiveProfile(string profileName)
    {
        selectedProfile.profileName = profileName;
        foreach (ProfileData profile in profilesList)
        {
            if (selectedProfile.profileName == profile.profileName)
            {
                currentProfileIndex = profilesList.IndexOf(profile);
                LevelsCompleted = profile.levelsCompleted;
                selectedProfile.levelsCompleted = LevelsCompleted;
                break;
            }
        }
        foreach (ProfileData profile in profilesList)
        {
            Debug.Log(profile.profileName);
        }
        Debug.Log(currentProfileIndex);
    }

    public void CheckIfDeletingProfile()
    {
        ProfileSelector profile = FindObjectOfType<ProfileSelector>();
        profile.DeleteProfiles(profilesList[currentProfileIndex].profileName);
        Debug.Log(profilesList[currentProfileIndex].profileName);
        profilesList.RemoveAt(currentProfileIndex);
        SaveSystem.SaveData(this);
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncrementLevel()
    {
        LevelsCompleted++;
        selectedProfile.levelsCompleted++;
        profilesList[currentProfileIndex].levelsCompleted++;
        SaveSystem.SaveData(this);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
