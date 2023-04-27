[System.Serializable]
public class SettingsData
{
    public int resolutionIndex;
    public float musicVolume;
    public float sfxVolume;
    public bool fullscreen;

    public SettingsData(SettingsMenu settings)
    {
        resolutionIndex = settings.currentResolutionIndex;
        musicVolume = settings.musicVolume;
        sfxVolume = settings.sfxVolume;
        fullscreen = settings.fullscreenToggle.isOn;
    }
}
