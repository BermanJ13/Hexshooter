using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
