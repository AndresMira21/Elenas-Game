using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ElenaSecuenciaNivel2 : MonoBehaviour
{
    public static ElenaSecuenciaNivel2 Instance;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Puntos Nivel 2")]
    public Transform N2_Ventana;
    public Transform N2_Fregadero;
    public Transform N2_Pasillo;
    public Transform N2_EscalerasAbajo;
    public Transform N2_EscalerasArriba;
    public Transform N2_Cama;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void IniciarSecuencia()
    {
        gameObject.SetActive(true);
        StartCoroutine(SecuenciaNivel2());
    }

    void Update()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            bool caminando = agent.velocity.magnitude > 0.1f;
            animator.SetBool("isWalking", caminando);
        }
    }

    IEnumerator SecuenciaNivel2()
    {
        yield return new WaitForSeconds(0.5f);
        agent.Warp(N2_Ventana.position);
        yield return new WaitUntil(() => agent.isOnNavMesh);

        // 1. Parada en ventana
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(5f);

        // 2. Camina lento al fregadero
        agent.speed = 0.8f;
        agent.SetDestination(N2_Fregadero.position);
        yield return new WaitUntil(() =>
            agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f);

        // 3. Parada en fregadero — abre agua
        agent.speed = 0f;
        animator.SetBool("isWalking", false);
        float duracionAgua = 4f;
        if (GameManager.Instance.negacion > GameManager.Instance.aceptacion
            && GameManager.Instance.negacion > GameManager.Instance.duda)
            duracionAgua = 6f;
        yield return new WaitForSeconds(duracionAgua);

        // 4. Camina al pasillo
        agent.speed = 1.2f;
        agent.SetDestination(N2_Pasillo.position);
        yield return new WaitUntil(() =>
            agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f);

        // 5. Camina a la base de las escaleras
        agent.SetDestination(N2_EscalerasAbajo.position);
        yield return new WaitUntil(() =>
            agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f);

        // 6. Sube las escaleras
        animator.SetBool("isWalking", false);
        animator.SetBool("isStairsUp", true);
        agent.speed = 0.8f;
        agent.SetDestination(N2_EscalerasArriba.position);
        yield return new WaitUntil(() =>
            agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f);

        animator.SetBool("isStairsUp", false);

        // 7. Camina a la cama
        agent.speed = 1.2f;
        agent.SetDestination(N2_Cama.position);
        yield return new WaitUntil(() =>
            agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f);

        // 8. Rota hacia la cama y se sienta
        agent.updateRotation = false;
        transform.rotation = Quaternion.LookRotation(N2_Cama.forward);
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", true);
    }
}