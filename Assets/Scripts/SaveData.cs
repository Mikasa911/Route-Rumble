using System.Collections.Generic;

[System.Serializable]
public class SaveData
{   
    public List<ProfileData> profilesList=new List<ProfileData>();
    
    public SaveData(List<ProfileData> profiles)
    {
        this.profilesList = profiles;
    }  
}
[System.Serializable]
public class ProfileData
{
    public string profileName;
    public int levelsCompleted;
    
    public ProfileData(string name,int levels)
    {
        this.profileName = name;
        this.levelsCompleted = levels;
    }
    
}
