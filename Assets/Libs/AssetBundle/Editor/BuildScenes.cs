using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class BuildScenes : MonoBehaviour {

    [MenuItem("AssetBundle/Build Added Scene - Android")]
    static void BuildSceneAndroid()
    {
        
        string[] names = FindAddedScenes();
        foreach( string str in names )
        {
            string[] strs = {str};
            string path = Application.dataPath + "/" + GetSceneName(str) + ".unity3d";
            Build(strs, path, BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);
        }
        
    }


    static string GetSceneName( string name )
    {
        int index_xiegang = name.LastIndexOf("/");
        int index_dian = name.LastIndexOf(".");
        return name.Substring(index_xiegang + 1, index_dian - index_xiegang-1);
    }


    static string[] FindAddedScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }


    static void Build(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        //EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        //AssetDatabase.Refresh();
    }
}
