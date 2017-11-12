using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour {

    private static TrailManager instance;
    public static TrailManager Instance
    {
        get { return instance; }
    }

    public bool isEnabled = true;
    public GameObject trail;
    public int poolSize;

    public List<Trail> available = null;
    public List<Trail> inUse = null;

    private Vector3 trailSource;

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        available = new List<Trail>(poolSize);
        inUse = new List<Trail>(poolSize);

        for(int i = 0; i < poolSize; i++)
        {
            available.Add(Instantiate(trail, trail.transform.position, trail.transform.rotation).GetComponent<Trail>());
        }

        StartCoroutine(GenerateTrail());
    }

    public void Enable(Vector3 trailSource)
    {
        this.trailSource = trailSource;
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public void FreeObject(Trail t)
    {
        if(inUse != null && t != null)
        {
            t.gameObject.SetActive(false);
            inUse.Remove(t);
            available.Add(t);            
        }
    }

    IEnumerator GenerateTrail()
    {
        Trail r = null;

        while (true)
        {
            if(isEnabled && available.Count > 0)
            {
                r = available[0];
                r.gameObject.SetActive(true);
                available.Remove(r);
                inUse.Add(r);

                r.Play(trailSource);
            }

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}