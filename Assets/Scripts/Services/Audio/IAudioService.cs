namespace Services.Audio
{
    public interface IAudioService : IService
    {
        bool Muted { get; set; }
        void PlayMusic();
        void PlayBump();
        void PlayDing();
        void PlayFinish();
    }
}