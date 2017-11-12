using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelection : MonoBehaviour
{
    [Header("UI")]
    public Button addButton;
    public Button subtractButton;
    public Button confirmMoveButton;

    [Header("Components")]
    public GameObject actionControl;
    public GameObject targetMarker;

    [Header("Action Buttons")]
    public GameObject confirmButton;
    public GameObject actionContainer;

    public void OnAddButtonPressed()
    {
        GameManager.Instance.AddToPreview();
    }

    public void OnSubtractButtonPressed()
    {
        GameManager.Instance.SubtractFromPreview();
    }

    public void OnConfirmButtonPressed()
    {
        GameManager.Instance.ConfirmMoveTroop();
    }

    public void Activate(ETargetSelectionType type)
    {
        gameObject.transform.localScale = Vector3.zero;

        if (type == ETargetSelectionType.EmptyLand)
        {
            targetMarker.gameObject.SetActive(false);
            actionControl.gameObject.SetActive(true);
        }
        else if(type == ETargetSelectionType.EnemyLand)
        {            
            targetMarker.gameObject.SetActive(true);
            actionControl.gameObject.SetActive(false);            
        }

        LeanTween.scale(gameObject, Vector3.one, 0.5f)
                 .setEaseInOutElastic();
    }

    public void RearrangeContentTop()
    {
        confirmButton.transform.SetSiblingIndex(0);
        actionContainer.transform.SetSiblingIndex(1);
    }

    public void RearrangeContentBottom()
    {
        actionContainer.transform.SetSiblingIndex(0);
        confirmButton.transform.SetSiblingIndex(1);
    }

    public void EnableAddButton()
    {
        if(addButton != null)
        {
            addButton.interactable = true;
        }
    }

    public void DisableAddButton()
    {
        if(addButton != null)
        {
            addButton.interactable = false;
        }
    }

    public void EnableSubtractButton()
    {
        if (subtractButton != null)
        {
            subtractButton.interactable = true;
        }
    }

    public void DisableSubtractButton()
    {
        if (subtractButton != null)
        {
            subtractButton.interactable = false;
        }
    }

    public void EnableConfirmButton()
    {
        if (confirmButton != null)
        {
            confirmMoveButton.interactable = true;
        }
    }

    public void DisableConfirmButton()
    {
        if (confirmMoveButton != null)
        {
            confirmMoveButton.interactable = false;
        }
    }

    public void ResetFunctionalities()
    {
        DisableConfirmButton();
        DisableSubtractButton();
    }
}