using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public static void GoToNextView()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void BackFromCurrentScene()
    {
        if (Application.CanStreamedLevelBeLoaded("Menu"))
        {
            PlaceOnPlane.isObjectPlaced = false;
            Destroy(PlaceOnPlane.spawnedObject);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            LoaderUtility.Deinitialize();
        }
    }

    public static void GamePlayScene()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }
}
