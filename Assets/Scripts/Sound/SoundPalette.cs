using UnityEngine;
using System.Linq;
using H2910.Common.Singleton;

namespace H2910.Base
{
    public class SoundPalette : ManualSingletonMono<SoundPalette>
    {
        [SerializeField] AudioPalette dicAudio = new AudioPalette();
        [SerializeField] AudioBackGround dicBackGround = new AudioBackGround();

        public AudioClip GetAudioClip(AudioClipType typeClip)
        {
            if (dicAudio.ContainsKey(typeClip))
            {
                return dicAudio[typeClip];
            }

            return dicAudio.FirstOrDefault().Value;
        }
        public AudioClip GetBackGroundMusic(AudioClipType typeClip)
        {
            if (dicBackGround.ContainsKey(typeClip))
            {
                return dicBackGround[typeClip];
            }

            return dicBackGround.FirstOrDefault().Value;
        }
        public void PlayButtonClickSound()
        {
            //SoundManager.Instance.PlaySound(SoundPalette.Instance.GetAudioClip(AudioClipType.ButtonClick));
        }
    }

    [System.Serializable]
    public class AudioPalette : SerializableDictionary<AudioClipType, AudioClip>
    {
    }

    public enum AudioClipType
    {
        BackGroundZone1,
        Win,
        BoomExplode,
        ReachCheckPoint,
        Teleport,
        CollectGold,
        ItemboxBreak,
        ButtonClick,
        ClickNPC,
        CollectFlameTrace,
        GuardCaptainDash,
        PlayerInjured,
        LuckyWheelSpin,
        CollectItem,
        OniDead,
        Defeat,
        BackGroundDefeat,
    }

    [System.Serializable]
    public class AudioBackGround: SerializableDictionary<AudioClipType, AudioClip>
    {
    }
}    
