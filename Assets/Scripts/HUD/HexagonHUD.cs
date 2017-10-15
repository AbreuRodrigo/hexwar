using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexagonHUD : MonoBehaviour
{
    public Text troopMarker;

    public void SetTroop(int amount)
    {
        if(troopMarker != null)
        {
            troopMarker.text = amount.ToString();
        }
    }
}