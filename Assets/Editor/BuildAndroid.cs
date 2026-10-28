#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildAndroid
{
    public static void Build()
    {
        PlayerSettings.companyName = "Shaquib Nawaid";
        PlayerSettings.productName = "Blue Army: Ocean Assault";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.shaquib01.bluearmyoceanassault");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;

        PrototypeSceneBuilder.BuildForCI();
        // Scene generation imports assets and refreshes editor state; set build-critical
        // Android settings after it so the final BuildPlayer call sees them.
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        if ((PlayerSettings.Android.targetArchitectures & AndroidArchitecture.ARM64) == 0)
            throw new InvalidOperationException("Android ARM64 target architecture could not be enabled.");
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
        if (scenes.Length == 0)
            throw new InvalidOperationException("Prototype scene generation did not register an enabled build scene.");

        const string output = "Builds/Android/BlueArmy-OceanAssault.apk";
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(output));
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = BuildTarget.Android,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException("Android build failed: " + report.summary.result + " — " + report.summary.totalErrors + " errors.");
        Debug.Log("Android APK created: " + output + " (" + report.summary.totalSize + " bytes)");
    }
}
#endif
