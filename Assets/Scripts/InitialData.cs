using UnityEngine;

public class InitialData : MonoBehaviour
{

    public static GameObject SpawningObject;

    public static bool _singleObjectPlacement;

    public void ShowPrefabInARView(GameObject spwaningObject)
    {
        SpawningObject = spwaningObject;
        SceneHandler.GoToNextView();
    }

    public void GamePlayScene(GameObject spwaningObject)
    {
        SpawningObject = spwaningObject;
        SceneHandler.GamePlayScene();
    }
}
