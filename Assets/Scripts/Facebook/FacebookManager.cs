using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Facebook;
using Facebook.Unity;
using Facebook.MiniJSON;

public class FacebookManager : MonoBehaviour
{
    void Awake()
    {
        InitFb();
    }

    void InitFb()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }


    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            //FB.ActivateApp();
            if (FB.IsLoggedIn)
            {
                // Do something if logined
            }
            else
            {
                // Do something if not login
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    [ContextMenu("Login Facebook")]
    public void Login()
    {
        var perms = new List<string>() { "publish_actions" };
        FB.LogInWithPublishPermissions(perms, LoginCallBack);

    }

    private void LoginCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            GetMyName();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void GetMyName()
    {
        FB.API("/me?fields=id,name", HttpMethod.GET, GetMyNameCallback);
    }

    void GetMyNameCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            return;
        }
        var dict = Json.Deserialize(result.RawResult.ToString()) as Dictionary<string, object>;
        string Id = dict["id"].ToString();
        string Name = dict["name"].ToString();

        print("Id= " + Id + " - Name= " + Name);

    }

}