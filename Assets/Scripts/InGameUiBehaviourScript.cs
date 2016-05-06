using UnityEngine;
using UnityEngine.UI;


public class InGameUiBehaviourScript : MonoBehaviour {

    // fields
    [Header("Labels")]
    [SerializeField]
    private Text score; // label do score
    [SerializeField]
    private Text level; // label do level
    [SerializeField]
    private Text combo; // label do combo
    [Header("Contadores")]
    [SerializeField]
    private Image[] comboCounters; // array de imagens para representar os contadores 
    [Header("Colors")]
    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private Color inactiveColor;



    /// <summary>
    /// Configura o score
    /// </summary>
    public int Score
    {
        set { score.text = value.ToString(); }        
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

    // on start
    void Start()
    {
        for (int i = 0; i < this.comboCounters.Length; i++)
        {
            comboCounters[i].color = this.inactiveColor;
        }
    }         
    
}
