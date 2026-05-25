using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ElenaSecuenciaNivel2 : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Puntos Nivel 2")]
    public Transform N2_Ventana;
    public Transform N2_Fregadero;
    public Transform N2_Pasillo;
    public Transform N2_Cama;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObject.SetActive(false); // Elena empieza invisible
    }

    public void IniciarSecuencia()
    {
        gameObject.SetActive(true);
        StartCoroutine(SecuenciaNivel2());
    }

    IEnumerator SecuenciaNivel2()
    {
        // 1. Aparece mirando por la ventana
        transform.position = N2_Ventana.position;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(3f);

        // 2. Camina al fregadero
        animator.SetBool("isWalking", true);
        agent.SetDestination(N2_Fregadero.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f);

        // 3. Abre el agua 4 segundos
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(4f);

        // 4. Camina por el pasillo
        animator.SetBool("isWalking", true);
        agent.SetDestination(N2_Pasillo.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f);

        // 5. Camina a la cama
        agent.SetDestination(N2_Cama.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f);

        // 6. Se sienta
        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", true);
    }
}