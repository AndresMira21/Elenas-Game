using UnityEngine;

public class TriggerProximidadEspejo : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ElenaEventos.Instance.JugadorCercaEspejo();
        }
    }
}