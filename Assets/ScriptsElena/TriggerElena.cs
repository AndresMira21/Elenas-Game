using UnityEngine;

public class TriggerElena : MonoBehaviour
{
    public string nombreEvento;
    public int diaRequerido = 2;
    public float delayActivacion = 0f;
    private bool activado = false;
    private float tiempoInicio = 0f;
    private int ultimoDia = -1;

    void OnEnable()
    {
        tiempoInicio = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.currentDay != ultimoDia)
        {
            activado = false;
            ultimoDia = GameManager.Instance.currentDay;
        }

        if (other.CompareTag("Player") && !activado
            && GameManager.Instance.currentDay == diaRequerido
            && Time.time - tiempoInicio >= delayActivacion)
        {
            if (nombreEvento == "jugador_entra_cuarto" && !ElenaEventos.Instance.ElenaListaEnCama())
            {
                Debug.Log(" Elena no está en cama todavía");
                return; 
            }
            if (nombreEvento == "jugador_toca_elena" && !ElenaEventos.Instance.ElenaListaEnCama())
            {
                return; 
            }

            activado = true;

            if (!ElenaEventos.Instance.gameObject.activeSelf)
                ElenaEventos.Instance.gameObject.SetActive(true);

            ElenaEventos.Instance.EjecutarEvento(nombreEvento);
        }
    }
}