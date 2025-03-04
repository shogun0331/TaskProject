#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using QuickEye.Utility;
using System.Text.RegularExpressions;

namespace GB
{
    public class EditorResources : EditorWindow
    {






        [MenuItem("GB/Core/Resources")]
        static void init()
        {
            EditorWindow.GetWindow(typeof(EditorResources));
        }

        private void OnEnable()
        {

            Load();

            
            _iconPath = resourcesData.spritePath ;
            _prefabPath =resourcesData.prefabPath;
            _audioClipPath = resourcesData.audioClipPath;

        }

        private void OnFocus()
        {
            Load();

            // Debug.Log( RES_SPRITE.AS.XX.GetFolder);
        }

        void Load()
        {
            resourcesData = Resources.Load<ResourcesData>("ResourcesData");

            if (resourcesData == null) 
            {
                Debug.Log("ResourcesData : null");
                resourcesData = new ResourcesData();
            }

            _serializedObject = new SerializedObject(resourcesData);
            _mySprites = _serializedObject.FindProperty("Sprites");
            _myAudioClips = _serializedObject.FindProperty("AudioClips");
            _myPrefabs = _serializedObject.FindProperty("Prefabs");

            

        }

        void Save(bool isScript)
        {
            resourcesData = ScriptableObject.CreateInstance<ResourcesData>();

            if (!string.IsNullOrEmpty(_iconPath)) resourcesData.Sprites = TryGetAsset<Sprite>(_iconPath);
            if (!string.IsNullOrEmpty(_audioClipPath)) resourcesData.AudioClips = TryGetAsset<AudioClip>(_audioClipPath);
            if (!string.IsNullOrEmpty(_prefabPath)) resourcesData.Prefabs = TryGetAsset<GameObject>(_prefabPath);

            resourcesData.spritePath = _iconPath;
            resourcesData.prefabPath = _prefabPath;
            resourcesData.audioClipPath = _audioClipPath;


            string path = Application.dataPath + "/" + "Resources/";
            var info = new DirectoryInfo(path);
            if (info.Exists == false) info.Create();
            AssetDatabase.CreateAsset(resourcesData, "Assets/Resources/ResourcesData.asset"); // 파일 생성
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = resourcesData;

            _serializedObject = new SerializedObject(resourcesData);
            _mySprites = _serializedObject.FindProperty("Sprites");
            _myAudioClips = _serializedObject.FindProperty("AudioClips");
            _myPrefabs = _serializedObject.FindProperty("Prefabs");


            if(isScript)
            {
                CreateScript();

            }

            

        }

        void CreateScript()
        {
            if (resourcesData == null) return;

            string ClassForme = @"
public struct RES_@R_TYPE
{
    public string path;
    @PROPERTY
}
";
    string propertyForme = @"
    public static RES_@R_TYPE @PROPERTY = new RES_@R_TYPE{path = ""@PATH""};
    ";

//  public static RES_SPRITE AS_gray_01_armory = new RES_SPRITE{path = "AS/gray_01_armory"};
//  public static RES_@R_TYPE @PROPERTY = new RES_@R_TYPE{path = "@PATH"};

        string data = string.Empty;

            string resType = "SPRITE";
            string structData = ClassForme.Replace("@R_TYPE",resType);
            string propertyDatas = string.Empty;

            foreach(var v in resourcesData.Sprites)
            {
                // string p = v.Key.Replace("/","_").Replace(" ","_");
                string p = Regex.Replace(v.Key, @"[^a-zA-Z0-9]|\s", "_");   
                string path = v.Key;
                propertyDatas += propertyForme.Replace("@PROPERTY",p).Replace("@PATH",path).Replace("@R_TYPE",resType);

            }

            structData = structData.Replace("@PROPERTY",propertyDatas);

            data += structData;

            resType = "PREFAB";
            structData = ClassForme.Replace("@R_TYPE",resType);
            propertyDatas = string.Empty;

            foreach(var v in resourcesData.Prefabs)
            {
                // string p = v.Key.Replace("/","_").Replace(" ","_");
                string p = Regex.Replace(v.Key, @"[^a-zA-Z0-9]|\s", "_");    
                string path = v.Key;
                propertyDatas += propertyForme.Replace("@PROPERTY",p).Replace("@PATH",path).Replace("@R_TYPE",resType);

            }

            structData = structData.Replace("@PROPERTY",propertyDatas);

            data += structData;


            
            resType = "AUDIO";
            structData = ClassForme.Replace("@R_TYPE",resType);
            propertyDatas = string.Empty;

            foreach(var v in resourcesData.AudioClips)
            {
                // string p = v.Key.Replace("/","_").Replace(" ","_");
                    // string p = v.Key.Replace("/","_").Replace(" ","_").Replace("-","_");
               string p = Regex.Replace(v.Key, @"[^a-zA-Z0-9]|\s", "_");    

                string path = v.Key;
                propertyDatas += propertyForme.Replace("@PROPERTY",p).Replace("@PATH",path).Replace("@R_TYPE",resType);
            }

            structData = structData.Replace("@PROPERTY",propertyDatas);

            data += structData;

            string savePath = Application.dataPath + "/" + "GB/ResManager";
            DirectoryInfo info = new DirectoryInfo(savePath);
            if (info.Exists == false)
            info.Create();

            System.IO.File.WriteAllText(savePath + "/" + "RES_struct.cs", data);


            

            UnityEditor.AssetDatabase.Refresh();

        }



