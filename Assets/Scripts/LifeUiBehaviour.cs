using UnityEngine;

[RequireComponent (typeof(Animator))]
public class LifeUiBehaviour : MonoBehaviour
{
    /// <summary>
    /// Components
    /// </summary>
    private Animator _animator;

    void Start()
    {
        this._animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Executa animação
    /// </summary>
    public void Animate()
    {
        this._animator.enabled = true;
    }
    /// <summary>
    /// Desativa o objeto
    /// </summary>
    public void Desactivate()
    {
        gameObject.SetActive(false);
    }
  
}
