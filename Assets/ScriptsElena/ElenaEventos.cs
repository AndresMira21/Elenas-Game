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

    [Header("Puntos Nivel 4")]
    public Transform N4_EntradaBano;
    public Transform N4_EspejoLavamanos;
    public Transform N4_EspejoPasillo;

    private float tiempoDesdeAparicion = 0f;
    private bool elenaActiva = false;
    private bool jugadorSiguioDeInmediato = false;
    private bool elenaEnCama = false;
    private bool elenaCaminando = false;
    private bool elenaYaAparecionivel3 = false;
    private bool jugadorCercaEspejo = false;
    private bool esperandoJugador = false;

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
        jugadorCercaEspejo = false;
        esperandoJugador = false;
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

    public void ActivarNivel4()
    {
        ResetElena();
        gameObject.SetActive(true);
        StartCoroutine(Nivel4());
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

    public void JugadorCercaEspejo()
    {
        if (esperandoJugador)
            jugadorCercaEspejo = true;
    }

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
            yield return new WaitForSeconds(1f);
            transform.rotation = Quaternion.LookRotation(
                (N3_Pasillo.forward + Vector3.right * 0.05f).normalized);
            yield return new WaitForSeconds(1f);
            transform.rotation = Quaternion.LookRotation(N3_Pasillo.forward);
            yield return new WaitForSeconds(6f);
        }
        else
        {
            yield return new WaitForSeconds(8f);
        }

        yield return StartCoroutine(CaminarHasta(N3_Cuarto));
        gameObject.SetActive(false);
    }

    IEnumerator Nivel3_ApareceEnSofa()
    {
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

    // ===== NIVEL 4 =====
    IEnumerator Nivel4()
    {
        string estado = EstadoDominante();
        Debug.Log("Nivel 4 - Estado: " + estado);

        if (estado == "Negacion")
        {
            agent.Warp(N4_EspejoPasillo.position);
            yield return null;
            transform.rotation = Quaternion.LookRotation(N4_EspejoPasillo.forward);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
        else if (estado == "Duda")
        {
            agent.Warp(N4_EntradaBano.position);
            yield return new WaitUntil(() => agent.isOnNavMesh);
            transform.rotation = Quaternion.LookRotation(N4_EntradaBano.forward);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            yield return StartCoroutine(CaminarHasta(N4_EspejoLavamanos));
            transform.rotation = Quaternion.LookRotation(N4_EspejoLavamanos.forward);
            yield return new WaitForSeconds(7f);

            yield return StartCoroutine(CaminarHasta(N4_EspejoPasillo));
            transform.rotation = Quaternion.LookRotation(N4_EspejoPasillo.forward);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            DialogueManager.Instance.ShowThought("¿Por qué sigo viéndola aquí?", 3f);
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
        else if (estado == "Aceptacion")
        {
            agent.Warp(N4_EntradaBano.position);
            yield return new WaitUntil(() => agent.isOnNavMesh);
            transform.rotation = Quaternion.LookRotation(N4_EntradaBano.forward);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            yield return StartCoroutine(CaminarHasta(N4_EspejoLavamanos));
            transform.rotation = Quaternion.LookRotation(N4_EspejoLavamanos.forward);
            yield return new WaitForSeconds(7f);

            yield return StartCoroutine(CaminarHasta(N4_EspejoPasillo));
            transform.rotation = Quaternion.LookRotation(N4_EspejoPasillo.forward);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            DialogueManager.Instance.ShowThought("Hay algo raro con mi reflejo...", 3f);
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}