using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARPlaneManager))]
public class PlaneDetectionController : MonoBehaviour
{
    [Tooltip("The UI Text element used to display plane detection messages.")]
    [SerializeField]
    Text m_TogglePlaneDetectionText;


    public Text togglePlaneDetectionText
    {
        get { return m_TogglePlaneDetectionText; }
        set { m_TogglePlaneDetectionText = value; }
    }


    public void TogglePlaneDetection()
    {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

        string planeDetectionMessage = "";
        if (m_ARPlaneManager.enabled)
        {
            planeDetectionMessage = "Disable Plane Detection and Hide Existing";
            SetAllPlanesActive(true);
        }
        else
        {
            planeDetectionMessage = "Enable Plane Detection and Show Existing";
            SetAllPlanesActive(false);
        }

        if (togglePlaneDetectionText != null)
            togglePlaneDetectionText.text = planeDetectionMessage;
    }


    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);
    }

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    ARPlaneManager m_ARPlaneManager;
}
