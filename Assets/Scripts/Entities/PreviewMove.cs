using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewMove : MonoBehaviour
{
    public Text counter;

    private int amount;
    public int Amount { get { return amount; } }

    public void Reset(Vector3 newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y, 0);
    }

    public void Add(int amount = 1)
    {
        this.amount += amount;
        UpdateHud();
    }

    public void Subtract(int amount = 1)
    {
        if (this.amount > 0)
        {
            this.amount -= amount;
            UpdateHud();
        }
    }

    private void UpdateHud()
    {
        if (counter != null)
        {
            counter.text = this.amount.ToString();

            LeanTween.scale(counter.gameObject, new Vector3(1.2f, 1.2f, 1), 0.25f)
                     .setEasePunch();
        }
    }
}