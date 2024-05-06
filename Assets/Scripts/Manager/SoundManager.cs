using H2910.Common.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace H2910.GameCore
{
    public class SoundManager : ManualSingletonMono<SoundManager>
    {
        [SerializeField] AudioSource music;
        [SerializeField] AudioSource sfx;
        [SerializeField] AudioSource sfx1;
        [SerializeField] AudioSource sfx2;

        [Header("Back Ground")]
        [SerializeField] Sound backgroundMusic;
        [Header("Common")]
        [SerializeField] Sound buttonClick;
        [Header("Game Play")]
        [SerializeField] Sound GetCoin;
        [SerializeField] Sound Jump;
        [SerializeField] Sound Climb;
        [SerializeField] Sound Die;

        void Start()
        {
            ToggleOnOff();

            if (backgroundMusic.audioClip) PlayMusic(backgroundMusic);
        }

        public void ToggleOnOff()
        {
            /*music.mute = !GameData.Music;
            sfx.mute = !GameData.Sound;
            sfx1.mute = !GameData.Sound;
            sfx2.mute = !GameData.Sound;*/
        }

        public void PlaySfx(Sound sound)
        {
            /*if (sound.audioClip == null)
            {
                return;
            }
            if (!GameData.Sound) return;
            sfx.volume = sound.volume;
            sfx.PlayOneShot(sound.audioClip);*/
        }

        public void PlaySfx1(Sound sound, bool loop = false)
        {
            if (sound.audioClip == null)
            {
                Debug.LogError("No audio clip!!!");
                return;
            }
            //if (!GameData.Sound) return;
            sfx1.PlayOneShot(sound.audioClip, sound.volume);
        }

        public void PlaySfx2(Sound sound, bool loop = false)
        {
            if (sound.audioClip == null)
            {
                Debug.LogError("No audio clip!!!");
                return;
            }
            //if (!GameData.Sound) return;
            sfx2.clip = sound.audioClip;
            sfx2.Play();
            sfx2.loop = loop;
        }

        public void PlayMusic()
        {
            if (!music.isPlaying)
            {
                music.Play();
            }
        }
        public void PauseMusic()
        {
            if (music.isPlaying)
            {
                music.Pause();
            }
        }

        public void PlayMusic(Sound sound)
        {
            if (sound.audioClip == null)
            {
                //Dacoder.LogError("No audio clip!!!");
                return;
            }

            //if (!GameData.Music) return;

            music.clip = sound.audioClip;
            music.volume = sound.volume;
            music.loop = true;
            music.Play();
        }

        public void PlayButtonClick()
        {
            PlaySfx(buttonClick);
        }

        public void PlayGetCoin()
        {
            PlaySfx(GetCoin);
        }

        public void PlayPlayerJump()
        {
            PlaySfx(Jump);
        }

        public void PlayPlayerDie()
        {
            PlaySfx(Die);
        }

        public void PlayPlayerClimb()
        {
            PlaySfx(Climb);
        }

        public void PlayBackGroundMusic()
        {
            PlayMusic(backgroundMusic);
        }

        public void Stop()
        {
            if (!sfx2.clip) return;
            sfx2.clip = null;
            sfx2.loop = false;
        }
    }
    [System.Serializable]
    public class Sound
    {
        public AudioClip audioClip;
        [Range(0, 1)] public int volume = 1;
    }
}