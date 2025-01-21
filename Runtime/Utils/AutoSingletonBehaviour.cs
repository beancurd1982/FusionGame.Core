using UnityEngine;

namespace FusionGame.Core.Utils
{
    /// <summary>
    /// Derive from this class to create a singleton MonoBehaviour that is automatically created when accessed
    /// Note that the serialized fields won't work because the instance is created at runtime.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                CreateInstance();
                return _instance;
            }
        }

        public static void Initialize()
        {
            CreateInstance();
        }

        private static void CreateInstance()
        {
            if (_instance != null) return;

            var go = new GameObject(typeof(T).Name);
            DontDestroyOnLoad(go);

            _instance = go.AddComponent<T>();
        }
    }
}
