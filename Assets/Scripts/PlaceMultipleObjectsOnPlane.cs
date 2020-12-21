using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceMultipleObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    Vector3 firstpoint;    //change type on Vector3
    Vector3 secondpoint;
    float xAngle = 0.0f;   //angle for axes x for rotation
    float xAngTemp = 0.0f; //temp variable for angle
    public List<GameObject> Popups;
    public List<GameObject> Parts;
    public static string log = "";

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        Parts = new List<GameObject>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (spawnedObject == null && m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;

                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    spawnedObject.transform.localScale = Vector3.one * 0.05f;
                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                    Parts.Add(FindObject(spawnedObject, "Flap_Sofa_L"));
                    Parts.Add(FindObject(spawnedObject, "Flap_Sofa_R"));
                }
            }
        }
    }


    void LateUpdate()
    {
        if (spawnedObject != null)
        {
            if (!UI.IsPointerOverUIObject())
            {
                xAngle = spawnedObject.transform.eulerAngles.y;

                //Check count touches
                if (Input.touchCount == 1)
                {
                    //Touch began, save position
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        firstpoint = Input.GetTouch(0).position;
                        xAngTemp = xAngle;
                    }
                    //Move finger
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        secondpoint = Input.GetTouch(0).position;
                        xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                        spawnedObject.transform.localRotation = Quaternion.AngleAxis(xAngle, Vector3.up);
                    }
                }

                //Pinch zoom
                if (Input.touchCount == 2 && !PopupActive())
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
                    float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                    float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

                    if (deltaMagDiff > 0)
                    {
                        if (spawnedObject.transform.localScale.x > 0.01f)
                            spawnedObject.transform.localScale = spawnedObject.transform.localScale / 1.05f;// * deltaMagDiff * zoomSpeed;
                    }
                    else
                    {
                        if (spawnedObject.transform.localScale.x < 1f)
                            spawnedObject.transform.localScale = spawnedObject.transform.localScale * 1.05f;
                    }
                }
            }
            if (SofaConfig.materialChanged)
            {
                foreach (GameObject part in Parts)
                {
                    if (part != null)
                    {
                        part.GetComponent<SkinnedMeshRenderer>().material = SofaConfig.material;
                    }
                }
                SofaConfig.materialChanged = false;
            }
        }
    }


    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }



    private bool PopupActive()
    {
        foreach (GameObject g in Popups)
        {
            if (g.active)
            {
                return true;
            }
        }
        return false;
    }


    public void RemoveAllAnchors()
    {
        //m_AnchorManager.RemoveAnchor(m_Anchor);
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }     
    }


    //void OnGUI()
    //{
    //    int w = Screen.width, h = Screen.height;

    //    GUIStyle style = new GUIStyle();

    //    Rect rect = new Rect(0, 0, w, h * 2 / 100);
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 2 / 100;
    //    style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
    //    string myFileName = "Screenshot" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".png";
    //    string myDefaultLocation = Application.persistentDataPath + "/" + myFileName;
    //    //string text = Shader.GetGlobalFloat("_GlobalLightEstimation").ToString();
    //    //string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    //    GUI.Label(rect, log, style);
    //}
}
