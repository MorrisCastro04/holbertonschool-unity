using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ChoosePlane : MonoBehaviour
{
    NavMeshSurface navMeshSurface;
    public Image BackGround;
    public TextMeshProUGUI text;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public Canvas startCanvas;
    public GameObject targetPrefab;
    public int numberOfTargets = 5;
    static ARPlane selectedPlane;
    bool gameStarted = false;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (planeManager.trackables.count > 0)
        {
            BackGround.color = new Color(0, 255, 0, 255);
            text.text = "tap on a plane to start.";
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !gameStarted)
        {
            Touch touch = Input.GetTouch(0);
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                ARPlane plane = planeManager.GetPlane(hits[0].trackableId);
                if (plane != null)
                {
                    SelectedPlane(plane);
                }
            }
        }
    }

    void SelectedPlane(ARPlane plane)
    {
        selectedPlane = plane;
        foreach (var p in planeManager.trackables)
        {
            if (p != plane)
            {
                p.gameObject.SetActive(false);
            }
        }

        navMeshSurface = selectedPlane.gameObject.AddComponent<NavMeshSurface>();
        navMeshSurface.collectObjects = CollectObjects.Children;
        navMeshSurface.BuildNavMesh();

        startCanvas.gameObject.SetActive(true);
    }

    public void StartGame()
    {

        for (int i = 0; i < numberOfTargets; i++)
        {
            Vector3 randomPosition = selectedPlane.transform.position + new Vector3(
                Random.Range(-3f, 3f) * selectedPlane.transform.localScale.x,
                0.1f,
                Random.Range(-3f, 3f) * selectedPlane.transform.localScale.z
            );
            Instantiate(targetPrefab, randomPosition, Quaternion.identity);
        }

        gameStarted = true;
        planeManager.enabled = false;
        startCanvas.gameObject.SetActive(false);
        BackGround.gameObject.SetActive(false);
    }
}
