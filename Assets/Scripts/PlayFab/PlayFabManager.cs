using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    public string PlayfabID = "D29F";
    public string CustomAccountID = "";

    public static PlayFabManager Instance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        PlayFabSettings.TitleId = PlayfabID;

        Login("iduser");
    }

    [ContextMenu("Login PlayFab")]
    public void Login(string CustomAccountLogin)
    {
        print("Login with " + CustomAccountLogin);

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayfabID,
            CreateAccount = true,
            CustomId = CustomAccountLogin
        };

        PlayFabClientAPI.LoginWithCustomID(
            request,
            (result) =>
            {
                CustomAccountID = result.PlayFabId;
                Debug.Log("Got PlayFabID: " + CustomAccountLogin);
                if (result.NewlyCreated)
                {
                    Debug.Log("(new account)");
                    setDisplayName("Name Display In PlayFab");
                }
                else
                {
                    Debug.Log("(existing account)");
                }
                // up test
                Dictionary<string, string> listUserData = new Dictionary<string, string>();
                listUserData.Add("keytest1", "Noi dung test 1");
                listUserData.Add("keytest2", "Noi dung test 2");
                UpData(listUserData);
                // down test
                DownData(null);
            },
            (error) =>
            {
                Debug.Log("Error logging in player with custom ID:");
                Debug.Log(error.ErrorMessage);
            }
        );
    }

    private void setDisplayName(String displayName)
    {
        UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = displayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            request,
            (result) =>
            {
                print("Current name in playfab: " + result.DisplayName);
            },
            (error) =>
            {
                Debug.Log(error.ErrorMessage);
            }
        );
    }

    private void UpData(Dictionary<string, string> listKeyValue)
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = listKeyValue
        };

        PlayFabClientAPI.UpdateUserData(
            request,
            (result) =>
            {
                Debug.Log("Successfully updated user data");
            },
            (error) =>
            {
                Debug.Log("Got error setting user data Ancestor to Arthur");
                Debug.Log(error.ErrorDetails);
            }
        );
    }

    // if listKey = null then down all
    private void DownData(List<string> listKey)
    {
        GetUserDataRequest request = new GetUserDataRequest()
        {
            PlayFabId = CustomAccountID,
            Keys = listKey
            //Keys = new List<string>(){"hightscore",...}
        };

        PlayFabClientAPI.GetUserData(
            request,
            (result) =>
            {
                if ((result.Data == null) || (result.Data.Count == 0))
                {
                    Debug.Log("No user data available");
                }
                else
                {
                    foreach (var item in result.Data)
                    {
                        Debug.Log("    " + item.Key + " == " + item.Value.Value);
                    }
                }
            },
            (error) =>
            {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.ErrorMessage);
            }
        );
    }

    [ContextMenu("DownTitleData")]
    public void DownTitleData()
    {
        GetTitleDataRequest request = new GetTitleDataRequest()
        {
            Keys = null
        };

        PlayFabClientAPI.GetTitleData(
            request,
            (result) =>
            {
                if ((result.Data == null) || (result.Data.Count == 0))
                {
                    Debug.Log("TitleData dont have data");
                }
                else
                {
                    Debug.Log(">> DownTitleData:");
                    foreach (var item in result.Data)
                    {
                        Debug.Log("    " + item.Key + " == " + item.Value);
                    }
                }
            },
            (error) =>
            {
                Debug.Log(">> Error in TitleData: " + error.ErrorMessage);
            }
        );
    }

}