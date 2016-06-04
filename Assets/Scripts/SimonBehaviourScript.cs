using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonBehaviourScript : MonoBehaviour {
    // eventos
    public delegate void SimonEvent(SimonBehaviourScript simon);
    /// <summary>
    /// Disparado quando começa a tocar 
    /// </summary>
    public static event SimonEvent OnStartPlay;
    /// <summary>
    /// Disparado quando terminar a sequencia
    /// </summary>
    public static event SimonEvent OnEndPlay;

	[Header("Lights")]
	public List<LightBehaviourScript> lights = new List<LightBehaviourScript>();
    [Header("Time to change the light")]
    public float timeToChange = 1.3f;
    [Header("Accelerate starting from")]
    public int accelerateIn = 5;

    // privates    
    private int lastIndexChange = 0;// ultimo index que foi alterado no array de sequence
    private int sequenceCursor = 0; // indice da sequencia para comparação
    
    // propriedades

    /// <summary>
    /// Posicao atual do cursor
    /// </summary>
    public int SequenceCursor
    {
        get { return sequenceCursor; }
    }
    /// <summary>
    /// Tamanho maximo da sequencia
    /// </summary>
    public int SequenceLenght
    {
        get { return sequence.Count; }
    }
	
	/// <summary>
	/// Sequencia de luzes 
	/// </summary>
	private List<LightBehaviourScript> sequence = new List<LightBehaviourScript>();

	// adiciona uma nova luz para a sequencia
	public void AddLight(){
		// randomiza uma posicao das luzes possives
		int index = Random.Range(0, lights.Count);
		LightBehaviourScript light = lights [index];
		// adiciona na sequencia
		sequence.Add(light);
        lastIndexChange = sequence.Count;

	}
	/// <summary>
	/// Inicia a sequencia
	/// </summary>
	public void Play(){
        // se o evento estiver setado executa-o
        if(OnStartPlay != null)
        {
            OnStartPlay(this);
        }

		// reseta a contagem da sequenica de comparação
		sequenceCursor = 0;
		// inicia a sequencia
		StartCoroutine (PlaySequence ());
	}
	// toca a sequencia em uma corrotina
	private IEnumerator PlaySequence(){

        // para cada nota
        int index = 0;
		foreach(LightBehaviourScript light in this.sequence )
        {

            float timeToPlay = timeToChange;
            // verifica se existem mais de X luzes  
            //  verifica se é a penultima luz adicionada
            //   se for volta o tempo para normal       
            if (index != (lastIndexChange - 1) && sequence.Count > accelerateIn )
            {
                timeToPlay *= .3f;// reduz o tempo de tocar pela metade                
            }

            index++;
            Debug.Log(timeToPlay);
            yield return Blink(light,timeToPlay);
            
        }

        // se o evento de termino estiver setado executa-o
        if(OnEndPlay != null)
        {
            OnEndPlay(this);
        }

	}
    /// <summary>
    /// Faz a luz piscar
    /// </summary>
    /// <param name="light"></param>
    /// <returns></returns>
    public IEnumerator Blink(LightBehaviourScript light)
    {
        yield return Blink(light, timeToChange);
        
    }
    /// <summary>
    /// Faz a luz piscar
    /// </summary>
    /// <param name="light"></param>
    /// <param name="timeToChange"></param>
    /// <returns></returns>
    public IEnumerator Blink(LightBehaviourScript light, float timeToChange)
    {
        // toca a nota 
        light.TurnOn();
        // espera
        yield return new WaitForSeconds(timeToChange);
        // apaga a nota
        light.TurnOff();
        // aguarda meio segundo antes de ligar 
        yield return new WaitForSeconds(0.5f);
    }

	/// <summary>
	/// Troca um elemento da sequencia por outro aleatorio
	/// </summary>
	public void ChangeOne(){
		// randomiza uma posicao das luzes possives
		int index = Random.Range(0, lights.Count);
		LightBehaviourScript light = lights [index];
		// pega uma posicao aleatorio da sequencia
		int seqRandom = Random.Range(0, sequence.Count);
		// adiciona na sequencia
		sequence[seqRandom] = light;
        lastIndexChange = index;

	}

	/// <summary>
	/// Recupera a Light atual e adianta o cursor
	/// </summary>
	/// <returns>The current light.</returns>
	public LightBehaviourScript GetCurrentLight(){
		LightBehaviourScript light = sequence [sequenceCursor];
		sequenceCursor++;
		return  light;			
	}


}
