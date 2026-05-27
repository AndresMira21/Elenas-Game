using UnityEngine;

public class TriggerElena : MonoBehaviour
{
    public string nombreEvento;
    public int diaRequerido = 2;
    public float delayActivacion = 0f;
    private bool activado = false;
    private float tiempoInicio = 0f;

    void OnEnable()
    {
        tiempoInicio = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado
            && GameManager.Instance.currentDay == diaRequerido
            && Time.time - tiempoInicio >= delayActivacion)
        {
            activado = true;

            if (nombreEvento == "jugador_entra_cuarto" && !ElenaEventos.Instance.ElenaListaEnCama())
                return;
            if (nombreEvento == "jugador_toca_elena" && !ElenaEventos.Instance.ElenaListaEnCama())
                return;

            if (!ElenaEventos.Instance.gameObject.activeSelf)
                ElenaEventos.Instance.gameObject.SetActive(true);

            ElenaEventos.Instance.EjecutarEvento(nombreEvento);
        }
    }
}