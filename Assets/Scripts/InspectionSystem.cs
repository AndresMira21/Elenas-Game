using UnityEngine;

public class InspectionSystem : MonoBehaviour
{
    public static InspectionSystem Instance;

    public Camera cam;
    public UIManager ui;

    public float zoomFOV = 40f;

    float normalFOV;
    bool inspecting = false;
    bool justExited = false;

    void Awake()
    {
        Instance = this;
        normalFOV = cam.fieldOfView;
    }

    void Update()
    {
        if (inspecting && Input.GetKeyDown(KeyCode.E))
        {
            StopInspection();
        }
    }

    public void StartInspection(string message)
    {
        if (justExited) return;

        inspecting = true;

        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        cam.fieldOfView = zoomFOV;

        ui.Show(message);
    }

    public void StopInspection()
    {
        inspecting = false;

        PlayerMovement.canMove = true;
        PlayerMovement.canLook = true;

        cam.fieldOfView = normalFOV;

        ui.Hide();

        justExited = true;
        Invoke(nameof(ResetExit), 0.2f);
    }

    void ResetExit()
    {
        justExited = false;
    }
}