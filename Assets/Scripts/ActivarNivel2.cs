using UnityEngine;

public class ActivarNivel2 : MonoBehaviour
{
    public ElenaSecuenciaNivel2 elena;
    private bool activado = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;
            elena.IniciarSecuencia();
        }
    }
}