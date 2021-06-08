using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] GameObject snakeBodyPartPrefab;
    [SerializeField] AudioClip eatSound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] float speed = .5f;
    [SerializeField] GameObject turnTopRightObject;
    [SerializeField] GameObject tailObject;
    [SerializeField] Vector2 instantiationPoint = new Vector2(-21,0);
    private List<Vector3> turningPoints = new List<Vector3>();
    private List<Vector2> directionChanges = new List<Vector2>();
    private List<GameObject> snakeBody = new List<GameObject>();
    private List<Quaternion> bodyPartsRotation = new List<Quaternion>();
    private Vector3 desiredScale;
    private Vector2 currentDirection = Vector2.right, previousDirection, lastTurnDirection;
    private int initialSize = 3, coroutinesRunning = 0, growSnakeCounter = 0, counter;
    private bool tailTurning, growSnake, turnRight, turnUp, turnLeft, turnDown, keepRotating, startAgain;
    public bool startGame, doneInitialRoutine;
    private AudioSource myAudioSource;
    private void Start() {
        doneInitialRoutine = false;
        myAudioSource = this.GetComponent<AudioSource>();
        desiredScale = new Vector3(.75f,1f,1f);
        SetUpGame();
        startAgain = true;
        directionChanges.Add(currentDirection);
    }

    public IEnumerator InitialRoutine() {
        yield return new WaitForSeconds(2f);
        doneInitialRoutine = true;
    }

    // Update is called once per frame
    private void Update() {
        if(Input.anyKey && !startGame && doneInitialRoutine) {
            doneInitialRoutine = false;
            Debug.Log("RESET GAME!");
            ScoreManager.commence = true;
            ScoreManager.allowEndFade = true;
            //ScoreManager.resetGame = true;
            //ScoreManager.startGameNow = true;
            //startGame = true;

        }
        //player input
        if(startGame) {
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
            if(currentDirection == Vector2.down) {
                return;
            }
            previousDirection = currentDirection;
            currentDirection = Vector2.up;
            turnUp = true;
            turningPoints.Add(new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0));
            directionChanges.Add(previousDirection);
            transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        }
            else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(currentDirection == Vector2.left) {
                return;
            }
            previousDirection = currentDirection;
            currentDirection = Vector2.right;
            turnRight = true;
            turningPoints.Add(new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0));
            directionChanges.Add(previousDirection);
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
            else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(currentDirection == Vector2.right) {
                return;
            }
            previousDirection = currentDirection;
            currentDirection = Vector2.left;
            turnLeft = true;
            turningPoints.Add(new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0));
            directionChanges.Add(previousDirection);
            transform.rotation = Quaternion.Euler(Vector3.forward * 180);
        }
            else if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if(currentDirection == Vector2.up) {
                return;
            }
            previousDirection = currentDirection;
            currentDirection = Vector2.down;
            turnDown = true;
            turningPoints.Add(new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0));
            directionChanges.Add(previousDirection);
            transform.rotation = Quaternion.Euler(Vector3.forward * -90);
        }
    
        }
        
    }

    private void FixedUpdate() {
        
        if(!startGame && counter < initialSize) {
            for(int i = snakeBody.Count - 1; i > 0; i--) {
                snakeBody[i].transform.position = snakeBody[i - 1].transform.position;
            /*
            if(currentDirection == Vector2.down) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            }
            if(currentDirection == Vector2.up) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }
            if(currentDirection == Vector2.left) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            }
            if(currentDirection == Vector2.right) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            */
            }
            this.transform.position = new Vector3(
                Mathf.RoundToInt(this.transform.position.x) + currentDirection.x,
                Mathf.RoundToInt(this.transform.position.y) + currentDirection.y,
                0f
            );
            counter++;
        }
        if(startGame) {
            //loop thru segments and shift each from back up one to propel snake forward
            for(int i = snakeBody.Count - 1; i > 0; i--) {
            snakeBody[i].transform.position = snakeBody[i - 1].transform.position;
            /*
            if(currentDirection == Vector2.down) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            }
            if(currentDirection == Vector2.up) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }
            if(currentDirection == Vector2.left) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            }
            if(currentDirection == Vector2.right) {
                snakeBody[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            */
        }
            
            if(turnUp && startAgain) {
            //Debug.Log("HELLO 1");
            //startAgain = false;
            turnUp = false;
            keepRotating = true;
            StartCoroutine(RotatePartsUp(snakeBody.Count,snakeBody.Count - 1,previousDirection,false));
        }

            if(turnRight && startAgain) {
            //Debug.Log("HELLO R");
            //startAgain = false;
            turnRight = false;
            keepRotating = true;
            StartCoroutine(RotatePartsRight(snakeBody.Count,snakeBody.Count - 1,previousDirection,false));
        }

            if(turnLeft && startAgain) {
            //startAgain = false;
            turnLeft = false;
            keepRotating = true;
            StartCoroutine(RotatePartsLeft(snakeBody.Count,snakeBody.Count - 1,previousDirection,false));
        }

            if(turnDown && startAgain) {
            //startAgain = false;
            turnDown = false;
            keepRotating = true;
            StartCoroutine(RotatePartsDown(snakeBody.Count,snakeBody.Count - 1,previousDirection,false));
        }
        
            //handle movement/physics.Rounding values to maintain node/grid based movement
            this.transform.position = new Vector3(
                Mathf.RoundToInt(this.transform.position.x) + currentDirection.x,
                Mathf.RoundToInt(this.transform.position.y) + currentDirection.y,
                0f
            );
        }
        
    }

    private IEnumerator RotatePartsLeft(int snakeBodyLength, int totalSnakeBodyLength, Vector2 previousDirection, bool entryGrowStatus) {
        bool entryGrowStatus1 = entryGrowStatus;
        Vector2 localPreviousDirection = previousDirection;
        Vector3 newScale = new Vector3(1,-1,1);
        Vector3 newScale2 = new Vector3(1,1,1);
        snakeBodyLength--;
        /*
        if(growSnake && !entryGrowStatus1) {
            entryGrowStatus1 = true;
            snakeBodyLength++;
            totalSnakeBodyLength++;
        }
        */
        if(startGame && keepRotating && (totalSnakeBodyLength - snakeBodyLength - 1) >= 0) {
            keepRotating = false;
            if(totalSnakeBodyLength - snakeBodyLength - 1 != 0) {
                snakeBody[totalSnakeBodyLength - snakeBodyLength - 1].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
            }
            //Debug.Log(totalSnakeBodyLength - snakeBodyLength);
            if(localPreviousDirection == Vector2.down) {
                if(snakeBodyLength > 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.forward * 180);
            }
            else if(localPreviousDirection == Vector2.up) {
                if(snakeBodyLength > 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale2;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.forward * 180);
            }
            if(tailTurning) {
                    tailTurning = false;
                }
        }
        yield return new WaitForFixedUpdate();
        if(startGame && snakeBodyLength > 0) {
            keepRotating = true;
            if(snakeBodyLength == 1) {
                tailTurning = true;
            }
            if(totalSnakeBodyLength < snakeBody.Count - 1) {
                StartCoroutine(RotatePartsLeft(snakeBodyLength + 1,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            else if(totalSnakeBodyLength == snakeBody.Count - 1) {
                StartCoroutine(RotatePartsLeft(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            //StartCoroutine(RotatePartsLeft(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
        }
    }

    private IEnumerator RotatePartsRight(int snakeBodyLength, int totalSnakeBodyLength, Vector2 previousDirection, bool entryGrowStatus) {
        bool entryGrowStatus1 = entryGrowStatus;
        Vector2 localPreviousDirection = previousDirection;
        Vector3 newScale = new Vector3(1,-1,1);
        Vector3 newScale2 = new Vector3(1,1,1);
        snakeBodyLength--;
        /*
        if(growSnake && !entryGrowStatus1) {
            entryGrowStatus1 = true;
            snakeBodyLength++;
            totalSnakeBodyLength++;
        }
        */
        if(startGame && keepRotating && (totalSnakeBodyLength - snakeBodyLength - 1) >= 0) {
                keepRotating = false;
                if(totalSnakeBodyLength - snakeBodyLength - 1 != 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength - 1].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
                }
                //Debug.Log(totalSnakeBodyLength - snakeBodyLength);
                if(localPreviousDirection == Vector2.up) {
                    if(snakeBodyLength > 0) {
                        snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                    }
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale;
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.zero);
                    bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.zero);
                }
                else if(localPreviousDirection == Vector2.down) {
                    if(snakeBodyLength > 0) {
                        snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                    }
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale2;
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.zero);
                    bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.zero);
                }
                if(tailTurning) {
                    tailTurning = false;
                }
                //snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                //snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        yield return new WaitForFixedUpdate();
        if(startGame && snakeBodyLength > 0) {
            if(snakeBodyLength == 1) {
                tailTurning = true;
            }
            keepRotating = true;
            if(totalSnakeBodyLength < snakeBody.Count - 1) {
                StartCoroutine(RotatePartsRight(snakeBodyLength + 1,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            else if(totalSnakeBodyLength == snakeBody.Count - 1) {
                StartCoroutine(RotatePartsRight(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            //StartCoroutine(RotatePartsRight(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
        }
        /*
        if(previousDirection == Vector2.up) {
            
        }
        else if(previousDirection == Vector2.down) {
            if(keepRotating && (totalSnakeBodyLength - snakeBodyLength - 1) >= 0) {
                keepRotating = false;
                if(totalSnakeBodyLength - snakeBodyLength - 1 != 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength - 1].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            yield return new WaitForFixedUpdate();
            if(snakeBodyLength > 0) {
                keepRotating = true;
                StartCoroutine(RotatePartsLeft(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection));
            }
        }
        */
        /*
        if(snakeBodyLength == 0) {
            startAgain = true;
        }
        */
    }

    private IEnumerator RotatePartsDown(int snakeBodyLength, int totalSnakeBodyLength, Vector2 previousDirection, bool entryGrowStatus) {
        bool entryGrowStatus1 = entryGrowStatus;
        Vector2 localPreviousDirection = previousDirection;
        Vector3 newScale = new Vector3(1,-1,1);
        Vector3 newScale2 = new Vector3(1,1,1);
        snakeBodyLength--;
        /*
        if(growSnake && !entryGrowStatus1) {
            entryGrowStatus1 = true;
            snakeBodyLength++;
            totalSnakeBodyLength++;
        }
        */
        if(startGame && keepRotating && (totalSnakeBodyLength - snakeBodyLength - 1) >= 0) {
            keepRotating = false;
            if(totalSnakeBodyLength - snakeBodyLength - 1 != 0) {
                snakeBody[totalSnakeBodyLength - snakeBodyLength - 1].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
            }
            //Debug.Log(totalSnakeBodyLength - snakeBodyLength);
            if(localPreviousDirection == Vector2.right) {
                if(snakeBodyLength > 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.forward * -90);
            }
            else if(localPreviousDirection == Vector2.left) {
                if(snakeBodyLength > 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale2;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.forward * -90);
            }
            if(tailTurning) {
                    tailTurning = false;
                }
        }
        yield return new WaitForFixedUpdate();
        if(startGame && snakeBodyLength > 0) {
            keepRotating = true;
            if(snakeBodyLength == 1) {
                tailTurning = true;
            }
            if(totalSnakeBodyLength < snakeBody.Count - 1) {
                StartCoroutine(RotatePartsDown(snakeBodyLength + 1,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            else if(totalSnakeBodyLength == snakeBody.Count - 1) {
                StartCoroutine(RotatePartsDown(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            //StartCoroutine(RotatePartsDown(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
        }
    }

    private IEnumerator RotatePartsUp(int snakeBodyLength, int totalSnakeBodyLength, Vector2 previousDirection, bool entryGrowStatus) {
        bool entryGrowStatus1 = entryGrowStatus;
        Vector2 localPreviousDirection = previousDirection;
        Vector3 newScale = new Vector3(1,1,1);
        Vector3 newScale2 = new Vector3(1,-1,1);
        snakeBodyLength--;
        /*
        if(growSnake && !entryGrowStatus1) {
            entryGrowStatus1 = true;
            snakeBodyLength++;
            totalSnakeBodyLength++;
        }
        */
        if(startGame && keepRotating && (totalSnakeBodyLength - snakeBodyLength - 1) >= 0) {
            keepRotating = false;
            if(totalSnakeBodyLength - snakeBodyLength - 1 != 0) {
                snakeBody[totalSnakeBodyLength - snakeBodyLength - 1].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
            }
            //Debug.Log(totalSnakeBodyLength - snakeBodyLength);
            if(localPreviousDirection == Vector2.left) {
                if(snakeBodyLength > 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale2;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.forward * 90);
            }
            if(localPreviousDirection == Vector2.right) {
                if(snakeBodyLength > 0) {
                    snakeBody[totalSnakeBodyLength - snakeBodyLength].GetComponent<SpriteRenderer>().sprite = turnTopRightObject.GetComponent<SpriteRenderer>().sprite;
                }
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.localScale = newScale;
                snakeBody[totalSnakeBodyLength - snakeBodyLength].transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                bodyPartsRotation[totalSnakeBodyLength - snakeBodyLength] = Quaternion.Euler(Vector3.forward * 90);
            }
            if(tailTurning) {
                    tailTurning = false;
                }
        }
         yield return new WaitForFixedUpdate();
        if(startGame && snakeBodyLength > 0) {
            keepRotating = true;
            if(snakeBodyLength == 1) {
                tailTurning = true;
            }
            if(totalSnakeBodyLength < snakeBody.Count - 1) {
                StartCoroutine(RotatePartsUp(snakeBodyLength + 1,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            else if(totalSnakeBodyLength == snakeBody.Count - 1) {
                StartCoroutine(RotatePartsUp(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
            }
            //StartCoroutine(RotatePartsUp(snakeBodyLength,snakeBody.Count - 1,localPreviousDirection,entryGrowStatus1));
        }
        /*
        if(snakeBodyLength == 0) {
            startAgain = true;
        }
        */
    }

    public void SetUpGame() {
        StartCoroutine(InitialRoutine());
        startGame = false;
        counter = 0;
        Debug.Log("COUNTER");
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
        currentDirection = Vector2.right;
        int count = snakeBody.Count;
        //Debug.Log(count);
        //ensure just snake head is present, if resetting
        for(int i = 1; i < count; i++) {
            //Debug.Log("HELLO " + i);
            Destroy(snakeBody[i].gameObject);
            //bodyPartsRotation.RemoveAt(i);
        }

        snakeBody.Clear();//erase reference to destroyed elements
        bodyPartsRotation.Clear(); //erase 
        snakeBody.Add(this.gameObject);
        bodyPartsRotation.Add(Quaternion.Euler(Vector3.zero));

        //add initial set of elements
        for(int i = 0; i < initialSize; i++) {
            if(i < initialSize - 1) {
                GameObject bodyPart = Instantiate(this.snakeBodyPartPrefab) as GameObject;
                //bodyPart.transform.localScale = desiredScale;
                //Debug.Log(bodyPart.transform.localScale);
                snakeBody.Add(bodyPart);
                bodyPartsRotation.Add(Quaternion.Euler(Vector3.zero));
            }
            if(i == initialSize - 1) {
                GameObject bodyPart = Instantiate(this.tailObject) as GameObject;
                //bodyPart.transform.localScale = desiredScale;
                //Debug.Log(bodyPart.transform.localScale);
                snakeBody.Add(bodyPart);
                bodyPartsRotation.Add(Quaternion.Euler(Vector3.zero));
            }
        }

        this.transform.position = instantiationPoint;
    } 
    
    private void Grow() {
       myAudioSource.PlayOneShot(eatSound);
       snakeBody[snakeBody.Count - 1].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
       GameObject bodyPartInstance =  Instantiate(this.tailObject);
       bodyPartInstance.GetComponent<SpriteRenderer>().enabled = false;
       //bodyPartInstance.GetComponent<SpriteRenderer>().sprite = tailObject.GetComponent<SpriteRenderer>().sprite;
       //bodyPartInstance.transform.localScale = desiredScale;
       //Debug.Log(bodyPartInstance.transform.localScale);
       bodyPartInstance.transform.rotation = bodyPartsRotation[bodyPartsRotation.Count - 1];
       bodyPartInstance.transform.position = snakeBody[snakeBody.Count - 1].transform.position;
       Quaternion tempRotation = bodyPartsRotation[bodyPartsRotation.Count - 1];
       bodyPartsRotation.Add(tempRotation);
       snakeBody.Add(bodyPartInstance);
       //Debug.Log(snakeBody.Count);
       growSnake = true;
       StartCoroutine(SmoothenSnake());
    }

    private IEnumerator SmoothenSnake() {
       yield return new WaitForEndOfFrame();
        do {
            snakeBody[snakeBody.Count - 1].transform.rotation = snakeBody[snakeBody.Count - 2].transform.rotation;
            if(Mathf.RoundToInt(snakeBody[snakeBody.Count - 1].transform.rotation.z) == Mathf.RoundToInt(snakeBody[snakeBody.Count - 2].transform.rotation.z)) {
                snakeBody[snakeBody.Count - 2].GetComponent<SpriteRenderer>().sprite = snakeBodyPartPrefab.GetComponent<SpriteRenderer>().sprite;
                snakeBody[snakeBody.Count - 1].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        while(Mathf.RoundToInt(snakeBody[snakeBody.Count - 1].transform.rotation.z) != Mathf.RoundToInt(snakeBody[snakeBody.Count - 2].transform.rotation.z));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Food") {
            Grow();
        }

        if(other.gameObject.tag == "Obstacle" || other.gameObject.tag == "turningPart") {
            myAudioSource.PlayOneShot(gameOverSound);
            ScoreManager.endGameNow = true;
            SetUpGame();
        }
    }
}