        private SerializedObject _serializedObject;
        private SerializedProperty _mySprites;

        private SerializedProperty _myAudioClips;

        private SerializedProperty _myPrefabs;



        public ResourcesData resourcesData;
        string _iconPath;
        string _audioClipPath;
        string _prefabPath;

        public UnityDictionary<string, Sprite> dictIcon = new UnityDictionary<string, Sprite>();
        public UnityDictionary<string, AudioClip> dictAudioClip = new UnityDictionary<string, AudioClip>();
        public UnityDictionary<string, GameObject> dictPrefabClip = new UnityDictionary<string, GameObject>();

        Vector2 pathScroll = new Vector2();
        Vector2 rScroll = new Vector2();

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 20, position.width - 20, position.height - 20));
            GB.EditorGUIUtil.DrawHeaderLabel("GB Resources");
            GB.EditorGUIUtil.Space(5);
            GB.EditorGUIUtil.BackgroundColor(Color.gray);
            GB.EditorGUIUtil.DrawSectionStyleLabel("Path");
            GB.EditorGUIUtil.BackgroundColor(Color.white);

            GB.EditorGUIUtil.Start_VerticalBox();


            //Path

            GB.EditorGUIUtil.Start_Horizontal();
            GB.EditorGUIUtil.DrawLabel("Sprite Path", GUILayout.Width(100));
            GB.EditorGUIUtil.DrawLabel(_iconPath, GUILayout.Width(350));
            if (GB.EditorGUIUtil.DrawButton("Select Folder", GUILayout.Width(100)))
            {
                _iconPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if(!string.IsNullOrEmpty( _iconPath)) Save(false);

            }

            GB.EditorGUIUtil.End_Horizontal();

            GB.EditorGUIUtil.Start_Horizontal();
            GB.EditorGUIUtil.DrawLabel("AudioClip Path", GUILayout.Width(100));
            GB.EditorGUIUtil.DrawLabel(_audioClipPath, GUILayout.Width(350));
            if (GB.EditorGUIUtil.DrawButton("Select Folder", GUILayout.Width(100)))
            {
                _audioClipPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if(!string.IsNullOrEmpty( _audioClipPath)) Save(false);
            }
            GB.EditorGUIUtil.End_Horizontal();

            GB.EditorGUIUtil.Start_Horizontal();
            GB.EditorGUIUtil.DrawLabel("Prefab Path", GUILayout.Width(100));
            GB.EditorGUIUtil.DrawLabel(_prefabPath, GUILayout.Width(350));
            if (GB.EditorGUIUtil.DrawButton("Select Folder", GUILayout.Width(100)))
            {
                _prefabPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if(!string.IsNullOrEmpty( _prefabPath)) Save(false);

            }
            GB.EditorGUIUtil.End_Horizontal();

            GB.EditorGUIUtil.BackgroundColor(Color.green);
            if (GB.EditorGUIUtil.DrawSyleButton("Save"))
            {
                Save(true);

            }

            GB.EditorGUIUtil.BackgroundColor(Color.white);
            GB.EditorGUIUtil.End_Vertical();



            GB.EditorGUIUtil.BackgroundColor(Color.blue);
            GB.EditorGUIUtil.DrawSectionStyleLabel("Resources");
            GB.EditorGUIUtil.BackgroundColor(Color.white);

            GB.EditorGUIUtil.Start_VerticalBox();

            rScroll = GB.EditorGUIUtil.Start_ScrollView(rScroll);

            if (_serializedObject != null)
            {
                _serializedObject.Update();
                if (_mySprites != null) EditorGUILayout.PropertyField(_mySprites);
                if (_myAudioClips != null) EditorGUILayout.PropertyField(_myAudioClips);
                if (_myPrefabs != null) EditorGUILayout.PropertyField(_myPrefabs);
                _serializedObject.ApplyModifiedProperties();
            }



            GB.EditorGUIUtil.End_ScrollView();
            GB.EditorGUIUtil.End_Vertical();










            GUILayout.EndArea();


        }

        public static UnityDictionary<string, T> TryGetAsset<T>(string folderPath, string optionalName = "") where T : UnityEngine.Object
        {
            // Gets all files with the Directory System.IO class
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);


            UnityDictionary<string, T> dict = new UnityDictionary<string, T>();

            // move through all files
            foreach (var file in files)
            {

                string path = FixFilePath(file);

                // Then I try and load the asset at the current path.
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);

                // check the asset to see if it's not null
                if (asset)
                {
                    var sprits = folderPath.Split("/");
                    var fName = sprits[sprits.Length - 1];

                    int idx = file.IndexOf(fName);
                    idx += fName.Length + 1;
                    string paserName = file.Substring(idx, file.Length - idx);

                    sprits = paserName.Split(".");


                    dict[sprits[0].Replace("\\", "/")] = asset;

                }
            }

            Debug.Log("files :" + dict.Count);


            return dict;
        }

        static string FixFilePath(string path)
        {
            if (path.Contains("Assets"))
            {
                int idx = path.IndexOf("Assets");
                int len = path.Length - idx;

                return path.Substring(idx, len);

            }

            return string.Empty;
        }

    }
}

#endif