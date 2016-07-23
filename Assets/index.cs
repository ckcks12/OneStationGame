using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine.UI;

public class index : MonoBehaviour
{
    public Text txtLog;
    public Button btnLogout;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.Screen.SetResolution(480, 800, true);  

        if (FB.IsLoggedIn)
        {
            FB.API("me", HttpMethod.GET, (IGraphResult rslt) =>
            {
                txtLog.text = rslt.ResultDictionary["name"] + "님 안녕하세요";
            });
            btnLogout.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void click()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.loginHandler);
    }

    public void loginHandler(IResult rslt)
    {
        if (FB.IsLoggedIn)
        {
            FB.API("me", HttpMethod.GET, (IGraphResult r) =>
            {
                txtLog.text = r.ResultDictionary["name"] + "님 안녕하세요";
            });
            btnLogout.gameObject.SetActive(true);
        }
        else
        {
            txtLog.text = "";
        }

    }

    public void btnLogout_Click()
    {
        FB.LogOut();
        btnLogout.gameObject.SetActive(false);
    }

    void OnGUI()
    {
    }
     
}
