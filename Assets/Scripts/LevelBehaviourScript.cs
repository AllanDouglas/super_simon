using UnityEngine;

public class LevelBehaviourScript : MonoBehaviour
{
    // CONTANTES
    private readonly string LEVEL_PLAYER_PREFS = "_LEVEL_";
    private readonly string SCORE_PLAYER_PREFS = "_SCORE_";

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
    public AlertBehaviour Alert;

    [Header("Controles")]
    public int levelModulator = 12;

    // propriedades privadas
    private int score = 0;
    private int level = 1;
    private int combo = 1;
    private int lifes = 3;    
    private int comboCounter = 0;
    private bool isPlayerTurn = false;
    private bool isPlaying = true;

    private static int continues = 0;

    private UnityAdsHelper AdsHelper = new UnityAdsHelper();
    //audios
    private AudioClip ac_alarm, ac_comboBreaker, ac_levelup;

    // components
    private AudioSource _audioSource;

    // Use this for initialization
    void Awake()
    {
        // para quando abrir o game pela primeira vez a 
        //quantidade de continues ser igual a 1
        if (continues == 0)
            continues = 1;

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
        // Simon
        SimonBehaviourScript.OnEndPlay += HandlerEndPlaySimon;
        SimonBehaviourScript.OnStartPlay += HandlerStartPlaySimon;
        // Timer
        TimerBehaviourScript.OnOverTime += HandlerOverTime;
        // Game Over 
        GameOverUIBehaviour.OnRestartClick += Restart;
        GameOverUIBehaviour.OnContinueClick += HandlerContinueEvent;
        // InGame
        InGameUiBehaviourScript.OnPauseClicked += HandlerPauseEvent;
        InGameUiBehaviourScript.OnMuteClicked += HandlerMuteEvent;
        // ADS
#if UNITY_ANDROID
        UnityAdsHelper.OnFinished += HandlerVideoFinished;
#endif

    }
    /// <summary>
    /// StartUp the game
    /// </summary>
    public void StartUp()
    {
        // adiciona uma rodada ao simon        
        this.Simon.AddLight();
        Play();
    }
    /// <summary>
    /// Manipula o evento de vídeo completado
    /// </summary>
    private void HandlerVideoFinished()
    {
        continues = 3;

        GameOverUi.TextContinues = continues.ToString();
        GameOverUi.ContinueLabelText = "Continue";
        
    }

    /// <summary>
    /// Manipula o evento de click no botão continue
    /// </summary>
    private void HandlerContinueEvent()
    {
        if (continues > 0)
        {
            Debug.Log("continuando");
            Continue();
            return;
        }
        Debug.Log("Exibindo o vídeo de ADS");
        // exibe o vídeo do unity ads
#if UNITY_ANDROID
        this.AdsHelper.Show();
#endif
    }

    /// <summary>
    /// Retoma a partida se ainda houveram continues
    /// </summary>
    public void Continue()
    {
        if (continues <= 0)
            return;

        --continues; // desconta o continue
        if (lifes < 3) // verifica se a quantida de vidas é menor que o maximo
        {
            lifes += 1; // devolve uma vida 
            InGameUi.AddLife(); // exibe a vida
        }

        GameOverUi.gameObject.SetActive(false);
        Simon.gameObject.SetActive(true);

        Play();
    }
    /// <summary>
    /// Inicia o simon
    /// </summary>
    private void Play()
    {
        //inicia
        Alert.Text = "Memorize the sequence.";
        Invoke("PlaySimon", 1f);
    }

    /// <summary>
    /// Manipula o evento de termino da sequencia do simon
    /// para liberar a rodada do jogado
    /// </summary>
    /// <param name="simon"></param>
    private void HandlerEndPlaySimon(SimonBehaviourScript simon)
    {
        Alert.Text = "You turn.";

        // libera a rodada para o jogador
        isPlayerTurn = true;
        // inicia o temporizador
        this.Timer.Play();

    }
    /// <summary>
    /// Manipula o start do play
    /// </summary>
    /// <param name="simon"></param>
    private void HandlerStartPlaySimon(SimonBehaviourScript simon)
    {
        Alert.Text = "Memorize the sequence.";

        // trava a jogada do player
        isPlayerTurn = false;

    }
    /// <summary>
    /// Manipula o vento de mute
    /// </summary>
    private void HandlerMuteEvent(bool isMute)
    {
        AudioListener.pause = !AudioListener.pause;

    }
    /// <summary>
    /// Manipula o evento de pause
    /// </summary>
    private void HandlerPauseEvent()
    {
        // verifica se esta na vez do jogado
        if (isPlayerTurn)
        {
            isPlaying = !isPlaying; // inverte o status da partida

            // pausa ou libera o simon de acordo com o status 
            if (isPlaying)
            {
                Debug.Log("### RESUMING ###");
                // toca
                isPlaying = true;
                Alert.Text = "";
                Timer.Resume();
            }
            else
            {
                Debug.Log("### PAUSING ###");
                Alert.Text = "Paused.";
                // pausa
                Timer.Pause();
            }


        }
        else
        {
            Alert.Text = "Pause only your turn.";
        }
    }

