using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(SpriteRenderer))]

public class LightBehaviourScript : MonoBehaviour, ISwitch {

	//eventos
	public delegate void LightEvent(LightBehaviourScript light);
	public static event LightEvent OnTouchEvent; // diparado quando o elemento é tocado

	[Header("Sound")]
	[SerializeField]
	private AudioClip sound;

	[Header("Sprites")]
	// sprite padrao
	[SerializeField]
	private Sprite standard; 
	// sprite ativo
	[SerializeField]
	private Sprite active;

	// componentes
	private SpriteRenderer _spriteRenderer; // sprite renderer 
	private AudioSource _audioSource; // audio source
	//privados

	// Use this for initialization
	void Awake () {
		this._spriteRenderer = GetComponent<SpriteRenderer> (); // get the sprite renderer	
		this._audioSource = gameObject.AddComponent<AudioSource>(); // adiciona o componente de audiosource no objeto
	}
	
	// quanto é tocado 
	void OnMouseDown(){
		// dispara o evento de que foi tocado
		if(OnTouchEvent != null)
			OnTouchEvent(this);
	}


	/// <summary>
    /// Ativa a luz
    /// </summary>
	public void TurnOn(){
		this._spriteRenderer.sprite = this.active;
		this._audioSource.PlayOneShot (this.sound);
	}
	/// <summary>
    /// Apaga a luz
    /// </summary>
	public void TurnOff(){
		this._spriteRenderer.sprite = this.standard;
	}
    /// <summary>
    /// Piscar
    /// </summary>
    public void Blink()
    {
        StartCoroutine(Blinking());
    }
    /// <summary>
    /// Faz a luz piscar algumas vezes
    /// </summary>
    /// <returns></returns>
    private IEnumerator Blinking()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Acende");
            this._spriteRenderer.sprite = this.active;
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Apaga");
            this._spriteRenderer.sprite = this.standard;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
