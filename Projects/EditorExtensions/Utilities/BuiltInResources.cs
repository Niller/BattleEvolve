using System;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.Utilities
{
    public static class BuiltInResources
    {
        public static Texture2D FindIcon(string name)
        {
            Texture2D result;
            try
            {
                result = (Texture2D) (EditorGUIUtility.FindTexture(name) ?? EditorGUIUtility.Load(name));
            }
            catch (Exception)
            {
                Debug.LogWarning($"Unable to find editor resource {name}");
                return null;
            }

            return result;
        }
        
        public static Color GetDefaultSkinColor(bool isProSkin)
        {
            return isProSkin
                ? (Color) new Color32(41, 41, 41, 255)
                : (Color) new Color32(162, 162, 162, 255);
        }

        public static void FillUsingDefaultColor(Texture2D texture, bool proSkin)
        {
            if (texture == null)
            {
                return;
            }

            var color = GetDefaultSkinColor(proSkin);
            var w = texture.width;
            var h = texture.height;
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }

            texture.Apply();
        }

        public static void FillUsingColor(Texture2D texture, Color color)
        {
            if (texture == null)
            {
                return;
            }

            var w = texture.width;
            var h = texture.height;
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }

            texture.Apply();
        }
    }
}