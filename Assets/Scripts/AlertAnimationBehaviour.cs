using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Alertas 
/// </summary>
[RequireComponent(typeof(Animator))]

public class AlertAnimationBehaviour : MonoBehaviour, IAlert
{

    [Header("States")]
    [SerializeField]
    private string entrada;
    [SerializeField]
    private string saindo;
    [SerializeField]
    private Text message;

    // componets
    private Animator _animator;

    // Use this for initialization
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    /// <summary>
    /// Configura a mensagem que será exibida
    /// </summary>
    public string Message
    {
        set { this.message.text = value; }
    }

   
    /// <summary>
    /// Esconde
    /// </summary>
    public void Hide()
    {
        _animator.enabled = false;
    }
    /// <summary>
    /// Mostra
    /// </summary>
    public void Show()
    {
        _animator.enabled = true;
    }
}
