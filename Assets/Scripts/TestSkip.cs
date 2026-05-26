using UnityEngine;

public class TestSkip : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.Instance.Sleep();
        }
    }
}