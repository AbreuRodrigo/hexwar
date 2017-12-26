/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       23/12/2017 18:16
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hexwar
{
	public class AudioChannel : MonoBehaviour 
	{
        public int index = 0;
        public bool playing = false;
        public AudioSource audioSource;

        private float progress;

        [System.Serializable]
        public class ChannelEvent : UnityEvent<AudioChannel> { }

        private ChannelEvent observer = new ChannelEvent();

        private float timer = 0;
                		
		void Update () 
		{
			if(playing && !audioSource.loop)
            {
                timer += Time.unscaledDeltaTime;
                progress = Mathf.Clamp01(timer / audioSource.clip.length);

                if (progress >= 1)
                {
                    TurnOff();
                    timer = 0;
                }                
            }
		}

        public void TurnOn(int index, UnityAction<AudioChannel> listener)
        {
            if(observer != null)
            {
                this.index = index;
                this.timer = 0;

                observer.RemoveAllListeners();
                observer.AddListener(listener);
            }
        }

        private void TurnOff()
        {
            observer.Invoke(this);
            playing = false;
            audioSource.clip = null;
            gameObject.SetActive(false);
        }

        public void PlayOneShot(Audio audio)
        {
            if (!playing)
            {
                if (audio != null && audioSource != null)
                {
                    playing = true;

                    gameObject.SetActive(true);

                    audioSource.clip = audio.audioClip;
                    audioSource.loop = false;
                    audioSource.playOnAwake = false;
                    audioSource.volume = audio.volume;
                    audioSource.PlayOneShot(audio.audioClip, audio.volume);
                }
            }
        }

        public void PlayLoop(Audio audio)
        {
            if (!playing && audio != null && audioSource != null)
            {
                playing = true;

                gameObject.SetActive(true);

                audioSource.loop = true;
                audioSource.playOnAwake = true;
                audioSource.volume = audio.volume;
                audioSource.clip = audio.audioClip;
                audioSource.Play();
            }
        }
    }
}