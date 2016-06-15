// Builds an asset bundle from the selected objects in the project view,
// and changes the texture format using an AssetPostprocessor.
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class BuildAssetBundle
{

	// Store current texture format for the TextureProcessor.
	public static TextureImporterFormat textureFormat;

	[MenuItem("AssetBundle/Build Resources Together From Selection - PVRTC_RGB4 - Android")]
    static void ExportResourceTogetherRGB4()
    {
		textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedResource(BuildTarget.Android);
	}

    [MenuItem("AssetBundle/Build Resources Together From Selection - PVRTC_RGB4 - IOS")]
    static void ExportResourceTogetherRGB4_IOS()
    {
		textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedResource(BuildTarget.iOS);
	}

    [MenuItem("AssetBundle/Build Resources Together From Selection - PVRTC_RGB4 - PC")]
    static void ExportResourceTogetherRGB4_Windows()
    {
		textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedResource(BuildTarget.StandaloneWindows);
	}

    [MenuItem("AssetBundle/Build Resources Partly From Selection - PVRTC_RGB4 - Android")]
    static void ExportResourcePartlyRGB4()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedResource(BuildTarget.Android,false);
    }

    [MenuItem("AssetBundle/Build Resources Partly From Selection - PVRTC_RGB4 - IOS")]
    static void ExportResourcePartlyRGB4_IOS()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedResource(BuildTarget.iOS, false);
    }

    [MenuItem("AssetBundle/Build Resources Partly From Selection - PVRTC_RGB4 - PC")]
    static void ExportResourcePartlyRGB4_Windows()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedResource(BuildTarget.StandaloneWindows, false);
    }	

    [MenuItem("AssetBundle/Build One Folder Together From Selection - PVRTC_RGB4 - Android")]
    static void ExportSubDirResourceRGB4_Android()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedFolder(BuildTarget.Android);
    }

    [MenuItem("AssetBundle/Build One Folder Together From Selection - PVRTC_RGB4 - IOS")]
    static void ExportSubDirResourceRGB4_IOS()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedFolder(BuildTarget.iOS);
    }

    [MenuItem("AssetBundle/Build One Folder Together From Selection - PVRTC_RGB4 - PC")]
    static void ExportSubDirResourceRGB4_Windows()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedFolder(BuildTarget.StandaloneWindows);
    }

    [MenuItem("AssetBundle/Build Folders Partly From Selection - PVRTC_RGB4 - Android")]
    static void ExportSubDirResourceRGB4_AP_Android()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedFolder(BuildTarget.Android, false);
    }

    [MenuItem("AssetBundle/Build Folders Partly From Selection - PVRTC_RGB4 - IOS")]
    static void ExportSubDirResourceRGB4_AP_IOS()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedFolder(BuildTarget.iOS, false);
    }

    [MenuItem("AssetBundle/Build Folders Partly From Selection - PVRTC_RGB4 - PC")]
    static void ExportSubDirResourceRGB4_AP_Windows()
    {
        textureFormat = TextureImporterFormat.PVRTC_RGB4;
        BuildSelectedFolder(BuildTarget.StandaloneWindows, false);
    }	
	


    /// 打包选中资源，可以单选或者多选
	/// </summary>
	/// <param name="bt">平台</param>
	/// <param name="together">是打成一整个包还是打成分开独立的包</param>
	/// <returns></returns>
	static void BuildSelectedResource (BuildTarget bt = BuildTarget.Android,bool together = true )
    {
        string bundleName = "use resource name";
         if ( together && null != Selection.activeObject)
         {
             bundleName = Selection.activeObject.name;
         }
        
        string assetDir = Application.streamingAssetsPath;
        
        if (!Directory.Exists(assetDir))
        {
            assetDir = "";
        }

        string path = EditorUtility.SaveFilePanel("Save Resource", assetDir, bundleName.ToLowerInvariant(), "unity3d");

        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle;

        if ( !string.IsNullOrEmpty(path) && path.Length != 0)
        {
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            if (selection.Length == 0)
            {
                Debug.Log("No object selected!");
                return;
            }

            List<Object> sortsList = new List<Object>();
            for (int i = 0; i < selection.Length; i++)
            {
                sortsList.Add(selection[i]);
            }

            sortsList.Sort((Object x, Object y) =>
            {
                string assetPathX = AssetDatabase.GetAssetPath((UnityEngine.Object)x);
                string assetPathY = AssetDatabase.GetAssetPath((UnityEngine.Object)y);
                return string.Compare(assetPathX, assetPathY);
            });

            selection = sortsList.ToArray();

            foreach (object asset in selection)
            {
                string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object)asset);
                if (asset is Texture2D)
                {
                    AssetDatabase.ImportAsset(assetPath);
                }
            }

            if( together )
            {
                int index_xiegang = path.LastIndexOf("/");
                string folderpath = path.Substring(0, index_xiegang);
                bundleName = path.Substring(index_xiegang + 1);

                Build(selection, folderpath, bundleName, options, bt);
            }
            else
            {
                foreach (Object asset in selection)
                {
                    if (typeof(DefaultAsset) == asset.GetType())
                        continue;

                    int index_xiegang = path.LastIndexOf("/");
                    string folderpath = path.Substring(0, index_xiegang);
                    bundleName = asset.name + ".unity3d";
                    Object[] array = {asset};

                    Build(array, folderpath, bundleName, options, bt);
                }
            }

        }
	}

    /// <summary>
    /// 打包整个文件夹
    /// </summary>
    /// <param name="bt">平台</param>
    /// <param name="together">是否打成一整个包</param>
    /// <returns></returns>
    static void BuildSelectedFolder(BuildTarget bt = BuildTarget.Android, bool together = true)
    {
        string assetDir = Application.streamingAssetsPath;

        if (!Directory.Exists(assetDir))
        {
            assetDir = "";
        }

        string path = EditorUtility.SaveFilePanel("Save Resource", assetDir, "use folder name", "unity3d");
        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle;

        if (path.Length != 0)
        {
            int index_xiegang = path.LastIndexOf("/");
            string folderpath = path.Substring(0, index_xiegang);
            string bundleName = path.Substring(index_xiegang + 1);

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            List<Object> sortsList = new List<Object>();
            for (int i = 0; i < selection.Length; i++)
            {
                sortsList.Add(selection[i]);
            }

            sortsList.Sort((Object x, Object y) =>
            {
                string assetPathX = AssetDatabase.GetAssetPath((UnityEngine.Object)x);
                string assetPathY = AssetDatabase.GetAssetPath((UnityEngine.Object)y);
                return string.Compare(assetPathX, assetPathY);
            });

            selection = sortsList.ToArray();

            string foldername = "";
            List<Object> list = new List<Object>();
            for (int i = 0; i < selection.Length; i++)
            {
                Object asset = selection[i];
                string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object)asset);
                if ( typeof(DefaultAsset) != selection[i].GetType() )
                {
                    list.Add(asset);
                }
                else
                {
                    if (!string.IsNullOrEmpty(foldername) && list.Count > 0)
                    {
                        if (together)
                            continue;

                        Build(list.ToArray(), folderpath, foldername + ".unity3D", options, bt);

                        list.Clear();
                        foldername = "";
                    }

                    foldername = assetPath.Substring(assetPath.LastIndexOf("/") + 1);
                }
            }

            if (!string.IsNullOrEmpty(foldername) && list.Count > 0)
            {
                if (string.Compare(bundleName, "use folder name.unity3d") == 0)
                    bundleName = foldername+".unity3D";

                Build(list.ToArray(), folderpath, bundleName, options, bt);

                list.Clear();
                foldername = "";
            }

            if (string.IsNullOrEmpty(foldername))
                Debug.Log("No folder selected!");

            Selection.objects = selection;
        }
    }

    static void Build( Object[] selects,string path,string bundlename,BuildAssetBundleOptions option,BuildTarget target )
    {
        string[] names = new string[selects.Length];
        int i = 0;
        foreach (Object obj in selects)
        {
            names[i++] = AssetDatabase.GetAssetPath((UnityEngine.Object)obj);

        }
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = bundlename;
        build.assetNames = names;

        AssetBundleBuild[] builds = { build };

        BuildPipeline.BuildAssetBundles(path, builds, option, target);
    }


}

