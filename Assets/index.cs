using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic; 

public class index : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GUI.Box(new Rect(0, 0, 100, 100), "aaaaaaaaaaaaaaa");
        Debug.Log("AAA");
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


}
