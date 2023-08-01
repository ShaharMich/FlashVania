using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
