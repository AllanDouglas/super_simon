using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controle da UI In Game
/// </summary>
public class InGameUiBehaviourScript : MonoBehaviour {

    // fields
    [Header("Labels")]
    [SerializeField]
    private Text score; // label do score
    [SerializeField]
    private Text level; // label do level
    [SerializeField]
    private Text combo; // label do combo
    [SerializeField]
    private Text continues; // label da quantidade de continues
    [Header("Contadores")]
    [SerializeField]
    private Image[] comboCounters; // array de imagens para representar os contadores de combo
    [SerializeField]
    private LifeUiBehaviour[] lifeCounters; // array de imagens para representar os contadores de vida
    [Header("Colors combo counter")]
    [SerializeField]
    private Color activeColor; // cor quando o contador está ativo
    [SerializeField]
    private Color inactiveColor; // cor quando o contador está inativo
    // botão mute
    [Header("Button's")]
    [SerializeField]
    private Button buttonMute;
    [SerializeField]
    private Sprite audioOnSprite;
    [SerializeField]
    private Sprite audioOffSprite;
   

    // controle do audio
    private bool isMute = false;

    /// <summary>
    /// Configura o score
    /// </summary>
    public int Score
    {
        set { score.text = value.ToString("D6"); }        
    }
    /// <summary>
    /// Configura o level
    /// </summary>
    public int Level
    {
        set { level.text = value.ToString(); }
    }
    /// <summary>
    /// Configura o texto do combo
    /// </summary>
    public int Combo
    {
        set { combo.text = "X" + value.ToString(); }
    }

    /// <summary>
    /// Pinta a quantidade de contadores
    /// </summary>
    /// <param name="number"></param>
    public void ComboCounter(int number)
    {
        number = (number > this.comboCounters.Length) ? this.comboCounters.Length : number;

        for(int i =0; i < number; i++)
        {
            comboCounters[i].color = this.activeColor;
        }
    }
    /// <summary>
    /// Reseta o contador de combo
    /// </summary>
    public void ResetComboCounters()
    {
        for (int i = 0; i < this.comboCounters.Length; i++)
        {
            comboCounters[i].color = this.inactiveColor;
        }
    }
    /// <summary>
    /// Recupera uma vida ao contador
    /// </summary>
    public void AddLife()
    {
        for (int i = 0; i < this.lifeCounters.Length; i++)
        {
            if (lifeCounters[i].gameObject.activeSelf == false)
            {
                lifeCounters[i].gameObject.SetActive(true);
                break;
            }
        }
    }
    /// <summary>
    /// Retira uma vida do contador
    /// </summary>
    public void RemoveLife()
    {
        for (int i = 0; i < this.lifeCounters.Length; i++)
        {
            //executa animação da retirada da vida
            if (lifeCounters[i].gameObject.activeSelf == true)
            {
                lifeCounters[i].Remove();
                break;
            }
        }
    }
    /// <summary>
    /// Renova todas as vidas do contador
    /// </summary>
    public void ResetLifeCounters()
    {
        for (int i = 0; i < this.lifeCounters.Length; i++)
        {
            lifeCounters[i].gameObject.SetActive(true);
        }
    }

    // on start
    void Start()
    {
        ResetComboCounters();
    }         
    
    /// <summary>
    /// muta
    /// </summary>
    public void Mute()
    {
        isMute = !isMute;
        Camera.main.GetComponent<AudioListener>().enabled = !isMute;
        if (isMute)
        {
            buttonMute.image.sprite = audioOffSprite;
        }
        else
        {
            buttonMute.image.sprite = audioOnSprite;
        }
    }

}
