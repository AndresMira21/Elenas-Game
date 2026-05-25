using UnityEngine;
using UnityEngine.AI;

public class TestMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Puntos de destino")]
    public Transform[] puntosNegacion;
    public Transform[] puntosDuda;
    public Transform[] puntosAceptacion;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ActualizarComportamiento();
    }

    public void ActualizarComportamiento()
    {
        GameManager gm = GameManager.Instance;

        if (gm.negation >= gm.acceptance && gm.negation >= gm.doubt)
            ComportamientoNegacion();
        else if (gm.doubt >= gm.acceptance)
            ComportamientoDuda();
        else
            ComportamientoAceptacion();
    }

    void ComportamientoNegacion()
    {
        agent.speed = 0.8f;
        if (puntosNegacion.Length > 0)
            agent.SetDestination(puntosNegacion[0].position);
    }

    void ComportamientoDuda()
    {
        agent.speed = 1.2f;
        if (puntosDuda.Length > 0)
            agent.SetDestination(puntosDuda[0].position);
    }

    void ComportamientoAceptacion()
    {
        agent.speed = 1.5f;
        if (puntosAceptacion.Length > 0)
            agent.SetDestination(puntosAceptacion[0].position);
    }

    void Update()
    {
        if (agent.velocity.magnitude > 0.1f)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }
}