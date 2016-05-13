using UnityEngine;
using UnityEngine.UI;

public class AlertBehaviour : MonoBehaviour
{
    // component text
    [Header("Labels")]
    [SerializeField]
    private Text _text;

  
    public string Text
    {
        set
        {
            _text.text = value;
        }
    }
    
}
