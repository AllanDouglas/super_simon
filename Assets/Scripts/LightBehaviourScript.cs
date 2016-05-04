using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class LightBehaviourScript : MonoBehaviour {

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


	// ativar
	public void TurnOn(){
		this._spriteRenderer.sprite = this.active;
		//this._audioSource.PlayOneShot (this.sound);
	}
	// desativar 
	public void TurnOff(){
		this._spriteRenderer.sprite = this.standard;
	}


}