    /// <summary>
    /// Manipula o evento dispardo quando o tempo acaba
    /// </summary>
    private void HandlerOverTime()
    {
        _audioSource.PlayOneShot(ac_alarm); // toca o som da sirene
        // game over
        GameOver();

    }

    /// <summary>
    /// Captura a Light clicada
    /// </summary>
    /// <param name="light"></param>
    private void HandlerTouchLight(LightBehaviourScript light)
    {
        //se não for o turno do jogador ignora ou o jogo está pausado
        if (!isPlayerTurn | !isPlaying) return;

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
                isPlayerTurn = false;
                Debug.Log("### Level UP ###");
                // level up
                LevelUp();
            }

        }
        else
        {
            // erro
            Debug.Log("### Errou ###");
            CurrentLight.Blink();
            Invoke("Error",.5f);            
            
        }


    }
    /// <summary>
    /// Executa a sequencia do simon
    /// </summary>
    private void PlaySimon()
    {
        if(isPlaying) Simon.Play();
    }

    /// <summary>
    /// Sobe um nivel 
    /// Acrecenta um luz no simon
    /// </summary>
    private void LevelUp()
    {

        Alert.Text = "Level Up!";
        // toca o audio
        _audioSource.PlayOneShot(ac_levelup);
        // reseta o timer
        Timer.Stop();
        // incrementa o level
        level++;
        InGameUi.Level = level;


        // se o nivel for menor que o modelador
        if (level < levelModulator)
        {
            // incrementa o tempo
            Timer.MaxTime = Timer.MaxTime + incrementTime;
            // adiciona uma luz no simon
            Simon.AddLight();
        }
        else
        {
            //se não troca uma Light
            Simon.ChangeOne();
        }

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
    /// Game Over
    /// </summary>
    private void GameOver()
    {
        isPlaying = false;
        isPlayerTurn = false;

        GameOverUi.TextLevel = level.ToString();
        GameOverUi.TextScore = score.ToString();

        GameOverUi.TextBestLevel = PlayerPrefs.GetInt(LEVEL_PLAYER_PREFS).ToString();
        GameOverUi.TextBestScore = PlayerPrefs.GetInt(SCORE_PLAYER_PREFS).ToString();


        GameOverUi.TextContinues = continues.ToString();

        if (continues > 0)
        {
            GameOverUi.ContinueLabelText = "Continue";
        }
        else
        {
            GameOverUi.ContinueLabelText = "Get Continues!";
        }


        GameOverUi.gameObject.SetActive(true);
        Simon.gameObject.SetActive(false);
        Alert.Text = "";
    }
    /// <summary>
    /// Grava os records na memoria
    /// </summary>
    private void SaveRecords()
    {
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
    }

    /// <summary>
    /// Erro
    /// </summary>
    private void Error()
    {
        //remove uma vida
        this.lifes--;
        InGameUi.RemoveLife();        
        // verifica se ainda tem vidas 
        //se não houver gameover
        if (lifes <= 0)
        {
            GameOver();
            return;
        }

        Alert.Text = "Ops! Try again.";
        // interrompe a jogada do player
        isPlayerTurn = false;
        // pausa o timer
        Timer.Stop();
        // quebra o combo
        ComboBreak();
        //reinicia
        Invoke("PlaySimon", 2f);
    }
    /// <summary>
    /// Reinicia o level
    /// </summary>
    public void Restart()
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
        GameOverUIBehaviour.OnRestartClick -= Restart;        
        GameOverUIBehaviour.OnContinueClick -= HandlerContinueEvent;
        InGameUiBehaviourScript.OnPauseClicked -= HandlerPauseEvent;
        InGameUiBehaviourScript.OnMuteClicked -= HandlerMuteEvent;
#if UNITY_ANDROID
        UnityAdsHelper.OnFinished -= HandlerVideoFinished;
#endif
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}