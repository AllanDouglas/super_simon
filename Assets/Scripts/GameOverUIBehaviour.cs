using UnityEngine;
using UnityEngine.UI;

public class GameOverUIBehaviour : MonoBehaviour
{
    // const
    private static Color LIFE_ACTIVE_COLOR = new Color(1, 182 / 255, 182 / 255, 1);
    private static Color LIFE_INACTIVE_COLOR = new Color(236 / 255, 236 / 255, 236 / 255, 1);


    // eventos
    public delegate void GameOverUiEvent();
    public static event GameOverUiEvent OnRestartClick;
    public static event GameOverUiEvent OnContinueEventClick;

    // label score
    // label nível
    [Header("Labels")]
    [SerializeField]
    private Text labelScore;
    [SerializeField]
    private Text labelBestScore;
    [SerializeField]
    private Text labelLevel;
    [SerializeField]
    private Text labelBestLevel;
    //restart button
    [Header("Buttons")]
    [SerializeField]
    private Button buttonRestart;
    // continue button
    [SerializeField]
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

            for (int i = 0; i < value; i++)
            {
                lifeCounter[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Texto do label do score
    /// </summary>
    public string TextScore
    {
        get
        {
            return labelScore.text;
        }
        set
        {
            labelScore.text = "Score: " + value;
        }
    }

    /// <summary>
    /// Texto do label do melhor score
    /// </summary>
    public string TextBestScore
    {
        get
        {
            return labelBestScore.text;
        }

        set
        {
            labelBestScore.text = "Best: " + value;
        }

    }

    /// <summary>
    /// Texto do label do melhor level
    /// </summary>
    public string TextBestLevel
    {
        get
        {
            return labelBestLevel.text;
        }

        set
        {
            labelBestLevel.text = "Best: " + value;
        }

    }

    /// <summary>
    /// Texto do label do level
    /// </summary>
    public string TextLevel
    {
        get
        {
            return labelLevel.text;
        }

        set
        {
            labelLevel.text = "Level: " + value;
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
        if (OnRestartClick != null)
        {
            OnRestartClick();
        }
    }
    /// <summary>
    /// Continue
    /// </summary>
    private void Continue()
    {
        if (OnContinueEventClick != null)
        {
            OnContinueEventClick();
        }
    }

}
