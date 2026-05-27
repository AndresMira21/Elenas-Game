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

    [Header("Puntos Nivel 3")]
    public Transform N3_Pasillo;
    public Transform N3_Sofa;
    public Transform N3_Cuarto;

    private float tiempoDesdeAparicion = 0f;
    private bool elenaActiva = false;
    private bool jugadorSiguioDeInmediato = false;
    private bool elenaEnCama = false;
    private bool elenaCaminando = false;
    private bool elenaYaAparecionivel3 = false;

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
        if (animator != null)
            animator.SetBool("isWalking", elenaCaminando);
    }

    void ResetElena()
    {
        StopAllCoroutines();
        elenaEnCama = false;
        elenaActiva = false;
        elenaCaminando = false;
        elenaYaAparecionivel3 = false;
        if (animator != null)
        {
            animator.SetBool("isSitting", false);
            animator.SetBool("isWalking", false);
        }
        if (agent != null)
            agent.enabled = true;
    }

    public void ActivarYAparece()
    {
        ResetElena();
        gameObject.SetActive(true);
        StartCoroutine(Evento_ApareceVentana());
    }

    public void PrepararNivel3()
    {
        StopAllCoroutines();
        ResetElena();
        gameObject.SetActive(false);
    }

    public void PrepararNivel4()
    {
        StopAllCoroutines();
        ResetElena();
        gameObject.SetActive(false);
    }

    public void PrepararNivel5()
    {
        StopAllCoroutines();
        ResetElena();
        gameObject.SetActive(false);
    }

    string EstadoDominante()
    {
        int neg = GameManager.Instance.negacion;
        int dud = GameManager.Instance.duda;
        int acep = GameManager.Instance.aceptacion;
        Debug.Log($"Estado — Neg:{neg} Dud:{dud} Acep:{acep}");
        if (neg > dud && neg > acep) return "Negacion";
        else if (dud > acep) return "Duda";
        else if (acep > dud) return "Aceptacion";
        else return "Duda";
    }

    public bool ElenaListaEnCama() { return elenaEnCama; }

    public void EjecutarEvento(string nombreEvento)
    {
        Debug.Log("EjecutarEvento: " + nombreEvento + " Día: " + GameManager.Instance.currentDay);
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
            case "elena_aparece_pasillo":
                StartCoroutine(Nivel3_ApareceEnPasillo()); break;
            case "elena_aparece_sofa":
                StartCoroutine(Nivel3_ApareceEnSofa()); break;
        }
    }

    IEnumerator CaminarHasta(Transform destino)
    {
        if (!agent.enabled) agent.enabled = true;
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

    // ===== NIVEL 2 =====
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

    IEnumerator RutinaAgua()
    {
        yield return StartCoroutine(CaminarHasta(N2_Fregadero));

        float duracionAgua = GameManager.Instance.negacion > GameManager.Instance.aceptacion
            && GameManager.Instance.negacion > GameManager.Instance.duda ? 10f : 8f;
        yield return new WaitForSeconds(duracionAgua);

        agent.updateRotation = false;
        transform.rotation = Quaternion.LookRotation(-N2_Fregadero.forward);
        yield return new WaitForSeconds(3f);

        DialogueManager.Instance.ShowThought("¿Adónde vas?", 5f);
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(CaminarHasta(N2_Cama));

        agent.enabled = false;
        transform.position = N2_Cama.position;
        transform.rotation = Quaternion.LookRotation(N2_Cama.forward);
        animator.SetBool("isSitting", true);
        elenaEnCama = true;
    }

    IEnumerator Evento_JugadorEntraCuarto()
    {
        DialogueManager.Instance.ShowThought(
            "[Elena]: Ya es tarde para cambiar lo que pasó. Pero todavía no entiendo por qué sigue pasando.", 5f);
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

    // ===== NIVEL 3 =====
    IEnumerator Nivel3_ApareceEnPasillo()
    {
        string estado = EstadoDominante();
        Debug.Log("Nivel 3 Pasillo - Estado: " + estado);

        if (estado == "Negacion")
        {
            yield return null;
            yield break;
        }

        elenaYaAparecionivel3 = true;
        gameObject.SetActive(true);
        agent.enabled = true;
        animator.SetBool("isSitting", false);
        animator.SetBool("isWalking", false);

        agent.Warp(N3_Pasillo.position);
        yield return new WaitUntil(() => agent.isOnNavMesh);
        transform.rotation = Quaternion.LookRotation(N3_Pasillo.forward);
        elenaActiva = true;

        if (estado == "Aceptacion")
        {
            // Ojos se acercan 1cm por un segundo
            yield return new WaitForSeconds(1f);
            transform.rotation = Quaternion.LookRotation(
                (N3_Pasillo.forward + Vector3.right * 0.05f).normalized);
            yield return new WaitForSeconds(1f);
            transform.rotation = Quaternion.LookRotation(N3_Pasillo.forward);
            yield return new WaitForSeconds(6f);
        }
        else
        {
            // Duda — espera normal
            yield return new WaitForSeconds(8f);
        }

        // Da media vuelta, camina al cuarto y desaparece
        yield return StartCoroutine(CaminarHasta(N3_Cuarto));
        gameObject.SetActive(false); // ← desaparece siempre al final
    }

    IEnumerator Nivel3_ApareceEnSofa()
    {
        // Si ya apareció en pasillo no aparece en sofá
        if (elenaYaAparecionivel3)
        {
            yield return null;
            yield break;
        }

        string estado = EstadoDominante();
        Debug.Log("Nivel 3 Sofá - Estado: " + estado);

        if (estado != "Negacion")
        {
            yield return null;
            yield break;
        }

        gameObject.SetActive(true);
        agent.enabled = true;
        animator.SetBool("isSitting", false);
        animator.SetBool("isWalking", false);

        agent.Warp(N3_Sofa.position);
        yield return new WaitUntil(() => agent.isOnNavMesh);
        agent.enabled = false;
        transform.position = N3_Sofa.position;
        transform.rotation = Quaternion.LookRotation(N3_Sofa.forward);
        animator.SetBool("isSitting", true);
    }

    // ===== NIVEL 4 Y 5 — placeholder =====
    IEnumerator Nivel4_Iniciar()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    IEnumerator Nivel5_Iniciar()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}