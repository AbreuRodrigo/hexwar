using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexwar
{
    public class HexagonHUD : MonoBehaviour
    {
        public Text textVal;

        public void SetValue(int value)
        {
            if (textVal != null)
            {
                textVal.text = value.ToString();
            }
        }
    }
}