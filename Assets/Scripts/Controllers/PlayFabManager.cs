using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using PlayFab; 
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayFabManager : MonoBehaviour
{
    public GameObject rowPrefab;
    [CanBeNull] public Transform rowsParent;

    private void Start()
    {
        LoginPlayFab();
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "0_MainMenu" && rowsParent == null)
        {
            rowsParent = GameObject.FindGameObjectWithTag("LeaderTable").GetComponent<Transform>();
        }
    }

    void LoginPlayFab()
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true, 
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log($"Login Successful");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
    } 

    void OnError(PlayFabError error)
    {
        Debug.Log($"Error while logging in");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SavePlayerID()
     {
         var request = new UpdateUserTitleDisplayNameRequest()
         {
             DisplayName = PlayerPrefs.GetString("Username")
         };
         PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnPlayerIDSaved, OnError);
     }

    void OnPlayerIDSaved(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"User ID is now {result}");
    } 

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate
                {
                    StatisticName = "DistanceScore", Value = score
                }
                
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        Debug.Log($"Score {score} sent to PlayFab");
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result) => Debug.Log("Update Successful");

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "DistanceScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        if (rowsParent.childCount > 0)
        {
            foreach (Transform item in rowsParent) Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newRow = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] rowTexts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            rowTexts[0].text = (item.Position + 1).ToString();
            rowTexts[1].text = item.DisplayName;
            rowTexts[2].text = item.StatValue.ToString();
        }
    }

}
