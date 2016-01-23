using UnityEngine;
using Bridger;

public class ResourcesManager : MonoBehaviour
{
    protected ResourcesManager() {}
    private static ResourcesManager _instance;

    private static object _lock = new object();

    public BridgePart[] bridgePartPrefabs;
    public GameObject junctionPrefab;
    private AudioSource _audioMaster;
    public AudioSource audioMaster
    {
        get
        {
            if(_audioMaster == null)
            {
                _audioMaster = GetComponent<AudioSource>();
            }
            if(_audioMaster == null)
            {
                _audioMaster = gameObject.AddComponent<AudioSource>();
            }
            return _audioMaster;

        }
   }


    public static ResourcesManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(ResourcesManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ResourcesManager>();

                    if (FindObjectsOfType(typeof(ResourcesManager)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = Instantiate<GameObject>(Resources.Load<GameObject>("ResourcesManager"));
                        _instance = singleton.GetComponent<ResourcesManager>();
                        singleton.name += "(singleton)";

                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
