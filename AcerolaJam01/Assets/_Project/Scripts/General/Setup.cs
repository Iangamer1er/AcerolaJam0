#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
public static class Setup{

    [MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders(){
        Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings", "Resources");
        Refresh();
    }

    [MenuItem("Tools/Setup/Import My Favorite Assets")]
    public static void ImportMyFavoriteAssets(){
        Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");
    }

    static class Folders{
        public static void CreateDefault(string root, params string[] folders){
            string fullpath = Combine(Application.dataPath, root);
            foreach (var folder in folders){
                string path = Combine(fullpath, folder);
                if(!Directory.Exists(path)){
                    Directory.CreateDirectory(path);
                }
            }
        }
    }

    public static class Assets{
        public static void ImportAsset(string asset, string subfolder, string folder ="C:/Users/Acer/AppData/Roaming/Unity/Asset Store-5.x"){
            ImportPackage(Combine(folder, subfolder, asset), false);
        }
    }
}
#endif