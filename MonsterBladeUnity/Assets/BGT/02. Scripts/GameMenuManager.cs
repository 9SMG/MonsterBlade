using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public Slider mBGMSoundSlider;
    public Slider mEffectSoundSlider;
    public Slider mMasterSoundSlider;
    public Slider mVoiceSoundSlider;


    public void BGMVolume()
    {
        SoundManager.Instance.SetVolume(SoundType.BGM, mBGMSoundSlider.value);
    }
    public void EffectVolume()
    {
        SoundManager.Instance.SetVolume(SoundType.EFFECT, mEffectSoundSlider.value);
    }

    public void MasterVolume()
    {
        SoundManager.Instance.SetVolume(SoundType.MASTER, mMasterSoundSlider.value);
    }

    public void VoiceVolume()
    {
        SoundManager.Instance.SetVolume(SoundType.VOICE, mVoiceSoundSlider.value);
    }
}
