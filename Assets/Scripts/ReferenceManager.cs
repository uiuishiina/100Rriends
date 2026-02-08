using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
    [Header("éQè∆")]
    [SerializeField] public Slider BGMSlider;
    [SerializeField] public Slider SESlider;
    [SerializeField] public AudioSource BGMSource;
    [SerializeField] public AudioSource SESource;

    public AudioSource GetBGMSource()
    {
        return BGMSource;
    }
    public AudioSource GetSESource()
    {
        return SESource;
    }
}