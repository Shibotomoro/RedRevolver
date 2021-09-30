using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour
{
    public void ExitGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
