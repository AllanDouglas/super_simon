using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelBehaviourScript : MonoBehaviour
{
    // CONTANTES
    private const string LEVEL_PLAYER_PREFS = "_LEVEL_";
    private const string SCORE_PLAYER_PREFS = "_SCORE_";

    //propriedades publicas
    [Header("Tempo incrementador por level")]
    public float incrementTime = 0.5f;
    [Header("Simon")]
    public SimonBehaviourScript Simon;
    [Header("Timer")]
    public TimerBehaviourScript Timer;
    [Header("UI's")]
    public InGameUiBehaviourScript InGameUi;
    public GameOverUIBehaviour GameOverUi;

    // propriedades privadas
    private int score = 0;
    private int level = 1;
    private int combo = 1;
    private int lifes = 3;
    private int comboCounter = 0;
    private bool playerTurn = false;

    //audios
    private AudioClip ac_alarm, ac_comboBreaker, ac_levelup;


    // components
    private AudioSource _audioSource;

    // Use this for initialization
    void OnEnable()
    {
        // audio clips
        ac_alarm = Resources.Load<AudioClip>("Sounds/FX/Alarm") as AudioClip;
        ac_comboBreaker = Resources.Load<AudioClip>("Sounds/FX/Downer01") as AudioClip;
        ac_levelup = Resources.Load<AudioClip>("Sounds/FX/Rise06") as AudioClip;
        //components
        _audioSource = gameObject.AddComponent<AudioSource>();

        // configura a UI
        InGameUi.Level = level;
        InGameUi.Combo = combo;
        InGameUi.Score = score;
        // configura o evento de captura
        LightBehaviourScript.OnTouchEvent += HandlerTouchLight;
        SimonBehaviourScript.OnEndPlay += HandlerEndPlaySimon;
        SimonBehaviourScript.OnStartPlay += HandlerStartPlaySimon;
        TimerBehaviourScript.OnOverTime += HandlerOverTime;
        GameOverUIBehaviour.OnRestartClick += Restart;


    }
    /// <summary>
    /// StartUp the game
    /// </summary>
    public void StartUp()
    {
        // adiciona uma rodada ao simon        
        this.Simon.AddLight();
        // executa o simon
        this.Simon.Play();
    }
    /// <summary>
    /// Manipula o evento de termino da sequencia do simon
    /// para liberar a rodada do jogado
    /// </summary>
    /// <param name="simon"></param>
    private void HandlerEndPlaySimon(SimonBehaviourScript simon)
    {
        // libera a rodada para o jogador
        playerTurn = true;
        // inicia o temporizador
        this.Timer.Play();

    }
    /// <summary>
    /// Manipula o start do play
    /// </summary>
    /// <param name="simon"></param>
    private void HandlerStartPlaySimon(SimonBehaviourScript simon)
    {
        // trava a jogada do player
        playerTurn = false;

    }
    /// <summary>
    /// Manipula o evento dispardo quando o tempo acaba
    /// </summary>
    private void HandlerOverTime()
    {
        _audioSource.PlayOneShot(ac_alarm); // toca o som

        // ############ seta os records
        if (level > PlayerPrefs.GetInt(LEVEL_PLAYER_PREFS))
        {
            PlayerPrefs.SetInt(LEVEL_PLAYER_PREFS, level);
        }
        if (score > PlayerPrefs.GetInt(SCORE_PLAYER_PREFS))
        {
            PlayerPrefs.SetInt(SCORE_PLAYER_PREFS, score);
        }
        // ############ seta os records

        GameOverUi.TextLevel = level.ToString();
        GameOverUi.TextScore = score.ToString();

        GameOverUi.TextBestLevel = PlayerPrefs.GetInt(LEVEL_PLAYER_PREFS).ToString();
        GameOverUi.TextBestScore = PlayerPrefs.GetInt(SCORE_PLAYER_PREFS).ToString();

        GameOverUi.LifeCounter = lifes;

        GameOverUi.gameObject.SetActive(true);
        Simon.gameObject.SetActive(false);

    }

    /// <summary>
    /// Captura a Light clicada
    /// </summary>
    /// <param name="light"></param>
    private void HandlerTouchLight(LightBehaviourScript light)
    {
        //se não for o turno do jogador ignora
        if (!playerTurn) return;

        // liga a lampada
        StartCoroutine(Simon.Blink(light));

        // pego a luz atual da squencia do simon
        LightBehaviourScript CurrentLight = Simon.GetCurrentLight();
        // compara com light clicada        
        if (light == CurrentLight)
        {
            Debug.Log("### Acertou ###");
            // a cada acerto soma 
            ComboUp();
            Scorer();
            // verifica se terminou a sequencia
            if (Simon.SequenceCursor == Simon.SequenceLenght)
            {
                // quando passa de nivel termina a jogada do 
                playerTurn = false;
                Debug.Log("### Level UP ###");
                // level up
                LevelUp();
            }

        }
        else
        {
            Debug.Log("### Errou ###");
            // erro
            Error();
        }


    }
    /// <summary>
    /// Executa a sequencia do simon
    /// </summary>
    private void PlaySimon()
    {
        Simon.Play();
    }

    /// <summary>
    /// Sobe um nivel 
    /// Acrecenta um luz no simon
    /// </summary>
    private void LevelUp()
    {
        // toca o audio
        _audioSource.PlayOneShot(ac_levelup);
        // reseta o timer
        Timer.Stop();
        // incrementa o level
        level++;
        InGameUi.Level = level;
        // incrementa o tempo
        Timer.MaxTime = Timer.MaxTime + incrementTime;
        // adiciona uma luz no simon
        Simon.AddLight();
        // toca a sequencia novamente;
        Invoke("PlaySimon", 2f);

    }
    /// <summary>
    /// Eleva o combo
    /// </summary>
    private void ComboUp()
    {
        // se o combo já está no maximo
        if (combo < 4)
        {
            // incrementa o contador de combo
            comboCounter++;
            InGameUi.ComboCounter(comboCounter); // atualiza a ui
            if (comboCounter > 4)
            {
                comboCounter = 0; // zera o contador de combo            
                combo++; // incrementa o multiplicador de combo

                // atualiza a UI
                InGameUi.ResetComboCounters();
                InGameUi.Combo = combo;

            }
        }



    }
    /// <summary>
    /// Pontuar
    /// </summary>
    private void Scorer()
    {
        score += 10 * combo; // pontua
        InGameUi.Score = score; // atualiza a UI
    }

    /// <summary>
    /// Zera o combo
    /// </summary>
    private void ComboBreak()
    {
        _audioSource.PlayOneShot(ac_comboBreaker); // toca o som

        comboCounter = 0;
        combo = 1;
        // atualiza a UI
        InGameUi.ResetComboCounters();
        InGameUi.Combo = combo;

    }
    /// <summary>
    /// Erro
    /// </summary>
    private void Error()
    {
        // interrompe a jogada do player
        playerTurn = false;
        // pausa o timer
        this.Timer.Pause();
        // quebra o combo
        ComboBreak();
        //reinicia
        Invoke("PlaySimon", 2f);
    }
    /// <summary>
    /// Reinicia o level
    /// </summary>
    private void Restart()
    {
        UnityEngine.
            SceneManagement.
            SceneManager.LoadScene(
                UnityEngine.
                SceneManagement.
                SceneManager.GetActiveScene().buildIndex
                );
    }

    /// <summary>
    /// Quando o nível é destruido
    /// </summary>
    public void OnDestroy()
    {
        // remove os eventos
        LightBehaviourScript.OnTouchEvent -= HandlerTouchLight;
        SimonBehaviourScript.OnEndPlay -= HandlerEndPlaySimon;
        SimonBehaviourScript.OnStartPlay -= HandlerStartPlaySimon;
        TimerBehaviourScript.OnOverTime -= HandlerOverTime;
    }
}