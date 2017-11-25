using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGamePopUp : MonoBehaviour {

    public RectTransform myRect;
    public Image blockBackground;
    public InputField gameNameInputField;
    public Dropdown mapSizeDropDown;
    public float openTimer = 0.75f;
    public float closeTimer = 0.25f;

    private void OnEnable()
    {
        blockBackground.gameObject.SetActive(true);
        LeanTween.scale(myRect, Vector3.one, openTimer)
                 .setEaseOutElastic();
    }

    private void OnDisable()
    {
        ClosePopUp();
    }

    public void ClosePopUp()
    {
        blockBackground.gameObject.SetActive(false);
        LeanTween.scale(myRect, Vector3.zero, closeTimer)
                 .setOnComplete(Disable);
    }

    public string GetGameName()
    {
        string gameName = "";

        if(gameNameInputField != null)
        {
            gameName = gameNameInputField.text;
        }

        return gameName;
    }

    public string GetMapSize()
    {
        return EMapSize.SMALL.ToString();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
