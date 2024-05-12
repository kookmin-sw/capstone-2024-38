using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentCharacter;
    
    private void Awake()
    {
        SelectCharacter(0);
    }

    private void SelectCharacter(int _index)
    {
        previousButton.interactable = (_index!=0);
        nextButton.interactable = (_index!= transform.childCount-1);
        
        // Fixed the typo from 'tranceform' to 'transform'
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeCharacter(int _change)
    {
        // Added a semicolon at the end of the statement
        currentCharacter += _change;
        SelectCharacter(currentCharacter);
    }
}
