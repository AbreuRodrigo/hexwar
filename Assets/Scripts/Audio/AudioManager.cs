/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       23/12/2017 16:01
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Prefabs")]
        public GameObject audioChannelPrefab;

        [Header("Initial Channels")]
        public int initialAudioChannels;

        [Space(10)]
        [Header("Audio lists")]
        [Space(10)]

        [Header("Sound FXs")]
        public Audio[] soundFXs;

        [Header("Music Tracks")]
        public Audio[] musicTracks;

        [Header("Ambience FXs")]
        public Audio[] ambiecenFXs;

        private Dictionary<EAudioName, Audio> audioDictionaries = new Dictionary<EAudioName, Audio>();

        private Dictionary<int, AudioChannel> audioChannelsInUse = new Dictionary<int, AudioChannel>();

        private Stack<AudioChannel> audioChannelsAvailable = new Stack<AudioChannel>();

        public override void Awake()
        {
            base.Awake();

            InitializeAudioDictionaries();
            InitializeAudioSources();
        }
        
        private void InitializeAudioDictionaries()
        {
            foreach (Audio audio in soundFXs)
            {
                audioDictionaries.Add(audio.name, audio);
            }
            foreach (Audio audio in musicTracks)
            {
                audioDictionaries.Add(audio.name, audio);
            }
            foreach (Audio audio in ambiecenFXs)
            {
                audioDictionaries.Add(audio.name, audio);
            }
        }

        private void InitializeAudioSources()
        {
            AudioChannel channel = null;

            for(int i = 0; i < initialAudioChannels; i++)
            {
                channel = CreateNewChannel();
                channel.TurnOn(i, SetChannelAvailable);
                channel.gameObject.SetActive(false);

                audioChannelsAvailable.Push(channel);
            }
        }

        private void PlayOnShot(Audio audio)
        {
            AudioChannel channel = GetNextAvailableChannel();

            if (channel != null)
            {
                channel.PlayOneShot(audio);
            }
        }

        private void PlayTrack(Audio audio)
        {
            AudioChannel channel = GetNextAvailableChannel();

            if (channel != null)
            {
                channel.PlayLoop(audio);
            }
        }

        private AudioChannel GetNextAvailableChannel()
        {
            if (audioChannelsAvailable != null && audioChannelsAvailable.Count > 0)
            {
                return audioChannelsAvailable.Pop();
            }

            return null;
        }

        private void SetChannelInUse(AudioChannel channel)
        {
            if (audioChannelsInUse != null)
            {
                channel.gameObject.SetActive(true);
                audioChannelsInUse.Add(channel.index, channel);
            }
        }

        private AudioChannel CreateNewChannel()
        {
            return Instantiate(audioChannelPrefab, transform).GetComponent<AudioChannel>();
        }

        public void SetChannelAvailable(AudioChannel channel)
        {
            if (audioChannelsInUse != null)
            {
                int index = channel.index;

                AudioChannel ch = audioChannelsInUse[index];
                ch.gameObject.SetActive(false);

                audioChannelsInUse.Remove(index);

                audioChannelsAvailable.Push(ch);
            }
        }

        //SOUND FX
        public void PlayButtonClickFx()
        {
            PlayOnShot(audioDictionaries[EAudioName.BUTTON_CLICK]);
        }

        //MUSIC TRACK
        public void PlayMainTheme()
        {
            PlayTrack(audioDictionaries[EAudioName.MAIN_THEME]);
        }

        //AMBIENCE TRACK
        public void PlayMenuAmbience()
        {
            PlayTrack(audioDictionaries[EAudioName.MENU_AMBIENCE]);
        }
    }
}