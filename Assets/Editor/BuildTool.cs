#if UNITY_EDITOR
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;

public class BuildToolWindow : EditorWindow
{
    private string buildPath = "";
    private BuildTarget selectedTarget = BuildTarget.StandaloneWindows64;
    private bool useBuildSettingsScenes = true; // Opción para usar escenas del Build Settings

    [MenuItem("Build/Build Tool Window")]
    public static void ShowWindow()
    {
        GetWindow<BuildToolWindow>("Build Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Build Settings", EditorStyles.boldLabel);

        // Opción para seleccionar si se usan las escenas del Build Settings o un selector de escenas
        useBuildSettingsScenes = EditorGUILayout.Toggle("Use Build Settings Scenes", useBuildSettingsScenes);

        // Select platform for build
        selectedTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", selectedTarget);

        // Choose output folder
        if (GUILayout.Button("Select Build Folder"))
        {
            buildPath = EditorUtility.SaveFolderPanel("Choose Build Folder", "", "");
        }

        GUILayout.Label("Build Path: " + (string.IsNullOrEmpty(buildPath) ? "Not Selected" : buildPath));

        // Build button
        if (GUILayout.Button("Start Build"))
        {
            if (string.IsNullOrEmpty(buildPath))
            {
                Debug.LogError("Please select a build folder!");
            }
            else
            {
                StartBuildProcess();
            }
        }
    }

    private void StartBuildProcess()
    {
        string suffix = GetSuffixForTarget(selectedTarget);
        if (string.IsNullOrEmpty(suffix))
        {
            Debug.LogError("Invalid build target selected.");
            return;
        }

        // Obtener las escenas para la build
        string[] scenes = useBuildSettingsScenes ? GetScenesFromBuildSettings() : GetSelectedScenes();

        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes selected for the build.");
            return;
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = Path.Combine(buildPath, "Build" + suffix),
            target = selectedTarget,
            options = BuildOptions.None
        };

        // Start the build process
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (report.summary.result == BuildResult.Succeeded)
        {
            // Compress the build into a .zip file
            string zipPath = Path.Combine(buildPath, "Build" + suffix + ".zip");
            ZipFile.CreateFromDirectory(buildPlayerOptions.locationPathName, zipPath);
            Debug.Log("Build and compression completed: " + zipPath);
        }
        else
        {
            Debug.LogError("Build failed!");
        }
    }

    private string GetSuffixForTarget(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "_WIN";
            case BuildTarget.iOS:
                return "_IOS";
            case BuildTarget.Android:
                return "_APK";
            case BuildTarget.WebGL:
                return "_WEB";
            default:
                return null;
        }
    }

    // Opción 1: Obtener las escenas configuradas en el Build Settings
    private string[] GetScenesFromBuildSettings()
    {
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        return scenes.ToArray();
    }

    // Opción 2: Implementar un selector manual de escenas
    private string[] GetSelectedScenes()
    {
        List<string> selectedScenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (GUILayout.Toggle(scene.enabled, scene.path))
            {
                selectedScenes.Add(scene.path);
            }
        }
        return selectedScenes.ToArray();
    }
}
#endif
