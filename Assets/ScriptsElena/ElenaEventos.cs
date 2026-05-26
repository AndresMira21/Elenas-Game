using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ElenaEventos : MonoBehaviour
{
    public static ElenaEventos Instance;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Puntos Nivel 2")]
    public Transform N2_Ventana;
    public Transform N2_Fregadero;
    public Transform N2_Cama;

    private float tiempoDesdeAparicion = 0f;
    private bool elenaActiva = false;
    private bool jugadorSiguioDeInmediato = false;
    private bool elenaEnCama = false;
    private bool elenaCaminando = false;

    void Awake() { Instance = this; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (elenaActiva)
            tiempoDesdeAparicion += Time.deltaTime;

        animator.SetBool("isWalking", elenaCaminando);
    }

    public void ActivarYAparece()
    {
        gameObject.SetActive(true);
        StartCoroutine(Evento_ApareceVentana());
    }

    public bool ElenaListaEnCama() { return elenaEnCama; }

    public void EjecutarEvento(string nombreEvento)
    {
        switch (nombreEvento)
        {
            case "aparece_ventana":
                StartCoroutine(Evento_ApareceVentana()); break;
            case "jugador_se_acerca":
                StartCoroutine(Evento_JugadorSeAcerca()); break;
            case "jugador_entra_cuarto":
                StartCoroutine(Evento_JugadorEntraCuarto()); break;
            case "jugador_toca_elena":
                Evento_JugadorTocaElena(); break;
        }
    }

    IEnumerator Evento_ApareceVentana()
    {
        agent.Warp(N2_Ventana.position);
        yield return new WaitUntil(() => agent.isOnNavMesh);
        transform.rotation = Quaternion.LookRotation(N2_Ventana.forward);
        elenaCaminando = false;
        elenaActiva = true;
        tiempoDesdeAparicion = 0f;
    }

    IEnumerator Evento_JugadorSeAcerca()
    {
        DialogueManager.Instance.ShowThought("Elena...", 2f);
        yield return new WaitForSeconds(2f);
        jugadorSiguioDeInmediato = tiempoDesdeAparicion < 30f;
        yield return new WaitForSeconds(7f);
        StartCoroutine(RutinaAgua());
    }

    IEnumerator CaminarHasta(Transform destino)
    {
        agent.updateRotation = true;
        agent.speed = 1.0f;
        elenaCaminando = true;
        agent.SetDestination(destino.position);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() =>
            !agent.pathPending && agent.remainingDistance < 0.5f);
        elenaCaminando = false;
        agent.ResetPath();
    }

    IEnumerator RutinaAgua()
    {
        // 1. Camina al fregadero
        yield return StartCoroutine(CaminarHasta(N2_Fregadero));

        // 2. Parada larga en fregadero
        float duracionAgua = GameManager.Instance.negacion > GameManager.Instance.aceptacion
            && GameManager.Instance.negacion > GameManager.Instance.duda ? 10f : 8f;
        yield return new WaitForSeconds(duracionAgua);

        // 3. Se voltea
        agent.updateRotation = false;
        transform.rotation = Quaternion.LookRotation(-N2_Fregadero.forward);
        yield return new WaitForSeconds(3f);

        // Martín dice "¿Adónde vas?" cuando Elena pasa junto a él
        DialogueManager.Instance.ShowThought("¿Adónde vas?", 5f);
        yield return new WaitForSeconds(2f);

        // 4. Camina a la cama
        yield return StartCoroutine(CaminarHasta(N2_Cama));

        // 4. Camina a la cama
        yield return StartCoroutine(CaminarHasta(N2_Cama));

        // 5. Se sienta — desactiva NavMesh para bajar a posición exacta
        agent.enabled = false;
        transform.position = N2_Cama.position;
        transform.rotation = Quaternion.LookRotation(N2_Cama.forward);
        animator.SetBool("isSitting", true);
        elenaEnCama = true;
    }

    IEnumerator Evento_JugadorEntraCuarto()
    {
        DialogueManager.Instance.ShowThought(
            "[Elena]: Ya es tarde para cambiar lo que pasó. Pero todavía no entiendo por qué sigue pasando...", 5f);
        yield return new WaitForSeconds(5f);

        if (jugadorSiguioDeInmediato)
            GameManager.Instance.AddNegacion(1);
        else
            GameManager.Instance.AddDuda(1);
    }

    void Evento_JugadorTocaElena()
    {
        DialogueManager.Instance.ShowThought("No quiero interrumpirla.", 4f);
    }
}