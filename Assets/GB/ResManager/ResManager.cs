using UnityEngine;

namespace GB
{
    public class ResManager : AutoSingleton<ResManager>
    {
        private void Awake() 
        {
            if(I != null && I != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);

        }

        ResourcesData _resourcesData;

        void Load()
        {
            if( I._resourcesData== null)
            _resourcesData = Resources.Load<ResourcesData>("ResourcesData");
        }

        public static Sprite GetSprite(string path)
        {
              I.Load();
            if(I._resourcesData == null) return null;
            if(I._resourcesData.Sprites.ContainsKey(path))
               return I._resourcesData.Sprites[path];

            return null;
        }

        public static AudioClip GetAudioClip(string path)
        {
            I.Load();
            if(I._resourcesData == null) return null;
            if(I._resourcesData.AudioClips.ContainsKey(path))
               return I._resourcesData.AudioClips[path];

            return null;
        }

        public static GameObject GetGameObject(string path)
        {
            I.Load();
            if(I._resourcesData == null) return null;
            if(I._resourcesData.Prefabs.ContainsKey(path))
               return I._resourcesData.Prefabs[path];

            return null;
        }

        
      

        
        
        public static Sprite Get(RES_SPRITE spriteID)
        {
            I.Load();
            if(I._resourcesData == null) return null;
            if(I._resourcesData.Sprites.ContainsKey( spriteID.path))
                return I._resourcesData.Sprites[spriteID.path];

            return null;
        }

        public static AudioClip Get(RES_AUDIO audioID)
        {
            I.Load();
            if(I._resourcesData == null) return null;
            if(I._resourcesData.AudioClips.ContainsKey( audioID.path))
                return I._resourcesData.AudioClips[audioID.path];

            return null;
        }

        public static GameObject Get(RES_PREFAB prefabID)
        {
            I.Load();
            if(I._resourcesData == null) return null;
            if(I._resourcesData.Prefabs.ContainsKey( prefabID.path))
                return I._resourcesData.Prefabs[prefabID.path];
            return null;
        }
        



    }

}
