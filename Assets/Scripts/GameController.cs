using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    // bg image hold the bg image for each button initial image
    [SerializeField]
    private Sprite bgimage;

    // create a list of button to get button from the scene
    public List<Button> btns = new List<Button>();

    // array that will store all of the sticks image from the Resource/Sprites/all folder
    public Sprite[] sticks;

    // a list that will store the sticks image for the game
    public List<Sprite> gameSticks;

    private bool firstGuess, secondGuess;

    private int countGuesses, countCorrectGuesses, gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessStick, secondGuessStick;

  
    void Start()
    {
        GetButton();
        AddListener();
        AddGameSticks();
        Shuffle(gameSticks);
        gameGuesses = btns.Count / 2;

    }

    void Awake()
    {
        sticks = Resources.LoadAll<Sprite>("Sprites/all");
    }

    void GetButton()
    {
        // store created button list in the array objects
        GameObject[] objects = GameObject.FindGameObjectsWithTag("StickButton");

        for(int i = 0; i < objects.Length; i++)
        {
            // add new created button into array objects & to the scene
            btns.Add(objects[i].GetComponent<Button>());
            // add button bg image to every button created
            btns[i].image.sprite = bgimage;
        }
    }
    
    void AddListener()
    {
        // for each created button we add a listener that will inform us that we clicking that button
        // need to do it this way because we created the button upon start
        foreach(Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle()); 
        }
    }

    void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        // prompt us when we clicking a button in the scene
        Debug.Log("You are clicking a puzzle button " + name);

        if (!firstGuess)
        {
            firstGuess = true;

            firstGuessIndex = int.Parse(name);

            firstGuessStick = gameSticks[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gameSticks[firstGuessIndex];
        }

        else if (!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(name);

            secondGuessStick = gameSticks[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gameSticks[secondGuessIndex];

            if (firstGuessStick == secondGuessStick)
            {
                Debug.Log("The puzzle match");
            }

            else
            {
                Debug.Log("The puzzle dont match");
            }

            countGuesses++;

            StartCoroutine(CheckIfStickMatch());
        }
    }

    void AddGameSticks()
    {
        int looper = btns.Count;
        int index = 0;

        for(int i = 0; i < looper; i++)
        {
            // check if the game puzzles have 2 same sticks image
            // need to have the 1 same puzzle 2 times
            if (index == looper / 2)
            {
                index = 0;
            }

            gameSticks.Add(sticks[index]);
            index++;
        }
    }

    IEnumerator CheckIfStickMatch()
    {
        yield return new WaitForSeconds(.5f);

        if(firstGuessStick == secondGuessStick)
        {
            // need to wait for .5f before set the match stick 
            yield return new WaitForSeconds(.5f);

            // if the 2 stick is matched, the puzzle button cannot be click
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            // set the match stick image to transparent @ not set to Color(0,0,0,0) the matched stick will stay there
             btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
             btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckIfGameFinished();
        }

        else
        {
            // if the stick dont match, it will wait for .5f sec before turn back to bgimage
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].image.sprite = bgimage;
            btns[secondGuessIndex].image.sprite = bgimage;
        }

        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;
    }

    void CheckIfGameFinished()
    {
        countCorrectGuesses++;

        // if true shows the game is finished
        if(countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game finished.");
            Debug.Log("You finished the game with " + countGuesses + " guesses.");
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);

            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
