using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelBehaviourScript : MonoBehaviour {
    //propriedades publicas
    [Header("Simon")]
    public SimonBehaviourScript Simon;
    [Header("In Game Ui")]
    public InGameUiBehaviourScript InGameUi;    

    // propriedades privadas
    private int score = 0;
    private int level = 1;
    private int combo = 1;
    private int comboCounter = 0;
    private bool playerTurn = false;

	// Use this for initialization
	void Start () {
        // configura a UI
        InGameUi.Level = level;
        InGameUi.Combo = combo;
        InGameUi.Score = score;
        // configura o evento de captura
        LightBehaviourScript.OnTouchEvent += HandlerTouchLight;
        SimonBehaviourScript.OnEndPlay += HandlerEndPlaySimon;
        SimonBehaviourScript.OnStartPlay += HandlerStartPlaySimon;
        // Inicia o Game
        StartUp();

	}
    /// <summary>
    /// StartUp the game
    /// </summary>
    private void StartUp()
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
        LightBehaviourScript CurrentLight= Simon.GetCurrentLight();
        // compara com light clicada        
        if (light == CurrentLight)
        {

            Debug.Log("### Acertou ###");
            // a cada acerto soma 
            ComboUp();

            // verifica se terminou a sequencia
            if(Simon.SequenceCursor == Simon.SequenceLenght)
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
        // incrementa o level
        level++;
        InGameUi.Level = level;

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
        if (combo >= 4) return;

        // incrementa o contador de combo
        comboCounter++;
        InGameUi.ComboCounter(comboCounter); // atualiza a ui
        if(comboCounter > 4)
        {
            comboCounter = 0; // zera o contador de combo            
            combo++; // incrementa o multiplicador de combo

            // atualiza a UI
            InGameUi.ResetComboCounters();
            InGameUi.Combo = combo;

        }

        score += 10 * combo; // pontua
        InGameUi.Score = score; // atualiza a UI
    }
    /// <summary>
    /// Zera o combo
    /// </summary>
    private void ComboBreak()
    {
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



        // quebra o combo
        ComboBreak();
        //reinicia
        Invoke("PlaySimon", 2f);
    }
    

}