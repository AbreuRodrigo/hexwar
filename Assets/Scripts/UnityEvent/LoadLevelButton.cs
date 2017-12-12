using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelButton : MonoBehaviour {
    public void LoadLevel(int level)
    {
        SceneLoader.LoadScene(level);
    }
}
