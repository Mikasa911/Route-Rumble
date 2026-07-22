using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileSelector : MonoBehaviour
{
    [SerializeField]public TMP_InputField ProfileNameField;
    List<string> profiles= new List<string>();
    List<GameObject> buttonList = new List<GameObject>();
    [SerializeField]GameObject ProfileButtonPrefab;
    public Transform content;
    [SerializeField]GameObject profileListObj;
    [SerializeField] CanvasManager canvasManager;
    
    void Start()
  {     
      LoadProfileButtons();
    }
    public void AddProfile()
    {   
        profiles.Add(ProfileNameField.text);
        GameObject button = Instantiate(ProfileButtonPrefab, content);
        button.GetComponentInChildren<TextMeshProUGUI>().text = ProfileNameField.text; // Set button text
        //SaveSystem.SaveProfiles(profiles);
    }
    public bool NameChecker(string s)
    {     
        foreach (string profile in profiles)
        {
            if (profile==s)
            {        
                return true;
            }    
        }
        return false;
        }
   public void LoadProfileButtons()
    {       
        SaveData data = SaveSystem.LoadData();      
        foreach (ProfileData profile in data.profilesList)
        {       
            profiles.Add(profile.profileName);
        }
    
        foreach (string profile in profiles)
        {
            Debug.Log(profile);
            GameObject button = Instantiate(ProfileButtonPrefab,content);            
            button.GetComponentInChildren<TextMeshProUGUI>().text = profile; // Set button text
            buttonList.Add(button);          
        }     
    }
    public void DeleteProfiles(string name)
    {
        profiles.Remove(name);      
        foreach(GameObject g in buttonList)
        {
            if(g.GetComponentInChildren<TextMeshProUGUI>().text==name)
            {
                Debug.Log("DESTROY"+name);
                Destroy(g);
                buttonList.Remove(g);
                return;
            }
        }
    }
    
}
