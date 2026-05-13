using UnityEngine;
using UnityEditor;
using System.IO;

public class ListarNombres : EditorWindow
{
    [MenuItem("Tools/Listar Materiales y Texturas")]
    public static void Listar()
    {
        string texturesPath = "Assets/Casita/Textures";

        string[] texGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { texturesPath });

        string texNames = "=== TEXTURAS ===\n";
        foreach (var g in texGUIDs)
            texNames += Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(g)) + "\n";

        Debug.Log(texNames);
    }
}