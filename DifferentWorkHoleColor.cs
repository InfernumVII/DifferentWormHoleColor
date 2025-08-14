using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace DifferentWormHoleColor;

[BepInPlugin("com.infernumvii.differentwormholecolor", "DifferentWormHoleColor", "1.0.0")]
public class DifferentWormHoleColor : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private readonly Harmony harmony = new Harmony("com.infernumvii.differentwormholecolor");
    private static readonly Dictionary<GameObject, int> MageBookToTeamID = new();

    private void Awake()
    {
        Logger = base.Logger;
        harmony.PatchAll();
        Logger.LogInfo($"DifferentWormHoleColor is loaded!");
    }

    private void Update()
    {
        MageBookToTeamID.Clear();

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.name.Contains("Player")) continue;

            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement == null) continue;

            ScanForMageBooks(player.transform, playerMovement.playerTeam);
        }
    }

    private static void ScanForMageBooks(Transform parent, int teamID)
    {
        foreach (Transform child in parent)
        {
            if (child.name.Contains("MageBook") && child.gameObject.activeSelf)
            {
                MageBookToTeamID[child.gameObject] = teamID;
            }
            ScanForMageBooks(child, teamID); 
        }
    }
    
    public static int GetTeamIDByMageBook(GameObject mageBook)
    {
        return MageBookToTeamID.TryGetValue(mageBook, out int teamID) ? teamID : -1;
    }
    
}

