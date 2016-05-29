using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]

public class TimerBehaviourScript : MonoBehaviour, ITimer
{
    // eventos
    public delegate void TimerEvent();
    public static event TimerEvent OnOverTime; // disparado quando o tempo chega a zero


    // tempo            
    [SerializeField]
    private float maxTime;
    public float MaxTime
    {
        get
        {
            return maxTime;
        }
        set
        {
            maxTime = value;
            _slider.maxValue = value;
        }
    }

    // privates
    private bool isPlaying = false;// status 
    private float leftTime = 0; // tempo restante
    private float LeftTime
    {
        set
        {
            leftTime = value;
            _slider.value = value;
        }
    }


    // components
    // slider
    private Slider _slider;

    // Use this for initialization
    void Awake()
    {
        // captura o component  de slider
        _slider = GetComponent<Slider>();
        // configura o tempo maximo        
        _slider.maxValue = maxTime;

    }
    /// <summary>
    /// Inicia a Contagem
    /// </summary>
    public void Play()
    {
        isPlaying = true;
        _slider.value = leftTime;
        StartCoroutine(Playing());

    }
    /// <summary>
    /// Continua a contagem
    /// </summary>
    public void Resume()
    {
        isPlaying = true;
    }

    /// <summary>
    /// Puasa a contagem
    /// </summary>
    public void Pause()
    {
        isPlaying = false;
    }
    /// <summary>
    /// Para e reseta a contagem
    /// </summary>
    public void Stop()
    {
        // reseta o tempo
        LeftTime = 0;
        // para a corrotina
        isPlaying = false;
        StopCoroutine(Playing());
    }
    /// <summary>
    ///  Enquanto está Ativo
    /// </summary>
    /// <returns></returns>
    private IEnumerator Playing()
    {
        while (this.isPlaying == true)
        {
            // ajusta o tempo
            leftTime += Time.fixedDeltaTime;
            _slider.value = leftTime;

            // verifica se o tempo acabou     
            if (leftTime >= maxTime && OnOverTime != null)
            {
                OnOverTime();
                Stop();
            }

            yield return new WaitForFixedUpdate();
        }

    }



}
