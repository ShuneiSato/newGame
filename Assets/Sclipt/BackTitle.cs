using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour
{
    public void GoTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
        GameManager.instance.RetryGame();
    }
}
