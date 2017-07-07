using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        OverPlayer op = GameObject.FindObjectOfType<OverPlayer>();
        op.enabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
