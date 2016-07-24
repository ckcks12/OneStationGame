using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System.Text;
using System.Reflection;

public class main : MonoBehaviour
{

    public Button btnTest;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btnTest_Click(Text txt)
    {
        Score score = new Score();
        string id = Random.Range(1, 10).ToString();
        score.id = id;
        score.score = (uint)Random.Range(1, 100);
        score.game_id = 1;
        Restful.post("http://ckcks12.com:5000/score", score);

        score = Restful.get<Score>("http://ckcks12.com:5000/score", id);
        txt.text = score.id + " = " + score.score;
    }
}

public class Score
{
    public uint pk;
    public string id;
    public uint score;
    public uint game_id;
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