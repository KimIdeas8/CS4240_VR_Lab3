using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToPlace;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        //whenever the screen is tapped,
        //PlaceObject will be called,
        //and an object will be spawned
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    /// <summary>
    /// a raycast towards the center of the screen, looks for planes, and stores the results, if any, in the hits List
    /// if there are any hits, placementPoseIsValid is set to true. placementPose is then set to that Pose
    /// if not it is set to false.
    /// </summary>
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    /// <summary>
    /// need some code to actually move the placement indicator
    /// </summary>
    private void UpdatePlacementIndicator()
    {
        // If placementPoseIsValid is true, if sets the placement indicator to active
        // and sets the position and rotation according to the placement pose
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        // If not, placement indicator is set to inactive, which means that it will disappear.
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    //spawns the object at the placement pose position and rotation
    private void PlaceObject()
    {
        Instantiate(objectToPlace, PlacementPose.position, PlacementPose.rotation);
    }

}
