using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<Character> characters = new List<Character>();
    public Character characterCurrent = null;

    public void CreateCharacter(int index,Vector3 position)
    {
        if (characterCurrent == null)
        {
            characterCurrent = characters[index];
            characters[index].gameObject.SetActive(true);
            characters[index].transform.SetParent(null);
            characters[index].transform.position = position;
        }
    }
}
