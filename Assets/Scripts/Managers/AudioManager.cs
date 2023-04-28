using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .2f;
    [Range(.1f, 3f)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class SoundGroup
{
    public string name;
    public List<Sound> sounds;
    public AudioMixerGroup mixerGroup;
}


public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager instance;

    #endregion

    public AudioMixer masterMixer;

    public Dictionary<string, List<Sound>> soundGroupsDict;
    [SerializeField] private float crossfadeSpeed;

    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private List<SoundGroup> soundGroups;
    //[SerializeField] private List<Sound> sounds;
    //[SerializeField] private List<Sound> themes;
    //[SerializeField] private List<SoundGroup> footsteps;

    private Sound currMusic;
    private List<Sound> currStoppingSounds;

    private void Awake()
    {
        currStoppingSounds = new List<Sound>();
        soundGroupsDict = new Dictionary<string, List<Sound>>();
        instance = this;

        foreach (SoundGroup group in soundGroups)
        {
            foreach (Sound sound in group.sounds)
            {
                CreateSound(sound, group.mixerGroup);
            }
            soundGroupsDict.Add(group.name, group.sounds);
        }

        /*foreach (Sound sound in sounds)
        {
            CreateSound(sound, sfxGroup);
        }
        soundGroupsDict.Add("sfx", sounds);

        foreach (Sound theme in themes)
        {
            CreateSound(theme, musicGroup);
        }
        soundGroupsDict.Add("music", themes);*/

        /*foreach (SoundGroup footstepGroup in footsteps)
        {
            foreach (Sound footstep in footstepGroup.sounds)
            {
                CreateSound(footstep, sfxGroup);
            }
            soundGroupsDict.Add(footstepGroup.name, footstepGroup.sounds);
        }*/
    }

    private void Start()
    {
        if (soundGroupsDict["Music"].Count > 0)
            soundGroupsDict["Music"][0].source.Play();
    }

    private void Update()
    {
        #region Starting Sounds
        foreach (SoundGroup soundGroup in soundGroups)
        {
            foreach (Sound sound in soundGroup.sounds)
            {
                if (currStoppingSounds.Contains(sound))
                    continue;

                if (sound.source.isPlaying && sound.source.volume < sound.volume)
                {
                    sound.source.volume += Time.deltaTime * crossfadeSpeed;
                    sound.source.volume = Mathf.Clamp(sound.source.volume, 0, sound.volume);
                }
            }
        }
        #endregion

        #region Stopping Sounds
        List<Sound> stoppedSounds = new List<Sound>();
        for (int i = 0; i < currStoppingSounds.Count; i++)
        {
            currStoppingSounds[i].source.volume -= Time.deltaTime * crossfadeSpeed;
            if (currStoppingSounds[i].source.volume <= 0)
            {
                currStoppingSounds[i].source.Stop();
                stoppedSounds.Add(currStoppingSounds[i]);
            }
        }

        for (int i = 0; i < stoppedSounds.Count; i++)
        {
            currStoppingSounds.Remove(stoppedSounds[i]);
        }
        #endregion
    }

    private Sound FindSound(string group, string name)
    {
        if (!soundGroupsDict.ContainsKey(group))
        {
            Debug.LogError("Sound group: " + group + " not found!");
            return null;
        }

        foreach (Sound sound in soundGroupsDict[group])
        {
            if (sound.name == name)
                return sound;
        }

        Debug.LogError("Sound: " + name + " not found!");
        return null;
    }

    public void PlayRandomFootstep(string group)
    {
        if (!soundGroupsDict.ContainsKey(group))
        {
            Debug.LogError("Sound group: " + group + " not found!");
            return;
        }

        List<Sound> footstepSounds = soundGroupsDict[group];
        int randomSoundIndex = Random.Range(0, footstepSounds.Count);

        PlayFromGroup(group, footstepSounds[randomSoundIndex].name, footstepSounds[randomSoundIndex].volume);
    }

    public void PlaySound(string name)
    {
        Sound s = FindSound("sfx", name);
        if (s == null)
            return;

        s.source.Play();
    }

    public void PlayFromGroup(string group, string name, float defaultVolume)
    {
        if (!soundGroupsDict.ContainsKey(group))
        {
            Debug.LogError("Sound group: " + group + " not found!");
            return;
        }

        Sound s = FindSound(group, name);
        if (s == null)
            return;

        s.source.volume = defaultVolume;
        s.source.Play();
    }

    public void PlayOnceFromGroup(string group, string name)
    {
        if (!soundGroupsDict.ContainsKey(group))
        {
            Debug.LogError("Sound group: " + group + " not found!");
            return;
        }

        Sound s = FindSound(group, name);
        if (s == null)
            return;

        if (s.source.isPlaying)
            return;

        s.source.volume = 0f;
        s.source.Play();
    }

    public void StopFromGroup(string group, string name)
    {
        if (!soundGroupsDict.ContainsKey(group))
        {
            Debug.LogError("Sound group: " + group + " not found!");
            return;
        }

        Sound s = FindSound(group, name);
        if (s == null)
            return;

        if (currStoppingSounds.Contains(s))
            return;

        currStoppingSounds.Add(s);
    }

    private void CreateSound(Sound sound, AudioMixerGroup mixerGroup)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.outputAudioMixerGroup = mixerGroup;

        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }
}
