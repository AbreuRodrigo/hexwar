/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       23/12/2017 16:40
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    [System.Serializable]
    public class Audio
    {
        public EAudioName name;
        public AudioClip audioFile;
        [Range(0, 1)]
        public float volume;
    }
}