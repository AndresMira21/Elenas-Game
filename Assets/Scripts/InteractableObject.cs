using UnityEngine;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    [System.Serializable]
    public class MensajePorDia
    {
        public int dia;
        [TextArea] public string message;
        public float messageTime = 2f;
        public bool oneUseOnly = false;
        [HideInInspector] public bool used = false;
    }

    [Header("Mensajes por día")]
    public List<MensajePorDia> mensajesPorDia = new List<MensajePorDia>();

    [Header("Mensaje por defecto (si no hay uno para el día actual)")]
    [TextArea] public string mensajePorDefecto = "";
    public float tiempoPorDefecto = 2f;

    public void Interact()
    {
        int day = GameManager.Instance.currentDay;

        MensajePorDia entry = mensajesPorDia.Find(m => m.dia == day);

        if (entry == null)
        {
            if (!string.IsNullOrEmpty(mensajePorDefecto))
                DialogueManager.Instance.ShowThought(mensajePorDefecto, tiempoPorDefecto);
            return;
        }

        if (entry.oneUseOnly && entry.used)
            return;

        entry.used = true;
        GameManager.Instance.RegisterExploration();
        DialogueManager.Instance.ShowThought(entry.message, entry.messageTime);
        Debug.Log(entry.message);
    }
}