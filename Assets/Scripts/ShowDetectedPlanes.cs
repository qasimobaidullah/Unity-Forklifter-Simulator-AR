using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ShowDetectedPlanes : MonoBehaviour
{
    ARPlaneManager m_plane;
    public bool planeEnable = false;
    GameObject shadowPlane;


    void Awake()
    {
        m_plane = FindObjectOfType<ARPlaneManager>();
    }

    void Update()
    {
        if (planeEnable)
        {
            SetAllPlanesActive(true);
            if (shadowPlane != null)
            {
                shadowPlane.SetActive(false);
            }
        }
        else
        {
            SetAllPlanesActive(false);

        }
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_plane.trackables)
            plane.gameObject.SetActive(value);
    }

    public void ShowHidePlanes()
    {
        planeEnable = !planeEnable;
    }
}
