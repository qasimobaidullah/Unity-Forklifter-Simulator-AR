using System;
using UnityEngine;


public class ScaleRotateHandler : MonoBehaviour
{

    int scaleFactor;
    Vector3 MinimumValue;
    Vector3 MaximumValue;
    Vector3 LimitValue;
    float InitialDistance;
    float currentDistance;
    float previousDistance;
    Vector3 InitialScale;
    Vector3 InitialScaleOfGameObject;
    bool isScalling = false;
    bool calledForScalling = false;
    bool withInScaleLimit = true;
    int RotateFactor;
    float initalRotation;
    float currentRotation = 0;
    float previousRotation = 0;
    bool isRotating = false;
    bool calledForRotating = false;
    bool isRotationEnabled = true;
    bool isScalingEnabled = true;

    void Update()
    {

        if (TouchIndicatorHandler.hitObject != null)
        {


            InitialScaleOfGameObject = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale;
            scaleFactor = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scaleFactor;
            MinimumValue = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale * 0.4f;
            MaximumValue = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale * 10;
            LimitValue = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale * 12;
            RotateFactor = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().rotateFactor;
            isRotationEnabled = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().enableRotateFeature;
            isScalingEnabled = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().enableScaleFeature;
            limitScale(MinimumValue, MaximumValue);

            if (Input.touchCount == 0)
            {
                isScalling = false;
                isRotating = false;
            }
            if (Input.touchCount > 1)
            {
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    InitialDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    initalRotation = Angle(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    currentDistance = InitialDistance;
                    currentRotation = initalRotation;
                    previousDistance = InitialDistance;
                    InitialScale = TouchIndicatorHandler.hitObject.transform.localScale;
                    previousRotation = initalRotation;
                    isRotating = false;
                    isScalling = false;
                }
                else if (Input.GetTouch(1).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (InitialDistance == 0 || initalRotation == 0)
                    {
                        InitialScale = TouchIndicatorHandler.hitObject.transform.localScale;
                        InitialDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        initalRotation = Angle(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        currentDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        currentRotation = Angle(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    }
                    else
                    {
                        currentDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        currentRotation = Angle(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    }
                    if (isRotationEnabled)
                    {
                        CheckRotation();
                    }
                    if (isScalingEnabled)
                    {
                        CheckScaling();
                    }
                    if (isScalling)
                    {
                        changeScale(getScaleFactor(currentDistance));
                        if (InitialData._singleObjectPlacement)
                        {
                            PlaceOnPlane.ShowScalePercentageIndicator("" + (int)((TouchIndicatorHandler.hitObject.transform.localScale.magnitude / TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.magnitude) * 100));
                        }


                    }
                    else
                    {
                        if (InitialData._singleObjectPlacement)
                        {
                            PlaceOnPlane.hideScalePercentageIndicator();
                        }

                    }
                    if (isRotating)
                    {
                        changeRotation(getRotateFactor(currentRotation));
                    }
                }
                else if (Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isScalling = false;
                    isRotating = false;
                    calledForRotating = false;
                    calledForScalling = false;
                    InitialDistance = 0;
                    initalRotation = 0;
                    currentDistance = 0;
                    currentRotation = 0;
                }
            }
        }
        else
        {

            isScalling = false;
            isRotating = false;
            calledForRotating = false;
            calledForScalling = false;
            currentDistance = 0;
            currentRotation = 0;
            InitialDistance = 0;
            initalRotation = 0;
            if (withInScaleLimit)
            {
                if (InitialData._singleObjectPlacement && TouchIndicatorHandler.hitObject != null)
                {
                    PlaceOnPlane.hideScalePercentageIndicator();
                }

            }
        }
    }

    void CheckScaling()
    {
        if ((Math.Abs(currentDistance - InitialDistance) < 40) && !calledForScalling)
        {
            previousDistance = currentDistance;
            isScalling = false;

        }
        else if (!isRotating)
        {
            isScalling = true;
            calledForScalling = true;
            calledForRotating = false;
        }
    }

    void CheckRotation()
    {
        if (Math.Abs(currentRotation - initalRotation) < 0.065f && !calledForRotating)
        {
            isRotating = false;
            previousRotation = currentRotation;
        }
        else if (!isScalling)
        {
            isRotating = true;
            calledForRotating = true;
            calledForScalling = false;
        }
    }
    float getScaleFactor(float current)
    {
        float value;
        if (previousDistance == current)
        {
            value = 0;
        }
        else
        {
            value = (current - previousDistance) / scaleFactor;
            previousDistance = current;
        }
        return value;
    }

    void changeScale(float factor)
    {
        if (!isRotating && withInScaleLimit)
        {
            if (factor > 0)
            {
                TouchIndicatorHandler.hitObject.transform.localScale = TouchIndicatorHandler.hitObject.transform.localScale + InitialScale * (factor);
            }
            else
            {
                TouchIndicatorHandler.hitObject.transform.localScale = TouchIndicatorHandler.hitObject.transform.localScale + InitialScale * (factor) * 0.7f;
            }
        }

    }



    void limitScale(Vector3 min, Vector3 max)
    {

        if (TouchIndicatorHandler.hitObject != null)
        {
            if (TouchIndicatorHandler.hitObject.transform.localScale.x < (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.x * 0.4f) ||
            TouchIndicatorHandler.hitObject.transform.localScale.y < (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.y * 0.4f) ||
            TouchIndicatorHandler.hitObject.transform.localScale.z < (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.z * 0.4f))
            {
                TouchIndicatorHandler.hitObject.transform.localScale = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale * 0.4f;
            }
            else if ((TouchIndicatorHandler.hitObject.transform.localScale.x > (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.x * 12) ||
                TouchIndicatorHandler.hitObject.transform.localScale.y > (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.y * 12) ||
                TouchIndicatorHandler.hitObject.transform.localScale.z > (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.z * 12)))
            {
                TouchIndicatorHandler.hitObject.transform.localScale = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale * 12f;
                withInScaleLimit = false;
            }
            else if ((TouchIndicatorHandler.hitObject.transform.localScale.x > (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.x * 10) ||
                TouchIndicatorHandler.hitObject.transform.localScale.y > (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.y * 10) ||
                TouchIndicatorHandler.hitObject.transform.localScale.z > (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.z * 10)))
            {
                if (InitialData._singleObjectPlacement)
                {
                    TouchIndicatorHandler.hitObject.transform.localScale -= TouchIndicatorHandler.hitObject.transform.localScale * Time.deltaTime;
                    PlaceOnPlane.ShowScalePercentageIndicator("" + (int)((TouchIndicatorHandler.hitObject.transform.localScale.magnitude / TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale.magnitude) * 100));
                }

            }
            else
            {
                withInScaleLimit = true; ;
            }
        }

    }


    float getRotateFactor(float current)
    {
        float value;
        if (previousRotation == current)
        {
            value = 0;
        }
        else
        {
            value = (current - previousRotation) * RotateFactor;
            previousRotation = current;
        }
        return value;
    }


    void changeRotation(float factor)
    {
        if (withInScaleLimit)
        {
            {
                if (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().planeDetectionMode.ToString() == "Horizontal")
                {
                    TouchIndicatorHandler.hitObject.transform.rotation = Quaternion.Euler(0, TouchIndicatorHandler.hitObject.transform.rotation.eulerAngles.y + factor, 0);
                }
                else if (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().planeDetectionMode.ToString() == "Vertical")
                {
                    TouchIndicatorHandler.hitObject.transform.rotation = Quaternion.Euler(TouchIndicatorHandler.hitObject.transform.rotation.eulerAngles.x + factor, TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialPlacedRotation.eulerAngles.y, TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialPlacedRotation.eulerAngles.z);
                }
            }

        }

    }

    float Angle(Vector2 one, Vector2 two)
    {
        return (one.x - two.x) / Mathf.Sqrt((one.x - two.x) * (one.x - two.x) + (one.y - two.y) * (one.y - two.y));
    }



}