using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class SpawningObjectDetails : MonoBehaviour
{
    [SerializeField]
    private PlaneDetectionMode _planeDetectionMode;

    [SerializeField]
    bool _enbleDragFeature = true;

    [SerializeField]
    bool _enableRotateFeature = true;

    [SerializeField]
    int _rotateFactor = 750;

    [SerializeField]
    bool _enableScaleFeature = true;


    [SerializeField]
    int _scaleFactor = 400;


    [SerializeField]
    bool _enableTouchIndicator = true;

    [SerializeField]
    private GameObject _touchIndicator;

    [SerializeField]
    private GameObject _scalePersentageIndicator;



    private Vector3 _initialScale = new Vector3(0, 0, 0);

    private Vector3 _minimumScaleValue = new Vector3(0, 0, 0);

    private Vector3 _maximumScaleValue = new Vector3(0, 0, 0);

    private Vector3 _limitScaleValue = new Vector3(0, 0, 0);

    private Quaternion _initialRotation;

    private Quaternion _initialPlacedRotation;


    public bool enableTouchIndicator
    {
        get { return _enableTouchIndicator; }
    }


    public bool enbleDragFeature
    {
        get { return _enbleDragFeature; }
    }
    public bool enableRotateFeature
    {
        get { return _enableRotateFeature; }
    }
    public bool enableScaleFeature
    {
        get { return _enableScaleFeature; }
    }

    public int rotateFactor
    {
        get { return _rotateFactor; }
    }
    public int scaleFactor
    {
        get { return _scaleFactor; }
    }



    public Quaternion initialPlacedRotation
    {
        get { return _initialPlacedRotation; }
        set { _initialPlacedRotation = value; }
    }
    public Quaternion initialRotation
    {
        get { return _initialRotation; }
        set { _initialRotation = value; }
    }
    public Vector3 initialScale
    {
        get { return _initialScale; }
        set { _initialScale = value; }
    }

    public Vector3 minimumScaleValue
    {
        get { return _minimumScaleValue; }
        set { _minimumScaleValue = value; }
    }

    public Vector3 maximumScaleValue
    {
        get { return _maximumScaleValue; }
        set { _maximumScaleValue = value; }
    }

    public Vector3 limitScaleValue
    {
        get { return _limitScaleValue; }
        set { _limitScaleValue = value; }
    }


    public PlaneDetectionMode planeDetectionMode
    {
        get { return _planeDetectionMode; }
        set { _planeDetectionMode = value; }
    }


    public GameObject scalePersentageIndicator
    {
        get { return _scalePersentageIndicator; }
        set { _scalePersentageIndicator = value; }
    }
    public GameObject touchIndicator
    {
        get { return _touchIndicator; }
        set { _touchIndicator = value; }
    }

    void Start()
    {
        _initialPlacedRotation = gameObject.transform.rotation;
        _initialScale = gameObject.transform.localScale;
        _scalePersentageIndicator.SetActive(false);
        _touchIndicator.SetActive(false);
    }
}
