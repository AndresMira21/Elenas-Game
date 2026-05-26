using UnityEngine;

public class TriggerElena : MonoBehaviour
{
    public string nombreEvento;
    public int diaRequerido = 2;
    private bool activado = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado
            && GameManager.Instance.currentDay == diaRequerido)
        {
            if (nombreEvento == "jugador_entra_cuarto" && !ElenaEventos.Instance.ElenaListaEnCama())
                return;

            if (nombreEvento == "jugador_toca_elena" && !ElenaEventos.Instance.ElenaListaEnCama())
                return;

            activado = true;
            if (!ElenaEventos.Instance.gameObject.activeSelf)
                ElenaEventos.Instance.gameObject.SetActive(true);
            ElenaEventos.Instance.EjecutarEvento(nombreEvento);
        }
    }
}