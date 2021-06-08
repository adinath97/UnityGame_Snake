using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int playerScore;
    [SerializeField] AudioClip click;
    [SerializeField] GameObject snake;
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject introText;
    [SerializeField] GameObject exitText;
    [SerializeField] GameObject endScoreText;
    [SerializeField] GameObject endHighScoreText;
    [SerializeField] GameObject continueText;
    [SerializeField] GameObject backgroundImage;
    [SerializeField] GameObject creditsText;
    [SerializeField] GameObject startFadeCover;
    [SerializeField] GameObject endFadeCover;
    public static bool startFadeOutRoutine, allowEndFade,commence, startGameNow, endGameNow, resetGame, resetComplete;
    private bool newHighScore;
    private AudioSource myAudioSource;
    private int counter;

    // Start is called before the first frame update
    void Awake()
    {
        newHighScore = false;
        endFadeCover.SetActive(false);
        startFadeCover.SetActive(true);
        StartCoroutine(TurnStartFadeOff());
        myAudioSource = this.GetComponent<AudioSource>();
        counter = 0;
        backgroundImage.SetActive(true);
        creditsText.SetActive(true);
        titleText.SetActive(true);
        exitText.SetActive(false);
        endScoreText.SetActive(false);
        endHighScoreText.SetActive(false);
        continueText.SetActive(false);
        exitText.GetComponent<Text>().text = "WELL PLAYED!";
        startFadeOutRoutine = false;
        allowEndFade = false;
        commence = false;
        startGameNow = false;
        endGameNow = false;
        resetGame = false;
        introText.SetActive(true);
        playerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowEndFade) {
            allowEndFade = false;
            StartCoroutine(EndFade());
        }
        if(Input.anyKey && !commence) {
            commence = true;
            myAudioSource.PlayOneShot(click);
        }
        if(resetGame) {
            Debug.Log("HELLO THERE!");
            counter = 0;
            commence = false;
            resetGame = false;
            //backgroundImage.SetActive(true);
            creditsText.SetActive(false);
            titleText.SetActive(false);
            exitText.SetActive(false);
            endScoreText.SetActive(false);
            endHighScoreText.SetActive(false);
            continueText.SetActive(false);
            exitText.GetComponent<Text>().text = "WELL PLAYED!";
            endGameNow = false;
            //introText.SetActive(true);
            playerScore = 0;
        }
        if(startGameNow) {
            startGameNow = false;
            introText.SetActive(false);
            backgroundImage.SetActive(false);
            creditsText.SetActive(false);
        }
        scoreText.GetComponent<Text>().text = playerScore.ToString();
        if(playerScore > PlayerPrefs.GetInt("HighScore",0)) {
            newHighScore = true;
            PlayerPrefs.SetInt("HighScore",playerScore);
        }
        if(playerScore >= 500 && counter == 0) {
            counter++;
            resetComplete = false;
        }
        if(playerScore >= 500 && !endGameNow && !resetComplete) {
            DeleteAllApples();
            startFadeCover.SetActive(true);
            StartCoroutine(TurnStartFadeOff());
            resetComplete = true;
            snake.GetComponent<Snake>().startGame = false;
            if(newHighScore) {
                exitText.GetComponent<Text>().text = "NEW HIGH SCORE ... + YOU WIN!";
            }
            else {
                exitText.GetComponent<Text>().text = "AWESOME PLAY!";
            }
            endScoreText.GetComponent<Text>().text = "SCORE: " + playerScore.ToString();
            endHighScoreText.GetComponent<Text>().text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore",0).ToString();
            startGameNow = false;
            endGameNow = false;
            backgroundImage.SetActive(true);
            titleText.SetActive(true);
            exitText.SetActive(true);
            endScoreText.SetActive(true);
            endHighScoreText.SetActive(true);
            continueText.SetActive(true);
            Debug.Log("HELLO 1");
            startFadeCover.GetComponent<Animator>().Play("StartFadeAnim");
            snake.GetComponent<Snake>().SetUpGame();
        }
        if(playerScore >= 500 && endGameNow && !resetComplete) {
            DeleteAllApples();
            startFadeCover.SetActive(true);
            StartCoroutine(TurnStartFadeOff());
            resetComplete = true;
            endGameNow = false;
            snake.GetComponent<Snake>().startGame = false;
            if(newHighScore) {
                exitText.GetComponent<Text>().text = "NEW HIGH SCORE ... + YOU WIN!";
            }
            else {
                exitText.GetComponent<Text>().text = "AWESOME PLAY!";
            }
            endScoreText.GetComponent<Text>().text = "SCORE: " + playerScore.ToString();
            endHighScoreText.GetComponent<Text>().text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore",0).ToString();
            startGameNow = false;
            snake.GetComponent<Snake>().startGame = false;
            backgroundImage.SetActive(true);
            titleText.SetActive(true);
            exitText.SetActive(true);
            endScoreText.SetActive(true);
            endHighScoreText.SetActive(true);
            continueText.SetActive(true);
            Debug.Log("HELLO 2");
            snake.GetComponent<Snake>().SetUpGame();
        }
        if(playerScore < 500 && endGameNow) {
            DeleteAllApples();
            startFadeCover.SetActive(true);
            StartCoroutine(TurnStartFadeOff());
            endGameNow = false;
            snake.GetComponent<Snake>().startGame = false;
            if(newHighScore) {
                newHighScore = false;
                exitText.GetComponent<Text>().text = "NEW HIGH SCORE! YAY!";
            }
            else {
                exitText.GetComponent<Text>().text = "AWESOME PLAY!";
            }
            endScoreText.GetComponent<Text>().text = "SCORE: " + playerScore.ToString();
            endHighScoreText.GetComponent<Text>().text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore",0).ToString();
            backgroundImage.SetActive(true);
            titleText.SetActive(true);
            exitText.SetActive(true);
            endScoreText.SetActive(true);
            endHighScoreText.SetActive(true);
            continueText.SetActive(true);
        }
    }

    private void DeleteAllApples() {
        GameObject[] foodItems = GameObject.FindGameObjectsWithTag("Food");
        foreach(GameObject foodItem in foodItems) {
            Destroy(foodItem);
        }
    }
    private IEnumerator TurnStartFadeOff() {
        yield return new WaitForSeconds(2f);
        startFadeCover.SetActive(false);
    }

    public IEnumerator EndFade() {
        //Debug.Log("HIIII");
        endFadeCover.SetActive(true);
        yield return new WaitForSeconds(2f);
        resetGame = true;
        startGameNow = true;
        snake.GetComponent<Snake>().startGame = true;
        yield return new WaitForSeconds(.5f);
        endFadeCover.SetActive(false);
        FoodInstantiationManager.instantiateFood = true;
    }
}
