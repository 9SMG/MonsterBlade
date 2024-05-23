using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    MASTER,
    BGM,
    EFFECT,
    VOICE
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    private SoundManager() { }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject soundManagerObject = new GameObject("SoundManager");
                    instance = soundManagerObject.AddComponent<SoundManager>();
                    DontDestroyOnLoad(soundManagerObject);
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    float soundLength;
    bool bgmEnd = false;

    [SerializeField] private AudioMixer sAudioMixer;
    [SerializeField] private AudioClip[] sBgmClips;
    [SerializeField] private AudioClip[] sEffectClips;
    [SerializeField] private AudioClip[] sVoiceClips;
    private float mCurrentBGMVolume, mCurrentEffectVolume;
    private Dictionary<string, AudioClip> mClipsDictionary;
    private List<TemporarySoundPlayer> mInstantiatedSounds;

    private void Start()
    {
        if (instance == null)
            instance = Instance;
        if (instance != this)
            Destroy(this.gameObject);

        mClipsDictionary = new Dictionary<string, AudioClip>();

        AddClipsToDictionary(sBgmClips, "BGM");
        AddClipsToDictionary(sEffectClips, "Effect");
        AddClipsToDictionary(sVoiceClips, "Voice");

        mInstantiatedSounds = new List<TemporarySoundPlayer>();
    }

    private void AddClipsToDictionary(AudioClip[] clips, string clipType)
    {
        if (clips == null)
        {
            Debug.LogError(clipType + " clips array is null.");
            return;
        }

        foreach (AudioClip clip in clips)
        {
            if (clip == null)
            {
                Debug.LogWarning(clipType + " clips array contains a null clip.");
                continue;
            }

            if (!mClipsDictionary.ContainsKey(clip.name))
            {
                mClipsDictionary.Add(clip.name, clip);
            }
            else
            {
                Debug.LogWarning("Duplicate clip name: " + clip.name + " in " + clipType + " clips array.");
            }
        }
    }

    public AudioClip GetClip(string clipName)
    {
        AudioClip clip = mClipsDictionary[clipName];

        if (clip == null)
        {
            Debug.LogError(clipName + "이 존재하지 않습니다.");
        }

        return clip;
    }

    private void AddToList(TemporarySoundPlayer soundPlayer)
    {
        mInstantiatedSounds.Add(soundPlayer);
    }

    public void StopLoopSound(string clipName)
    {
        foreach (TemporarySoundPlayer audioPlayer in mInstantiatedSounds)
        {
            if (audioPlayer.ClipName == clipName)
            {
                mInstantiatedSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }

        Debug.LogWarning(clipName + "을 찾을 수 없습니다.");
    }

    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 2D");
        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(sAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    public void PlaySound3D(string clipName, Transform audioTarget, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT, bool attachToTarget = true, float minDistance = 0.0f, float maxDistance = 50.0f)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 3D");
        obj.transform.localPosition = audioTarget.transform.position;
        if (attachToTarget) { obj.transform.parent = audioTarget; }

        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound3D(GetClip(clipName), minDistance, maxDistance);
        soundPlayer.Play(sAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.EFFECT, effect);
    }

    public void SetVolume(SoundType type, float value)
    {
        sAudioMixer.SetFloat(type.ToString(), value);
    }

    public static string Range(int from, int includedTo, bool isStartZero = false)
    {
        if (includedTo > 100 && isStartZero) { Debug.LogWarning("0을 포함한 세자리는 지원하지 않습니다."); }

        int value = UnityEngine.Random.Range(from, includedTo + 1);

        return value < 10 && isStartZero ? '0' + value.ToString() : value.ToString();
    }
}