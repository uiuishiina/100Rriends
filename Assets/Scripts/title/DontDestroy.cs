using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class DontDestroy : MonoBehaviour
{
    public static GameObject Instance;
    [SerializeField, Header("ê›íËÉfÅ[É^")] public GameSetting data_;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this.gameObject;
            DontDestroyOnLoad(Instance);
        }
        if(data_ == null)
        {
            var I = ScriptableObject.CreateInstance("GameSetting");
            data_ = (GameSetting)I;
        }
    }
}
