using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public SoundEffectType soundEffectType { get; }
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }

     TMP_Text musicSldierText;
     TMP_Text effectsSldierText;
    [SerializeField] AudioMixerGroup musicMixerGroup;
    [SerializeField] AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] Sound[] sounds;
    AudioOptionsManager audioOptionsManager;
    int levelOfScene = 0;
    private void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            audioOptionsManager = FindObjectOfType<AudioOptionsManager>();
            Stop(SoundEffectType.mainMenuTheme);
            audioOptionsManager.musicSlider.value = musicVolume;
            audioOptionsManager.effectsSlider.value = soundEffectsVolume;
            audioOptionsManager.musicTextValue.text = ((int)(musicVolume * 100)).ToString();
            audioOptionsManager.effectsTextValue.text = ((int)(soundEffectsVolume * 100)).ToString();
            musicSldierText = audioOptionsManager.musicTextValue;
            effectsSldierText = audioOptionsManager.effectsTextValue;
            audioOptionsManager.musicSlider.onValueChanged.AddListener(OnMusicSliderValueChange);
            audioOptionsManager.effectsSlider.onValueChanged.AddListener(OnEffectsSliderValueChange);
        }
        if (level == 2)
        {
            audioOptionsManager = FindObjectOfType<AudioOptionsManager>();
            OnMusicSliderValueChange(musicVolume);
            OnEffectsSliderValueChange(soundEffectsVolume);
            levelOfScene = 0;
        }
        if(level == 0)
        {
            audioOptionsManager = FindObjectOfType<AudioOptionsManager>();
            musicSldierText = audioOptionsManager.musicTextValue;
            effectsSldierText = audioOptionsManager.effectsTextValue;
            audioOptionsManager.musicSlider.onValueChanged.AddListener(OnMusicSliderValueChange);
            audioOptionsManager.effectsSlider.onValueChanged.AddListener(OnEffectsSliderValueChange);
            levelOfScene += 1;
            Stop(SoundEffectType.gameWin);
            Stop(SoundEffectType.gameLose);
            
        }
    }
    private void Start()
    {
        if(levelOfScene == 0)
        {
            audioOptionsManager = FindObjectOfType<AudioOptionsManager>();
            musicSldierText = audioOptionsManager.musicTextValue;
            effectsSldierText = audioOptionsManager.effectsTextValue;
            audioOptionsManager.musicSlider.onValueChanged.AddListener(OnMusicSliderValueChange);
            audioOptionsManager.effectsSlider.onValueChanged.AddListener(OnEffectsSliderValueChange);
            levelOfScene += 1;
        }
       
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
      
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.AudioClip;
            s.source.loop = s.IsLoop;
            s.source.volume = s.Volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
            if (s.PlayOnAwake)
            {
                s.source.Play();
            }
        }
    }
    public void Play(SoundEffectType clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does NOT exist!");
            return;
        }
        s.source.Play();
    }

    public void Stop(SoundEffectType clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does NOT exist!");
            return;
        }
        s.source.Stop();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        musicSldierText.text = ((int)(value * 100)).ToString();
        UpdateMixerVolume();
    }
    public void OnEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        effectsSldierText.text = ((int)(value * 100)).ToString();
        UpdateMixerVolume();
    }
    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume)*20);
        soundEffectsMixerGroup.audioMixer.SetFloat("Effect Volume", Mathf.Log10(soundEffectsVolume) * 20);
    }


}
