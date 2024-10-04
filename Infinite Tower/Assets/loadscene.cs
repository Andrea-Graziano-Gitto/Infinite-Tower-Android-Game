using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import to use SceneManager

public class LoadScene : MonoBehaviour
{
    // Public variable to set the scene name from the Unity Editor
    public string sceneToLoad;

    // Public method to load the scene using the sceneToLoad variable
    public void LoadSpecifiedScene()
    {
        // Check if the scene name is not empty
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // Load the scene with the specified name
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is empty! Please assign a valid scene name.");
        }
    }
}
