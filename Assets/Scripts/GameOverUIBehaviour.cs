using UnityEngine;
using UnityEngine.UI;

public class GameOverUIBehaviour : MonoBehaviour
{
    // eventos
    public delegate void GameOverUiEvent();
    public static event GameOverUiEvent OnRestartClick;
    public static event GameOverUiEvent OnContinueEventClick;

    // label score
    // label nível
    [Header("Label do score")]
    [SerializeField]
    private Text labelScore;
    [Header("Label do level")]
    [SerializeField]
    private Text labelLevel;
    //restart button
    [Header("Button para restart")]
    [SerializeField]
    private Button buttonRestart;
    // continue button
    private Button buttonContinue;
    [Header("Imagens para as vidas")]
    [SerializeField]
    private Image[] lifeCounter;
    /// <summary>
    /// Configura a quantidade de vidas restantes 
    /// </summary>
    public int LifeCounter
    {
        set
        {
            for (int i = 0; i < lifeCounter.Length; i++)
            {
                lifeCounter[i].gameObject.SetActive(false);
            }

            for (int i =0; i < value; i++)
            {
                lifeCounter[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Texto do label do score
    /// </summary>
    public string ScoreText
    {
        get
        {
            return labelScore.text;
        }
        set
        {
            labelScore.text = value;
        }
    }
    /// <summary>
    /// Texto do label do level
    /// </summary>
    public string LevelScore
    {
        get
        {
            return labelLevel.text;
        }

        set
        {
            labelLevel.text = value;
        }

    }

    // Use this for initialization
    void Start()
    {
        // configura os eventos no botão
        buttonRestart.onClick.AddListener(Restart);
        buttonContinue.onClick.AddListener(Continue);
    }
    /// <summary>
    /// Restart
    /// </summary>
    private void Restart()
    {
        if(OnRestartClick != null)
        {
            OnRestartClick();
        }
    }
    /// <summary>
    /// Continue
    /// </summary>
    private void Continue()
    {
        if(OnContinueEventClick != null)
        {
            OnContinueEventClick();
        }
    }
    


}
