using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCycleManager : MonoBehaviour
{
    public UIManager uiManager;

    [SerializeField] private float money = 100;
    [SerializeField] private int crystals = 1;

    [SerializeField] private Button buttonStrength;
    [SerializeField] private TextMeshProUGUI textStrengthName;
    [SerializeField] private TextMeshProUGUI textStrengthLevel;
    [SerializeField] private TextMeshProUGUI textStrengthPrice;
    public float valueStrength = 0.1f;
    [SerializeField] private float valueStrengthModificator = 0.1f;
    [SerializeField] private string nameStrength = "Strength";
    [SerializeField] private int lvStrength = 1;
    [SerializeField] private string lvStrengthText = "Lv.";
    [SerializeField] private int indexLvStrengthValue = 1; // add level
    [SerializeField] private float priceStrength = 15;
    [SerializeField] int indexPriceStrengthModificator = 5; // price modificator

    [SerializeField] private Button buttonSpeed;
    [SerializeField] private TextMeshProUGUI textSpeedName;
    [SerializeField] private TextMeshProUGUI textSpeedLevel;
    [SerializeField] private TextMeshProUGUI textSpeedPrice;
    public float valueSpeed = 0.1f;
    [SerializeField] private float valueSpeedModificator = 0.1f;
    [SerializeField] private string nameSpeed = "Speed";
    [SerializeField] private int lvSpeed = 1;
    [SerializeField] private string lvSpeedText = "Lv.";
    [SerializeField] private int indexLvSpeedValue = 1; // add level
    [SerializeField] private float priceSpeed = 15;
    [SerializeField] int indexPriceSpeedModificator = 5; // price modificator

    [SerializeField] private Button buttonCount;
    [SerializeField] private TextMeshProUGUI textCountName;
    [SerializeField] private TextMeshProUGUI textCountLevel;
    [SerializeField] private TextMeshProUGUI textCountPrice;
    public int valueCount = 1;
    [SerializeField] private int valueCountModificator = 1;
    [SerializeField] private string nameCount = "Count";
    [SerializeField] private int lvCount = 1;
    [SerializeField] private string lvCountText = "Lv.";
    [SerializeField] private int indexLvCountValue = 1; // add level
    [SerializeField] private float priceCount = 15;
    [SerializeField] int indexPriceCountModificator = 5; // price modificator
    private Planet myPlanet;

    public SpaceShark spaceSharkPrefab;

    [SerializeField] private bool gameStarted = false;
    private bool startSpawn = false;

    private void Start()
    {
        StartInit();
    }

    private void LateUpdate()
    {
        ButtonsInteractionUpdate();
        EconomyTextUpdate();
        StartUpdater();
    }

    private void StartUpdater()
    {
        if (gameStarted)
        {
            if (!startSpawn)
            {
                uiManager.ButtonsUpdateEnable(true); // show update buttons
                StartCoroutine(SpawnStartShips());
                startSpawn = true;
            }
        }
    }

    private void EconomyTextUpdate()
    {
        uiManager.SetCrystal(crystals.ToString());
        uiManager.SetMoney(money.ToString());
    }

    private void StartInit()
    {
        myPlanet = FindObjectOfType<Planet>();
        uiManager = FindObjectOfType<UIManager>();

        UpdateStrengthButton();
        UpdateSpeedButton();
        UpdateCountButton();

        uiManager.ButtonsUpdateEnable(false); // hide update buttons
    }

    private void UpdateStrengthButton()
    {
        textStrengthName.text = nameStrength;
        textStrengthLevel.text = lvStrengthText + lvStrength.ToString();
        textStrengthPrice.text = priceStrength.ToString();
    }

    private void UpdateSpeedButton()
    {
        textSpeedName.text = nameSpeed;
        textSpeedLevel.text = lvSpeedText + lvSpeed.ToString();
        textSpeedPrice.text = priceSpeed.ToString();
    }

    private void UpdateCountButton()
    {
        textCountName.text = nameCount;
        textCountLevel.text = lvCountText + lvCount.ToString();
        textCountPrice.text = priceCount.ToString();
    }

    public void PressStrengthButton()
    {
        if (money >= priceStrength)
        {
            money -= priceStrength;
            lvStrength += indexLvStrengthValue;
            priceStrength += indexPriceStrengthModificator;
            valueStrength += valueStrengthModificator;
            UpdateStrengthButton();
        }
    }

    public void PressSpeedButton()
    {
        if (money >= priceSpeed)
        {
            money -= priceSpeed;
            lvSpeed += indexLvSpeedValue;
            priceSpeed += indexPriceSpeedModificator;
            valueSpeed += valueSpeedModificator;
            UpdateSpeedButton();
        }
    }

    public void PressCountButton()
    {
        if (money >= priceCount)
        {
            money -= priceCount;
            lvCount += indexLvCountValue;
            priceCount += indexPriceCountModificator;
            valueCount += valueCountModificator;
            myPlanet.ButtonCreateChild(); // spawn one ship
            UpdateCountButton();
        }
    }

    private void ButtonsInteractionUpdate()
    {
        if(money >= priceStrength)
        {
            buttonStrength.interactable = true;
        }
        else
        {
            buttonStrength.interactable = false;
        }

        if (money >= priceSpeed)
        {
            buttonSpeed.interactable = true;
        }
        else
        {
            buttonSpeed.interactable = false;
        }

        if (money >= priceCount)
        {
            buttonCount.interactable = true;
        }
        else
        {
            buttonCount.interactable = false;
        }
    }

    public void PressStartGameButton(Transform buttonPressed)
    {
        gameStarted = true;
        buttonPressed.gameObject.SetActive(false);
    }

    private IEnumerator SpawnStartShips()
    {
        for(int i =0;i < valueCount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            myPlanet.ButtonCreateChild();// spawn one ship
        }
    }
}
