using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System.Text;
using System.Reflection;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

    public Button btnTest;
    public Text txtGameRanking;
    private string user_id;

    // Use this for initialization
    void Start()
    {
        FB.API("me", HttpMethod.GET, (IGraphResult rslt) =>
        {
            user_id = rslt.ResultDictionary["id"].ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("index");
            SceneManager.UnloadScene("main");
            return;
        }
    }

    public void btnTest_Click(Text txt)
    {
        /*
        FB.API("me/taggable_friends", HttpMethod.GET, (IGraphResult rslt) =>
        {
            Debug.Log(rslt.ResultDictionary["data"].ToString());
            var list = JsonUtility.FromJson<User[]>(rslt.ResultDictionary["data"].ToString());
            txt.text = list.Length.ToString();
        });

        return;
        */

        var score = Restful.get<Score>("http://ckcks12.com:5000/score", user_id);
        txt.text = "당신의 점수 : " + score.score.ToString();
    }
}

public class Score
{
    public uint pk;
    public string id;
    public uint score;
    public uint game_id;
}

public class Article
{
    public uint pk;
    public string title;
    public string content;
    public string written_time;
}

public class User
{
    public string id;
}

public static class Restful
{
    public static T get<T>(string url, string id = null, Encoding encoding = null)
    {
        if (id != null)
        {
            url += "/" + id;
        }

        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        string resp;
        using (StreamReader r = new StreamReader(req.GetResponse().GetResponseStream(), encoding))
        {
            resp = r.ReadToEnd();
        }

        return JsonUtility.FromJson<T>(resp); 
    }

    public static string post<T>(string url, T t, Encoding encoding = null)
    {
        if( encoding == null )
        {
            encoding = Encoding.UTF8;
        }

        string data = "";
        foreach( FieldInfo f in t.GetType().GetFields())
        {
            data += "&" + f.Name + "=" + f.GetValue(t);
        }

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        using (StreamWriter w = new StreamWriter(req.GetRequestStream()))
        {
            w.Write(data);
            w.Flush();
        }
        string resp;
        using (StreamReader r = new StreamReader(req.GetResponse().GetResponseStream(), encoding))
        {
            resp = r.ReadToEnd();
        }
        Debug.Log(data);
        return resp;
    }
}