using System.IO;
using HarmonyLib;
using UnityEngine;

namespace DifferentWormHoleColor.Patches
{
    [HarmonyPatch(typeof(WormholeTele))]
    public static class WormholeTelePatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPostfix(WormholeTele __instance)
        {
            string filePath = "";
            int teamID = DifferentWormHoleColor.GetTeamIDByMageBook(__instance.mbc.gameObject);
            if (teamID == 0)
            {
                filePath = "C:/Users/egor5/Documents/portal_red2.png";
            }
            else if (teamID == 2)
            {
                filePath = "C:/Users/egor5/Documents/portal_green.png";
            }
            else
            {
                return;
            }
            GameObject planeOfWormHole = __instance.gameObject.transform.GetChild(0).gameObject;
            MeshRenderer renderer = planeOfWormHole.GetComponent<MeshRenderer>();
            Material material = renderer.materials[0];
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(fileData);
            material.SetTexture("_Texture2D", texture2D);
        }
    }
}
