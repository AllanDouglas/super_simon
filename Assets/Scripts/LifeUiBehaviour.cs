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
        this._animator.enabled = false;
    }

    /// <summary>
    /// Executa animação
    /// </summary>
    public void Remove()
    {
        this._animator.enabled = true;
    }
    /// <summary>
    /// Desativa o objeto
    /// </summary>
    public void Desactivate()
    {
        this._animator.enabled = false;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
  
}
