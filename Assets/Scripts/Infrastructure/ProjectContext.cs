using Core;
using Core.Loading;
using UnityEngine;

namespace Infrastructure
{
    public class ProjectContext : MonoBehaviour
    {
        public Camera UICamera { get; private set; }
        public PersistentDataProvider PersistentDataProvider { get; private set; }
        public AssetProvider AssetProvider { get; private set; }
        public LoadingScreenProvider LoadingScreenProvider { get; private set; }
        public AudioProvider AudioProvider { get; private set; }
        public EventsProvider EventsProvider { get; private set; }
        public InputProvider InputProvider { get; private set; }
        public AdsProvider AdsProvider { get; private set; }
        public StaticDataProvider StaticDataProvider { get; private set; }

        public static ProjectContext I { get; private set; }

        private void Awake()
        {
            I = this;
            DontDestroyOnLoad(this);
        }

        public void Initialize()
        {
            PersistentDataProvider = new PersistentDataProvider();
            AssetProvider = new AssetProvider();
            LoadingScreenProvider = new LoadingScreenProvider();
            AudioProvider = GetComponentInChildren<AudioProvider>();
            UICamera = GetComponentInChildren<Camera>();
            EventsProvider = new EventsProvider();
            InputProvider = new InputProvider();
            AdsProvider = GetComponentInChildren<AdsProvider>();
            StaticDataProvider = GetComponentInChildren<StaticDataProvider>();
        }

        private void OnDestroy()
        {
            AssetProvider.Dispose();
        }
    }
}