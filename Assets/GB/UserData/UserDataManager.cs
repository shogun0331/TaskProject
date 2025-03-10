using UnityEngine;
using GB;
using System.IO;
using Newtonsoft.Json;
namespace GB
{

    public class UserDataManager : AutoSingleton<UserDataManager>
    {
        private void Awake()
        {
            if (I != null && I != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        [SerializeField] UserData _current = new UserData();
        public static UserData Current { get { return I._current; } }

        public static bool Load()
        {
            string path = Path.Combine(Application.persistentDataPath, "user_data.data");

            if (File.Exists(path))
            {
                string gz = File.ReadAllText(path); // 파일에서 읽기
                string json = Gzip.DeCompression(gz);
                I._current = JsonConvert.DeserializeObject<UserData>(json);
                return true;
            }
            else { return false; }
        }

        public static bool Load(string json)
        {
            try
            {
                I._current = JsonConvert.DeserializeObject<UserData>(json);
                return true;
            }
            catch { return false; }
        }

        public static void Save()
        {
            if (I._current == null) I._current = new UserData();

            string json = I._current.ToJson();
            string gz = Gzip.Compression(json);
            string path = Path.Combine(Application.persistentDataPath, "user_data.data");
            File.WriteAllText(path, gz); // 파일에 저장
        }
    }
}