using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "StaticData/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        public AudioClip[] runningMusics;
        public AudioClip[] finishMusics;
        public AudioClip bump;
        public AudioClip ding;
    }
}