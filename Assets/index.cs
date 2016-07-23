using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic; 

public class index : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        UnityEngine.Screen.SetResolution(480, 800, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void click()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.loginHandler);
    }

    public void loginHandler(ILoginResult rslt)
    {
        GUI.Box(new Rect(0, 0, 100, 100), "This is the text to be displayed");

        if (rslt.Error == null && ! rslt.Cancelled  && rslt.AccessToken != null)
        {
            
        }
    }

    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1"))
        {
            Application.LoadLevel(1);
        }

        // Make the second button.
        if (GUI.Button(new Rect(20, 70, 80, 20), "Level 2"))
        {
            Application.LoadLevel(2);
        }
    }

}
