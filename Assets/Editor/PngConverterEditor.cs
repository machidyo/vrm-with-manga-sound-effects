using UnityEditor;
using UnityEngine;
using System.IO;

public class PngConverterEditor : EditorWindow
{
    private Texture2D input;
    private Texture2D output;

    [MenuItem("Tools/Convert PNG")]
    static void ShowWindow()
    {
        var window = GetWindow<PngConverterEditor>();
        window.titleContent = new GUIContent("PNG Converter");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("PNG Converter", EditorStyles.boldLabel);

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("input png:", GUILayout.Width(100));
        input = (Texture2D)EditorGUILayout.ObjectField(input, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Convert"))
        {
            LoadPNG(input);
            SavePNG();
        }

        EditorGUI.EndDisabledGroup();
    }

    private void LoadPNG(Texture2D input)
    {
        if (input == null) return;

        var outputWidth = input.width * 2;
        var outputHeight = input.height * 2;

        output = new Texture2D(outputWidth, outputHeight, TextureFormat.ARGB32, false);

        var transparentColors = new Color[outputWidth * outputHeight];
        for (var i = 0; i < transparentColors.Length; i++)
        {
            transparentColors[i] = Color.clear;
        }

        output.SetPixels(transparentColors);

        for (var y = 0; y < input.height; y++)
        {
            for (var x = 0; x < input.width; x++)
            {
                output.SetPixel(x, outputHeight / 2 + y - 1, input.GetPixel(x, y));
            }
        }

        output.Apply();

        Debug.Log("Complete to load PNG.");
    }

    private void SavePNG()
    {
        if (output == null) return;

        var savePath = EditorUtility.SaveFilePanel("Save PNG", "", "output", "png");
        if (string.IsNullOrEmpty(savePath)) return;
        
        var imageData = output.EncodeToPNG();
        File.WriteAllBytes(savePath, imageData);

        Debug.Log("Complete to save PNG.");
    }
}