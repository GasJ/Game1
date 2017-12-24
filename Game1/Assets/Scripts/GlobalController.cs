using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalController : MonoBehaviour {

    private bool _gameBlocked = false;
    private bool _areAxisesTriggered = false;
    private bool _isCameraMoving = false;
    private float _cameraMovingSpeed = 0f;
    private Vector3 _cameraMovingTargetPosition;
    private Utility.CharacterType _characterOnTurn = Utility.CharacterType.Player;
    private bool _isTurnStarted = false;
    private GameObject _waitingAIDialog;
    private Dictionary<Character, bool> _aiMoveCheckList = new Dictionary<Character, bool>();

    public GameObject CanvasGameObject;
    public GameObject PlayerGameObject;
    public List<GameObject> AIGameObjectList;
    public GameObject MainCameraGameObject;
    public GameObject AdjustCameraGameObject;

    public UnityAction leftButtonDownAction;
    public UnityAction rightButtonDownAction;
    public UnityAction upButtonDownAction;
    public UnityAction downButtonDownAction;
    public UnityAction cancelButtonUpAction;
    public UnityAction mouseLeftUpAction;

    /// <summary>
    /// The current global controller object
    /// </summary>
    public static GlobalController CurrentGlobalController { get; private set; } 
    /// <summary>
    /// The current canvas object to put GUI elements in
    /// </summary>
    public static GameObject CurrentCanvasGameObject { get; private set; }
    /// <summary>
    /// The current player game object
    /// </summary>
    public static GameObject CurrentPlayerGameObject { get; private set; }
    /// <summary>
    /// The current player object
    /// </summary>
    public static Character CurrentPlayer { get { return CurrentPlayerGameObject.GetComponent<Character>(); } }
    /// <summary>
    /// The list of all AI game objects
    /// </summary>
    public static List<GameObject> CurrentAIGameObjectList { get; private set; }
    /// <summary>
    /// The list of all AI objects
    /// </summary>
    public static List<Character> CurrentAIList
    {
        get
        {
            var list = new List<Character>();
            foreach(var a in CurrentAIGameObjectList)
            {
                list.Add(a.GetComponent<AI>());
            }
            return list;
        }
    }
    /// <summary>
    /// The current main camera game object
    /// </summary>
    public static GameObject CurrentMainCameraGameObject { get; private set; }
    /// <summary>
    /// Indicate if a dialog is showing
    /// </summary>
    public static bool IsShowingDialog { get; set; }
    /// <summary>
    /// Indicate if a pause menu is showing
    /// </summary>
    public static bool IsShowingPauseMenu { get; set; }
    /// <summary>
    /// The dictionary of all registered inteactive objects
    /// </summary>
    public static Dictionary<string, InteractiveObject> InteractiveObjectDictionary { get; private set; }
    public static GameEventHolder GlobalGameEventHolder { get; private set; }

    void Awake()
    {
        CurrentGlobalController = this;
        CurrentCanvasGameObject = CanvasGameObject;
        CurrentPlayerGameObject = PlayerGameObject;
        CurrentAIGameObjectList = AIGameObjectList;
        CurrentMainCameraGameObject = MainCameraGameObject;
        InteractiveObjectDictionary = new Dictionary<string, InteractiveObject>();
        GlobalGameEventHolder = new GameEventHolder();

        cancelButtonUpAction += showPauseMenu;
    }

    // Use this for initialization
    void Start () {
        _gameBlocked = true;
		UnityAction showTutorial4 = () => 
		{
			Utility.ShowDialog("TRY TO ESCAPE FROM THE HOUSE NOW!!\n (Don't get caught by your parents!!)", () => { _gameBlocked = false; });
		};
        UnityAction showTutorial3 = () =>
        {
			Utility.ShowDialog("This is a turn-based game, so one step per turn!", showTutorial4);
		};
        UnityAction showTutorial2 = () =>
        {
			Utility.ShowDialog("Click nearby items to see what happen! \n (you can use direction keys too!)", showTutorial3);    
        };
        Utility.ShowDialog(
			"Click nearby waypoints to move your step!\n (you can use direction keys as well!)", showTutorial2
        );

	}
	
	// Update is called once per frame
	void Update () {
        if (!_gameBlocked)
        {
            handleCharacterTurns();
            handleInputs();
            handleCameraMoving();
        }
	}

    /// <summary>
    /// Notify the global controller that a character finishes its move
    /// </summary>
    /// <param name="actionController">Action controller of the character that finishes its move</param>
    public static void NotifyTurnFinished(ActionController actionController)
    {
        switch (CurrentGlobalController._characterOnTurn)
        {
            case Utility.CharacterType.Player:
                CurrentGlobalController._isTurnStarted = false;
                CurrentGlobalController._characterOnTurn = Utility.CharacterType.AI;
                break;
            case Utility.CharacterType.AI:
                CurrentGlobalController._aiMoveCheckList[actionController.MyCharacter] = true;
                if (!CurrentGlobalController._aiMoveCheckList.ContainsValue(false))
                {
                    CurrentGlobalController._isTurnStarted = false;
                    CurrentGlobalController._characterOnTurn = Utility.CharacterType.Player;
                }
                break;
        }
    }

    /// <summary>
    /// Move main camera to a game object
    /// </summary>
    /// <param name="targetObject">The game object that camera moves to</param>
    public void moveCameraTo(GameObject targetObject)
    {
        var targetPosition = targetObject.transform.position;
        CurrentMainCameraGameObject.transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
    }

    /// <summary>
    /// Move main camera to a game object with a constant speed
    /// </summary>
    /// <param name="targetObject">The game object that camera moves to</param>
    /// <param name="speed">The moving speed</param>
    public void moveCameraTo(GameObject targetObject, float speed)
    {
        var targetPosition = targetObject.transform.position;
        _cameraMovingTargetPosition = new Vector3(targetPosition.x, targetPosition.y, -10);

        _isCameraMoving = true;
    }

    private void handleCharacterTurns()
    {
        if (!_isTurnStarted)
        {
            switch (_characterOnTurn)
            {
                case Utility.CharacterType.Player:
                    // Todo this is a temporary camera moving solution
					//Destroy(_waitingAIDialog);
                    IsShowingDialog = false;
                    _isTurnStarted = true;
                    CurrentPlayer.MyActionController.startTurn();

					moveCameraTo(CurrentPlayerGameObject);
                    MainCameraGameObject.GetComponent<Camera>().orthographicSize = 5;
                    break;
                case Utility.CharacterType.AI:
                    // Todo write actually functional code here
                    _isTurnStarted = true;

                    //_waitingAIDialog = Utility.ShowDialog("Waiting for AIs to finish move...");

                    moveCameraTo(AdjustCameraGameObject);
                    MainCameraGameObject.GetComponent<Camera>().orthographicSize = 15;
                    foreach(var c in CurrentAIList)
                    {
                        _aiMoveCheckList[c] = false;
                        c.MyActionController.startTurn();
                    }
                    break;
            }
        }
    }

    private void handleCameraMoving()
    {
        if (_isCameraMoving)
        {
            var currentPosition = gameObject.transform.position;
            if (Vector3.Distance(currentPosition, _cameraMovingTargetPosition) >= 0.1f)
            {
                gameObject.transform.position
                    = Vector3.MoveTowards(currentPosition, _cameraMovingTargetPosition, _cameraMovingSpeed * Time.deltaTime);
            }
            else
            {
                gameObject.transform.position = _cameraMovingTargetPosition;
                _isCameraMoving = false;
            }
        }
    }

    private void handleInputs()
    {
        // keyboard and joystick input
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            _areAxisesTriggered = false;
        }
        if (Input.GetAxis("Horizontal") < 0 && !_areAxisesTriggered)  // left down
        {
            _areAxisesTriggered = true;
            leftButtonDownAction();
        }
        if (Input.GetAxis("Horizontal") > 0 && !_areAxisesTriggered) // right down
        {
            _areAxisesTriggered = true;
            rightButtonDownAction();
        }
        if (Input.GetAxis("Vertical") > 0 && !_areAxisesTriggered) // up down
        {
            _areAxisesTriggered = true;
            upButtonDownAction();
        }
        if (Input.GetAxis("Vertical") < 0 && !_areAxisesTriggered) // down down
        {
            _areAxisesTriggered = true;
            downButtonDownAction();
        }
        if (Input.GetButtonUp("Cancel")) // cancel up
        {
            cancelButtonUpAction();
        }
        if (Input.GetMouseButtonUp(0)) // mouse left up
        {
            mouseLeftUpAction();
        }
    }

    private void showPauseMenu()
    {
        gameObject.GetComponent<AudioSource>().Play();
        Utility.ShowPauseMenu();
    }
}
