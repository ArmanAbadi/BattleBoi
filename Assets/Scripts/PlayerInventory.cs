using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public static class PlayerInventory
{
    public static ItemName[] InventoryItems = new ItemName[GlobalConstants.MaxBagSize];

    public static void ClaimXP(int xp)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "ClaimXP", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { ClaimXP = xp }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, ClaimXPSuccess, ClaimXPFail);
    }
    static void ClaimXPSuccess(ExecuteCloudScriptResult result)
    {
        //PlayFabLogin.Instance.playerData.XP = int.Parse(result);
        JsonObject jsonResult = (JsonObject)result.FunctionResult;

        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script

        PlayFabLogin.Instance.playerData.XP = int.Parse(messageValue.ToString());
        PlayerController.Instance.UpdateXP();
    }
    static void ClaimXPFail(PlayFabError error)
    {

    }
    public static void UpdateServerBag()
    {
        JsonArray bag = new JsonArray();
        foreach (Item item in ItemManager.Instance.PlayerItems)
        {
            serverItem temp = new serverItem();
            temp.itemName = item.itemName.ToString();
            temp.Quantity = item.Quantity;
            bag.Add(temp);
        }

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateBag", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { bag }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, UpdateBagSuccess, UpdateBagFail);
    }
    static void UpdateBagSuccess(ExecuteCloudScriptResult result)
    {
        
    }
    static void UpdateBagFail(PlayFabError error)
    {

    }
    public static void AddItem(Item Item)
    {
        bool found = false;
        foreach(Item item in ItemManager.Instance.PlayerItems)
        {
            if (item.itemName == Item.itemName)
            {
                found = true;
                item.Quantity += Item.Quantity;
            }
        }
        ItemBarController.Instance.RefreshItems();
        PlayerBag.Instance.RefreshItems();
        UpdateServerBag();
    }
    public static void UseItem(Item Item)
    {
        foreach (Item item in ItemManager.Instance.PlayerItems)
        {
            if (item.itemName == Item.itemName)
            {
                item.Quantity--;
            }
        }
        ItemBarController.Instance.RefreshItems();
        PlayerBag.Instance.RefreshItems();
        UpdateServerBag();
    }
}