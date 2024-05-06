using H2910.Common.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace H2910.Base
{
    public class SoundManager : ManualSingletonMono<SoundManager>
    {
        public bool IgnoreDuplicateMusic { get; set; }
        public bool IgnoreDuplicateSounds { get; set; }
        public bool IgnoreDuplicateUISounds { get; set; }
        public float GlobalVolume { get; set; }
        public float GlobalVMusicVolume { get; set; }
        public float GlobalSoundsVolume { get; set; }
        public float GlobalUISoundsVolume { get; set; }

        private Dictionary<int, Audio> _musicAudio = new Dictionary<int, Audio>();
        private Dictionary<int, Audio> _soundsAudio = new Dictionary<int, Audio>();
        private Dictionary<int, Audio> _UISoundsAudio = new Dictionary<int, Audio>();
        private Dictionary<int, Audio> _audioPool = new Dictionary<int, Audio>();

        private bool _initialized = false;

        public void Start()
        {
            Init();
            AudioSettings.OnAudioConfigurationChanged += OnBluetoothHeadphoneConnect;
        }

        private void OnBluetoothHeadphoneConnect(bool deviceWasChanged)
        {
            
        }

        private void Init()
        {
            if (!_initialized)
            {
                GlobalVolume = 1;
                GlobalVMusicVolume = 1;
                GlobalSoundsVolume = 1;
                GlobalUISoundsVolume = 1;

                IgnoreDuplicateMusic = false;
                IgnoreDuplicateSounds = false;
                IgnoreDuplicateUISounds = false;

                _initialized = true;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RemoveNonPersistAudio(_musicAudio);
            RemoveNonPersistAudio(_soundsAudio);
            RemoveNonPersistAudio(_UISoundsAudio);
        }

        private void Update()
        {
            UpdateAllAudio(_musicAudio);
            UpdateAllAudio(_soundsAudio);
            UpdateAllAudio(_UISoundsAudio);
        }

        private Dictionary<int, Audio> GetAudioTypeDictionary(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = new Dictionary<int, Audio>();
            switch (audioType)
            {
                case Audio.AudioType.Music:
                    audioDict = _musicAudio;
                    break;
                case Audio.AudioType.Sound:
                    audioDict = _soundsAudio;
                    break;
                case Audio.AudioType.UISound:
                    audioDict = _UISoundsAudio;
                    break;
            }

            return audioDict;
        }

        private bool GetAudioTypeIgnoreDuplicateSetting(Audio.AudioType audioType)
        {
            switch (audioType)
            {
                case Audio.AudioType.Music:
                    return IgnoreDuplicateMusic;
                case Audio.AudioType.Sound:
                    return IgnoreDuplicateSounds;
                case Audio.AudioType.UISound:
                    return IgnoreDuplicateUISounds;
                default:
                    return false;
            }
        }

        private void UpdateAllAudio(Dictionary<int, Audio> audioDict)
        {
            List<int> keys = new List<int>(audioDict.Keys);
            foreach (var key in keys)
            {
                Audio audio = audioDict[key];
                audio.Update();
                if (!audio.IsPlaying && !audio.Paused)
                {
                    Destroy(audio.AudioSource);
                    _audioPool.Add(key,audio);
                    audio.Pooled = true;
                    audioDict.Remove(key);
                }
            }
        }

        private void RemoveNonPersistAudio(Dictionary<int, Audio> audioDict)
        {
            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                if (!audio.Persist && audio.Activated)
                {
                    Destroy(audio.AudioSource);
                    audioDict.Remove(key);
                }
            }
            
            keys = new List<int>(_audioPool.Keys);
            foreach (int key in keys)
            {
                Audio audio = _audioPool[key];
                if (!audio.Persist && audio.Activated)
                {
                    _audioPool.Remove(key);
                }
            }
        }
        
        public bool RestoreAudioFromPool(Audio.AudioType audioType, int audioID)
        {
            if (_audioPool.ContainsKey(audioID))
            {
                Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);
                audioDict.Add(audioID, _audioPool[audioID]);
                _audioPool.Remove(audioID);

                return true;
            }

            return false;
        }
        
        #region GetAudio Functions

        public Audio GetAudio(int audioID)
        {
            Audio audio;

            audio = GetMusicAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            audio = GetSoundAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            audio = GetUISoundAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            return null;
        }
        
        public Audio GetAudio(AudioClip audioClip)
        {
            Audio audio = GetMusicAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            audio = GetSoundAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            audio = GetUISoundAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            return null;
        }
        
        public Audio GetMusicAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.Music, true, audioID);
        }
        
        public Audio GetMusicAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.Music, true, audioClip);
        }
        
        public Audio GetSoundAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.Sound, true, audioID);
        }
        
        public Audio GetSoundAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.Sound, true, audioClip);
        }
        
        public Audio GetUISoundAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.UISound, true, audioID);
        }
        
        public Audio GetUISoundAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.UISound, true, audioClip);
        }

        private Audio GetAudio(Audio.AudioType audioType, bool usePool, int audioID)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            if (audioDict.ContainsKey(audioID))
            {
                return audioDict[audioID];
            }

            if (usePool && _audioPool.ContainsKey(audioID) && _audioPool[audioID].Type == audioType)
            {
                return _audioPool[audioID];
            }

            return null;
        }

        private Audio GetAudio(Audio.AudioType audioType, bool usePool, AudioClip audioClip)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> audioTypeKeys = new List<int>(audioDict.Keys);
            List<int> poolKeys = new List<int>(_audioPool.Keys);
            List<int> keys = usePool ? audioTypeKeys.Concat(poolKeys).ToList() : audioTypeKeys;
            foreach (int key in keys)
            {
                Audio audio = null;
                if (audioDict.ContainsKey(key))
                {
                    audio = audioDict[key];
                }
                else if (_audioPool.ContainsKey(key))
                {
                    audio = _audioPool[key];
                }

                if (audio == null)
                {
                    return null;
                }

                if (audio.Clip == audioClip && audio.Type == audioType)
                {
                    return audio;
                }
            }

            return null;
        }

        #endregion

        #region Prepare Function

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name = "persist" > Whether the audio persists in between scene changes</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume, bool loop, bool persist)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, loop, persist, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Prerpares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInValue">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutValue"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds,
            float fadeOutSeconds)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, -1f,
                null);
        }

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInValue">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutValue"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <param name="currentMusicfadeOutSeconds"> How many seconds it needs for current music audio to fade out. It will override its own fade out seconds. If -1 is passed, current music will keep its own fade out seconds</param>
        /// <param name="sourceTransform">The transform that is the source of the music (will become 3D audio). If 3D audio is not wanted, use null</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds,
            float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds,
                currentMusicfadeOutSeconds, sourceTransform);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }


        public int PrepareSound(AudioClip clip, float volume)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip, bool loop)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, 1f, loop, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <param name="sourceTransform">The transform that is the source of the sound (will become 3D audio). If 3D audio is not wanted, use null</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip, float volume, bool loop, Transform sourceTransform)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, volume, loop, false, 0f, 0f, -1f, sourceTransform);
        }

        /// <summary>
        /// Prepares and initializes a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareUISound(AudioClip clip)
        {
            return PrepareAudio(Audio.AudioType.UISound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareUISound(AudioClip clip, float volume)
        {
            return PrepareAudio(Audio.AudioType.UISound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        private int PrepareAudio(Audio.AudioType audioType, AudioClip clip, float volume, bool loop, bool persist,
            float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            if (clip == null)
            {
                Debug.LogError("[Eazy Sound Manager] Audio clip is null", clip);
            }

            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);
            bool ignoreDuplicateAudio = GetAudioTypeIgnoreDuplicateSetting(audioType);

            if (ignoreDuplicateAudio)
            {
                Audio duplicateAudio = GetAudio(audioType, true, clip);
                if (duplicateAudio != null)
                {
                    return duplicateAudio.AudioID;
                }
            }

            // Create the audioSource
            Audio audio = new Audio(audioType, clip, loop, persist, volume, fadeInSeconds, fadeOutSeconds, sourceTransform);

            // Add it to dictionary
            audioDict.Add(audio.AudioID, audio);

            return audio.AudioID;
        }

        #endregion

        #region Play Functions
        
        public int PlayMusic(AudioClip clip)
        {
            return PlayAudio(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, null);
        }
        
        public int PlayMusic(AudioClip clip, float volume)
        {
            return PlayAudio(Audio.AudioType.Music, clip, volume, false, false, 1f, 1f, -1f, null);
        }
        
        public int PlayMusic(AudioClip clip, float volume, bool loop, bool persist)
        {
            return PlayAudio(Audio.AudioType.Music, clip, volume, loop, persist, 1f, 1f, -1f, null);
        }
        
        public int PlayMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds,
            float fadeOutSeconds)
        {
            return PlayAudio(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, -1f, null);
        }


        public int PlayMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds,
            float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            return PlayAudio(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds,
                currentMusicfadeOutSeconds, sourceTransform);
        }
        
        public int PlaySound(AudioClip clip)
        {
            return PlayAudio(Audio.AudioType.Sound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }
        
        public int PlaySound(AudioClip clip, float volume)
        {
            return PlayAudio(Audio.AudioType.Sound, clip, volume, false, false, 0f, 0f, -1f, null);
        }
        
        public int PlaySound(AudioClip clip, bool loop)
        {
            return PlayAudio(Audio.AudioType.Sound, clip, 1f, loop, false, 0f, 0f, -1f, null);
        }
        
        public int PlaySound(AudioClip clip, float volume, bool loop, Transform sourceTransform)
        {
            return PlayAudio(Audio.AudioType.Sound, clip, volume, loop, false, 0f, 0f, -1f, sourceTransform);
        }
        
        public int PlayUISound(AudioClip clip)
        {
            return PlayAudio(Audio.AudioType.UISound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }
        
        public int PlayUISound(AudioClip clip, float volume)
        {
            return PlayAudio(Audio.AudioType.UISound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        private int PlayAudio(Audio.AudioType audioType, AudioClip clip, float volume, bool loop, bool persist,
            float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            int audioID = PrepareAudio(audioType, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds,
                currentMusicfadeOutSeconds, sourceTransform);
            
            if (audioType == Audio.AudioType.Music)
            {
                StopAllMusic(currentMusicfadeOutSeconds);
            }

            GetAudio(audioType, false, audioID).Play();

            return audioID;
        }

        #endregion

        #region Stop Functions

        public void StopAll()
        {
            StopAll(-1f);
        }

        public void StopAll(float musicFadeOutSeconds)
        {
            StopAllMusic(musicFadeOutSeconds);
            StopAllSounds();
            StopAllUISounds();
        }

        public void StopAllMusic()
        {
            StopAllAudio(Audio.AudioType.Music,-1f);
        }

        public void StopAllMusic(float fadeOutSeconds)
        {
            StopAllAudio(Audio.AudioType.Music,fadeOutSeconds);
        }

        public void StopAllSounds()
        {
            StopAllAudio(Audio.AudioType.Sound,-1f);
        }

        public void StopAllUISounds()
        {
            StopAllAudio(Audio.AudioType.UISound,-1f);
        }

        private void StopAllAudio(Audio.AudioType audioType, float fadeOutSeconds)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);
            List<int> keys = new List<int>(audioDict.Keys);
            foreach (var key in keys)
            {
                Audio audio = audioDict[key];
                if (fadeOutSeconds > 0)
                {
                    audio.FadeOutSeconds = fadeOutSeconds;
                }
                audio.Stop();
            }
        }

        #endregion
        
        #region Pause Functions

        public void PauseAll()
        {
            PauseAllMuic();
            PauseAllSounds();
            PauseAllUISounds();
        }
        
        private void PauseAllAudio(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);
            List<int> keys = new List<int>(audioDict.Keys);
            foreach (var key in keys)
            {
                Audio audio = audioDict[key];
                audio.Pause();
            }
        }
        
        public void PauseAllMuic()
        {
            PauseAllAudio(Audio.AudioType.Music);
        }

        public void PauseAllSounds()
        {
            PauseAllAudio(Audio.AudioType.Sound);
        }

        public void PauseAllUISounds()
        {
            PauseAllAudio(Audio.AudioType.UISound);
        }

        #endregion

        #region Resume Functions

        public void ResumeAll()
        {
            ResumeAllMusic();
            ResumeAllUISounds();
            ResumeAllSounds();
        }

        public void ResumeAllMusic()
        {
            ResumeAllAudio(Audio.AudioType.Music);
        }

        public void ResumeAllSounds()
        {
            ResumeAllAudio(Audio.AudioType.Sound);
        }

        public void ResumeAllUISounds()
        {
            ResumeAllAudio(Audio.AudioType.UISound);
        }

        private void ResumeAllAudio(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (var key in keys)
            {
                Audio audio = audioDict[key];
                audio.Resume();
            }
        }


        #endregion
    }
}