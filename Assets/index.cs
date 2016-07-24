using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class index : MonoBehaviour
{
    public Text txtLog;
    public Button btnLogout;
    public Button btnLogin;
    public Text txtArticle;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(()=>
            {
                if (FB.IsLoggedIn)
                {
                    FB.API("me", HttpMethod.GET, (IGraphResult rslt) =>
                    {
                        txtLog.text = rslt.ResultDictionary["name"] + "님 안녕하세요";
                    });
                    btnLogin.GetComponentInChildren<Text>().text = "게임 시작";
                    btnLogout.gameObject.SetActive(true);
                }
                else
                {
                    txtLog.text = "";
                    btnLogin.GetComponentInChildren<Text>().text = "Facebook 로그인";
                    btnLogout.gameObject.SetActive(false);
                }
            });
        }
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.Screen.SetResolution(480, 800, true);
    } 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }
    }

    void OnEnable()
    {
        // 공지사항
        var atc = Restful.get<Article>("http://ckcks12.com:5000/article");
        txtArticle.text = "공지사항\n\n----------------------------------------\n" + atc.title + "\n----------------------------------------\n" + atc.content.Replace("\\n", "\n") + "\n----------------------------------------\n" + atc.written_time;
    }

    public void click()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends", "read_custom_friendlists" }, this.loginHandler);
    }

    public void loginHandler(IResult rslt)
    {
        if (FB.IsLoggedIn)
        {
            FB.API("me", HttpMethod.GET, (IGraphResult r) =>
            {
                txtLog.text = r.ResultDictionary["name"] + "님 안녕하세요";
            });
            btnLogin.GetComponentInChildren<Text>().text = "게임 시작";
            btnLogout.gameObject.SetActive(true);
        }
        else
        {
            txtLog.text = "";
            btnLogin.GetComponentInChildren<Text>().text = "Facebook 로그인";
            btnLogout.gameObject.SetActive(false);
        }
    }

    public void btnLogout_Click()
    {
        FB.LogOut();
        if (FB.IsLoggedIn)
        {
            FB.API("me", HttpMethod.GET, (IGraphResult rslt) =>
            {
                txtLog.text = rslt.ResultDictionary["name"] + "님 안녕하세요";
            });
            btnLogin.GetComponentInChildren<Text>().text = "게임 시작";
            btnLogout.gameObject.SetActive(true);
        }
        else
        {
            txtLog.text = "";
            btnLogin.GetComponentInChildren<Text>().text = "Facebook 로그인";
            btnLogout.gameObject.SetActive(false);
        }
    }

    void OnGUI()
    {

    }

    public void btnLogin_Click()
    {
        if (FB.IsLoggedIn)
        {
            SceneManager.LoadScene("main");
            SceneManager.UnloadScene("index");
        }
        else
        {
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.loginHandler);
        }
    }
}
