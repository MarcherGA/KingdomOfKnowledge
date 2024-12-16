using System;

[Serializable]
public class UserData
{
    public string fullName { get; set; }
    public string birthday { get; set; }
    public string gender { get; set; }
    public string userClass { get; set; }
    public string username { get; set; }
    public string email { get; set; }


    public UserData(string fullName, string birthday, string gender, string userClass, string username, string email)
    {
        this.fullName = fullName;
        this.birthday = birthday;
        this.gender = gender;
        this.userClass = userClass;
        this.username = username;
        this.email = email; 
    }
}
