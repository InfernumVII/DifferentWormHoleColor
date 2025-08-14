using System;
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
            Texture2D texture2D = GetTeamWormholeTexture(DifferentWormHoleColor.GetTeamIDByMageBook(__instance.mbc.gameObject));
            if (texture2D == null) return;            
            GameObject planeOfWormHole = __instance.gameObject.transform.GetChild(0).gameObject;
            MeshRenderer renderer = planeOfWormHole.GetComponent<MeshRenderer>();
            Material material = renderer.materials[0];
            material.SetTexture("_Texture2D", texture2D);
        }

        private static Texture2D Base64ToTexture(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            return texture;
        }

        private static Texture2D GetTeamWormholeTexture(int teamId)
        {
            return teamId switch
            {
                0 => Base64ToTexture(Images.greenWormHole),
                2 => Base64ToTexture(Images.redWormHole),
                _ => null
            };
        }
    }
    
}
