using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public bool isOpen = false;

    public float openAngle = 90f;
    public float speed = 2f;

    Quaternion closedRotation;
    Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;

        openRotation =
            Quaternion.Euler(
                transform.eulerAngles +
                new Vector3(0, openAngle, 0)
            );
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation =
                Quaternion.Lerp(
                    transform.rotation,
                    openRotation,
                    Time.deltaTime * speed
                );
        }
        else
        {
            transform.rotation =
                Quaternion.Lerp(
                    transform.rotation,
                    closedRotation,
                    Time.deltaTime * speed
                );
        }
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }
}