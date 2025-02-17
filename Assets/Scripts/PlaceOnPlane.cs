using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using System.Linq;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(InitialData))]
public class PlaceOnPlane : MonoBehaviour
{
    public static GameObject spawnedObject;
    float MaxScaleNumber;
    GameObject ArCamera;
    public static bool isObjectPlaced = false;
    static Vector3 initialScale;
    Quaternion previousRotation;
    Quaternion InitialRotation;
    Vector3 previousPosition;
    Vector3 previousScale;
    float time = 0;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;
    bool isPositioning = false;
    bool gotMultipleTouchs = false;
    public Vector3 startMarker;
    public Vector3 endMarker;
    public float speed = 100f;
    private float startTime;
    private float journeyLength;
    Quaternion toRotation;
    Quaternion FromRotation;
    float speedR = 2.5f;
    bool rotate = false;
    bool wentToPosition = false;
    Vector2 initialPosition = new Vector2(0, 0);
    Vector3 ObjectScreenPosition = new Vector2(0, 0);
    Vector2 DistanceDifference = new Vector2(0, 0);
    public static GameObject percentageIndicator;
    ARPlaneManager aRPlaneManager;
    private List<SpawningObj> spawningObj = new List<SpawningObj>();
public GameObject spawnPlayerBtn;

    void Start()
    {
        InitialData._singleObjectPlacement = true;
        aRPlaneManager = FindObjectOfType<ARPlaneManager>();
        ArCamera = GameObject.FindWithTag("MainCamera");
        m_RaycastManager = GetComponent<ARRaycastManager>();
        GameObject.FindWithTag("ScanSurfaceAnim").SetActive(true);
        isObjectPlaced = false;
        spawnedObject = Instantiate(InitialData.SpawningObject);

        MeshRenderer[] allmeshRenders = spawnedObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mRender in allmeshRenders)
        {
            SpawningObj spj = new SpawningObj();
            spj.meshRenderer = mRender;

            spawningObj.Add(spj);
        }

