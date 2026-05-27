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

    [Header("Puntos Nivel 5")]
    public Transform N5_Sofa;
    public Transform N5_Ventana;
    public Transform N5_SalidaSala;

    private float tiempoDesdeAparicion = 0f;
    private bool elenaActiva = false;
    private bool jugadorSiguioDeInmediato = false;
    private bool elenaEnCama = false;
    private bool elenaCaminando = false;
    private bool elenaYaAparecionivel3 = false;
    private bool jugadorCercaEspejo = false;
    private bool esperandoJugador = false;
    private bool esperandoDecision = false;
    private bool conversacionNivel5Terminada = false;
    private bool decisionTVTomada = false;

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
        esperandoDecision = false;
        conversacionNivel5Terminada = false;
        decisionTVTomada = false;
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

    public void ActivarNivel5()
    {
        ResetElena();
        gameObject.SetActive(true);
        StartCoroutine(Nivel5());
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

    public void RespuestaDecisionA(int opcion)
    {
        if (opcion == 1)
            GameManager.Instance.AddDuda(1);
        else if (opcion == 2)
            GameManager.Instance.AddNegacion(1);
        esperandoDecision = false;
    }

    public void RespuestaDecisionTV(int opcion)
    {
        if (opcion == 1)
            GameManager.Instance.AddNegacion(2);
        else if (opcion == 2)
            GameManager.Instance.AddAceptacion(2);
        decisionTVTomada = true;
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
            case "elena_nivel5":
                StartCoroutine(Nivel5()); break;
            case "elena_final_n5":
                StartCoroutine(FinalNivel5()); break;
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
        Debug.Log(" Elena en cama: " + elenaEnCama);
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

    // ==== NIVEL 4 =====
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

    // ===== NIVEL 5 =====
    IEnumerator Nivel5()
    {
        string estado = EstadoDominante();
        Debug.Log("Nivel 5 - Estado: " + estado);

        if (estado == "Negacion")
        {
            agent.Warp(N5_Ventana.position);
            yield return null;
            transform.rotation = Quaternion.LookRotation(N5_Ventana.forward);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            transform.rotation = Quaternion.LookRotation(-N5_Ventana.forward);
            yield return StartCoroutine(ConversacionNivel5());

            yield return StartCoroutine(CaminarHasta(N5_SalidaSala));
            gameObject.SetActive(false);
        }
        else if (estado == "Duda")
        {
            agent.enabled = false;
            transform.position = N5_Sofa.position;
            transform.rotation = Quaternion.LookRotation(N5_Sofa.forward);
            animator.SetBool("isSitting", true);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            animator.SetBool("isSitting", false);
            agent.enabled = true;

            yield return StartCoroutine(ConversacionNivel5());

            yield return StartCoroutine(CaminarHasta(N5_SalidaSala));
            gameObject.SetActive(false);
        }
        else if (estado == "Aceptacion")
        {
            agent.enabled = false;
            transform.position = N5_Sofa.position;
            transform.rotation = Quaternion.LookRotation(N5_Sofa.forward);
            animator.SetBool("isSitting", true);

            esperandoJugador = true;
            yield return new WaitUntil(() => jugadorCercaEspejo);
            esperandoJugador = false;
            jugadorCercaEspejo = false;

            animator.SetBool("isSitting", false);
            agent.enabled = true;

            yield return StartCoroutine(ConversacionNivel5());

            yield return StartCoroutine(CaminarHasta(N5_SalidaSala));
            yield return new WaitForSeconds(1f);
            DialogueManager.Instance.ShowThought("[Elena]: Ya casi.", 3f);
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
    }

    IEnumerator ConversacionNivel5()
    {
        DialogueManager.Instance.ShowThought("[Elena]: Llevas mucho tiempo buscando algo.", 4f);
        yield return new WaitForSeconds(4f);

        DialogueManager.Instance.ShowThought("[Martín]: ¿Qué estoy buscando?", 3f);
        yield return new WaitForSeconds(3f);

        DialogueManager.Instance.ShowThought("[Elena]: Lo que estás buscando no es lo que crees que estás buscando.", 5f);
        yield return new WaitForSeconds(5f);

        transform.rotation = Quaternion.LookRotation(N5_Ventana.forward);
        yield return new WaitForSeconds(2f);
        transform.rotation = Quaternion.LookRotation(N5_Sofa.forward);

        DialogueManager.Instance.ShowThought("[Elena]: ¿Cuánto tiempo crees que llevas aquí?", 4f);
        yield return new WaitForSeconds(4f);

        esperandoDecision = true;
        yield return new WaitUntil(() => !esperandoDecision);

        DialogueManager.Instance.ShowThought("[Elena]: Yo sí sé cuánto tiempo llevas aquí.", 4f);
        yield return new WaitForSeconds(4f);

        DialogueManager.Instance.ShowThought("[Martín]: ¿Cuánto?", 2f);
        yield return new WaitForSeconds(2f);

        DialogueManager.Instance.ShowThought("[Elena]: Cuando estés listo para saberlo, ya lo vas a saber.", 5f);
        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(CaminarHasta(N5_Ventana));
        transform.rotation = Quaternion.LookRotation(N5_Ventana.forward);

        DialogueManager.Instance.ShowThought("[Elena]: El problema no es que no lo sepas. El problema es que no quieres saber.", 6f);
        yield return new WaitForSeconds(6f);

        yield return new WaitForSeconds(6f);

        conversacionNivel5Terminada = true;
    }

    IEnumerator FinalNivel5()
    {
        if (!conversacionNivel5Terminada || !decisionTVTomada)
        {
            yield return null;
            yield break;
        }

        string estado = EstadoDominante();
        Debug.Log("Final Nivel 5 - Estado: " + estado);

        gameObject.SetActive(true);
        agent.Warp(N5_Sofa.position);
        yield return null;
        transform.rotation = Quaternion.LookRotation(N5_Sofa.forward);

        if (estado == "Negacion")
        {
            DialogueManager.Instance.ShowThought("[Elena]: Sigues sin querer ver. Pero ya no importa cuánto tiempo pase... seguirás aquí.", 6f);
            yield return new WaitForSeconds(6f);
        }
        else if (estado == "Duda")
        {
            DialogueManager.Instance.ShowThought("[Elena]: Ya casi lo entiendes. La respuesta siempre estuvo frente a ti. Solo que nunca quisiste mirarla.", 6f);
            yield return new WaitForSeconds(6f);
        }
        else if (estado == "Aceptacion")
        {
            DialogueManager.Instance.ShowThought("[Elena]: Ya lo sabes. Solo tienes que aceptarlo. Yo ya lo acepté... hace mucho tiempo.", 6f);
            yield return new WaitForSeconds(6f);
        }

        gameObject.SetActive(false);
    }
}