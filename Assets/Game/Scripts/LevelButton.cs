using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelButton : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Level_1");
    }
}