        aRPlaneManager.requestedDetectionMode = spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode;
        spawnedObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.localScale = spawnedObject.GetComponent<Collider>().bounds.size * 0.0015f;
        spawnedObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.position = new Vector3(0, spawnedObject.GetComponent<Collider>().bounds.size.y * 1.2f, 0);
        initialScale = spawnedObject.transform.localScale;
        spawnedObject.GetComponent<SpawningObjectDetails>().initialScale = initialScale;
        keepObjectWithInClippingValue(spawnedObject);
        spawnedObject.transform.parent = ArCamera.transform.transform;
        InitialRotation = spawnedObject.transform.rotation;
        spawnedObject.transform.position = ArCamera.transform.position + new Vector3(0, 0, 0.3f);
        spawnedObject.SetActive(false);
        spawnPlayerBtn.SetActive(false);
        delayToShowSpawnedObject();
    }

    private void GetWallPlacement(ARRaycastHit _planeHit, out Quaternion orientation, out Quaternion zUp)
    {
        TrackableId planeHit_ID = _planeHit.trackableId;
        ARPlane planeHit = aRPlaneManager.GetPlane(planeHit_ID);
        Vector3 planeNormal = planeHit.normal;
        orientation = Quaternion.FromToRotation(Vector3.up, planeNormal);
        Vector3 forward = _planeHit.pose.position - (_planeHit.pose.position + Vector3.down);
        zUp = Quaternion.LookRotation(forward, planeNormal);
    }
    void Update()
    {
        if (!isObjectPlaced)
        {
            delayToShowSpawnedObject();
            Vector3 rayEmitPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            if (m_RaycastManager.Raycast(rayEmitPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;
                FromRotation = spawnedObject.transform.rotation;
                if (aRPlaneManager.requestedDetectionMode == PlaneDetectionMode.Vertical)
                {
                    Quaternion orientation = Quaternion.identity;
                    Quaternion zUp = Quaternion.identity;
                    GetWallPlacement(s_Hits[0], out orientation, out zUp);
                    spawnedObject.transform.rotation = zUp;
                    toRotation = Quaternion.Euler(spawnedObject.transform.eulerAngles.x, spawnedObject.transform.rotation.eulerAngles.y, 0);
                }
                else
                {
                    toRotation = Quaternion.Euler(0, spawnedObject.transform.rotation.eulerAngles.y, 0);
                }

                spawnedObject.transform.parent = null;
                spawnedObject.transform.localScale = initialScale;
                spawnedObject.GetComponent<SpawningObjectDetails>().initialScale = spawnedObject.transform.localScale;
                startTime = Time.time;
                startMarker = spawnedObject.transform.position;
                endMarker = hitPose.position;
                wentToPosition = true;
                journeyLength = Vector3.Distance(startMarker, endMarker);
                rotate = true;
                previousRotation = hitPose.rotation;
                previousPosition = hitPose.position;
                previousScale = spawnedObject.transform.localScale;
                isObjectPlaced = true;

                GameObject.FindWithTag("ScanSurfaceAnim").SetActive(false);
                MeshRenderer[] allmeshRenders = spawnedObject.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mRender in allmeshRenders)
                {
                    Material[] mats = mRender.materials;
                    SpawningObj temp = spawningObj.Where(obj => obj.meshRenderer.name == mRender.name).SingleOrDefault();




                    spawningObj.Remove(temp);
                }
            }
        }
        else
        {
            if (TouchIndicatorHandler.isTouchedTheObject && (Input.touchCount < 2) && !gotMultipleTouchs)
            {
                if (!TryGetTouchPosition(out Vector2 touchPosition))
                    return;

                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon) && !IsPointerOverUIObject())
                {
                    if (isPositioning)
                    {
                        var hitPose = s_Hits[0].pose;
                        if (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().enbleDragFeature)
                            TouchIndicatorHandler.hitObject.transform.position = hitPose.position;
                        previousPosition = hitPose.position;

                    }
                }
            }
        }
        if (TouchIndicatorHandler.isTouchedTheObject)
        {
            TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.rotation = Quaternion.Euler(ArCamera.transform.rotation.eulerAngles.x, ArCamera.transform.rotation.eulerAngles.y, 0);
        }


        MultipleTouchHandler();
        freezePositionWhenRotate();
        SendObjectToDetectedPosition();
    }



    void MultipleTouchHandler()
    {
        if (Input.touchCount == 0)
        {
            gotMultipleTouchs = false;
            DistanceDifference = new Vector2(0, 0);
        }
        else if (Input.touchCount > 1)
        {
            gotMultipleTouchs = true;
            DistanceDifference = new Vector2(0, 0);
        }
    }


    bool TryGetTouchPosition(out Vector2 touchPosition)
    {

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                initialPosition = Input.GetTouch(0).position;
                ObjectScreenPosition = ArCamera.GetComponent<Camera>().WorldToScreenPoint(spawnedObject.transform.position);
                DistanceDifference = new Vector2(ObjectScreenPosition.x, ObjectScreenPosition.y) - initialPosition;
                touchPosition = Input.GetTouch(0).position + DistanceDifference;
                return true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (DistanceDifference != new Vector2(0, 0))
                {
                    isPositioning = true;
                    touchPosition = Input.GetTouch(0).position + DistanceDifference;
                    return true;
                }
                else
                {
                    initialPosition = Input.GetTouch(0).position;
                    ObjectScreenPosition = ArCamera.GetComponent<Camera>().WorldToScreenPoint(spawnedObject.transform.position);
                    DistanceDifference = new Vector2(ObjectScreenPosition.x, ObjectScreenPosition.y) - initialPosition;
                    touchPosition = Input.GetTouch(0).position + DistanceDifference;
                    return true;
                }

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                TouchIndicatorHandler.isTouchedTheObject = false;
                isPositioning = false;
                touchPosition = default;
                return false;
            }
            else
            {
                touchPosition = default;
                return false;
            }
        }
        else
        {
            TouchIndicatorHandler.isTouchedTheObject = false;
            isPositioning = false;
        }
        touchPosition = default;
        return false;
    }


    void SendObjectToDetectedPosition()
    {
        if (rotate || wentToPosition)
        {
            spawnedObject.transform.rotation = Quaternion.Lerp(FromRotation, toRotation, (Time.time - startTime) * speedR);
            if (spawnedObject.transform.rotation == toRotation)
            {
                rotate = false;
            }
            speed = Vector3.Distance(startMarker, endMarker);
            float distCovered = (Time.time - startTime) * speed * 100 * Time.deltaTime;
            float fractionOfJourney = distCovered / journeyLength;
            spawnedObject.transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
            if (spawnedObject.transform.position == endMarker)
            {
                wentToPosition = false;
                rotate = false;
                spawnedObject.transform.rotation = toRotation;
            }
        }

    }


    public static void hideScalePercentageIndicator()
    {
        TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(false);
    }


    public static void ShowScalePercentageIndicator(string Percentage)
    {
        TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(true);
        TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Percentage + "%";
    }


    public static void hideTouchIndicator()
    {
        if (isObjectPlaced)
        {
            spawnedObject.GetComponent<SpawningObjectDetails>().touchIndicator.SetActive(false);
        }

    }

    public static void showTouchIndicator()
    {
        if (isObjectPlaced && !IsPointerOverUIObject() && spawnedObject.GetComponent<SpawningObjectDetails>().enableTouchIndicator)
        {
            spawnedObject.GetComponent<SpawningObjectDetails>().touchIndicator.SetActive(true);
        }

    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    void delayToShowSpawnedObject()
    {
        if (time > 1.2f)
        {
            if (spawnedObject != null)
            {
                if (spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode == PlaneDetectionMode.Vertical)
                {
                    spawnedObject.transform.eulerAngles = new Vector3(-90, spawnedObject.transform.eulerAngles.y, spawnedObject.transform.eulerAngles.z);
                }
                spawnedObject.SetActive(true);
                spawnPlayerBtn.SetActive(true);
            }
        }
        else
        {
            time += Time.deltaTime;
        }
    }


    void freezePositionWhenRotate()
    {
        if (isObjectPlaced && (Input.touchCount > 1))
        {
            if (previousRotation != spawnedObject.transform.rotation)
            {
                spawnedObject.transform.position = previousPosition;
                previousRotation = spawnedObject.transform.rotation;
            }
            else if (previousRotation == spawnedObject.transform.rotation)
            {
                previousPosition = spawnedObject.transform.position;
            }
        }
    }



    public static void resetToInitialScale()
    {
        spawnedObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(false);
        spawnedObject.transform.localScale = spawnedObject.GetComponent<SpawningObjectDetails>().initialScale;
    }


    void keepObjectWithInClippingValue(GameObject Obj)
    {
        Collider collider = Obj.GetComponent<Collider>();

        MaxScaleNumber = Mathf.Max(collider.bounds.size.x, collider.bounds.size.y, collider.bounds.size.z);
        if (MaxScaleNumber > 1)
        {
            Obj.transform.localScale = (Obj.transform.localScale / MaxScaleNumber) * 0.1f;
        }
        else
        {
            Obj.transform.localScale = (1 / MaxScaleNumber * Obj.transform.localScale) * 0.1f;
        }
    }
}


[System.Serializable]
public class SpawningObj
{
    public MeshRenderer meshRenderer;
}