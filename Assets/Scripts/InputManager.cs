using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private bool isRebinding = false;
    private string currentRebindAction = "";

    public void StartRebind(string actionName)
    {
        isRebinding = true;
        currentRebindAction = actionName;
    }

    void Update()
    {
        if (!isRebinding) return;

        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    ApplyRebind(key);
                    break;
                }
            }
        }
    }

    private void ApplyRebind(KeyCode newKey)
    {
        switch (currentRebindAction)
        {
            case "Fire":
                fireKey = newKey;
                break;

            case "Up":
                upKey = newKey;
                break;

            case "Down":
                downKey = newKey;
                break;

            case "Left":
                leftKey = newKey;
                break;

            case "Right":
                rightKey = newKey;
                break;
        }

        isRebinding = false;
        currentRebindAction = "";

        SaveBindings();
    }

    // =========================
    // KEYBINDINGS (Gameplay)
    // =========================
    [Header("Keybindings")]
    public KeyCode fireKey = KeyCode.Space;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    //public KeyCode bombKey = KeyCode.G;

    // =========================
    // PAUSE KEYS 
    // =========================
    [Header("Pause Keys")]
    public KeyCode[] pauseKeys = new KeyCode[]
    {
        KeyCode.Escape,
        KeyCode.P,
        KeyCode.F10
    };

    // =========================
    // SINGLETON SETUP
    // =========================
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBindings();
    }

    // =========================
    // INPUT QUERIES (READ ONLY)
    // =========================

    public bool FirePressed => Input.GetKeyDown(fireKey);

    public bool PausePressed => AnyKeyDown(pauseKeys);

    // public bool BombPressed => Input.GetKeyDown(bombKey);

    public Vector2 MoveInput
    {
        get
        {
            float x = 0f;
            float y = 0f;

            if (Input.GetKey(leftKey)) x -= 1f;
            if (Input.GetKey(rightKey)) x += 1f;
            if (Input.GetKey(upKey)) y += 1f;
            if (Input.GetKey(downKey)) y -= 1f;

            return new Vector2(x, y).normalized;
        }
    }

    // =========================
    // INPUT HELPERS
    // =========================

    private bool AnyKeyDown(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
                return true;
        }
        return false;
    }

    // =========================
    // SAVE / LOAD SYSTEM
    // =========================

    const string FIRE_KEY = "FireKey";
    const string UP_KEY = "UpKey";
    const string DOWN_KEY = "DownKey";
    const string LEFT_KEY = "LeftKey";
    const string RIGHT_KEY = "RightKey";
    //const string BOMB_KEY = "BombKey";

    public void SaveBindings()
    {
        PlayerPrefs.SetInt(FIRE_KEY, (int)fireKey);
        PlayerPrefs.SetInt(UP_KEY, (int)upKey);
        PlayerPrefs.SetInt(DOWN_KEY, (int)downKey);
        PlayerPrefs.SetInt(LEFT_KEY, (int)leftKey);
        PlayerPrefs.SetInt(RIGHT_KEY, (int)rightKey);
        //PlayerPrefs.SetInt(BOMB_KEY, (int)bombKey);

        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey(FIRE_KEY))
            fireKey = (KeyCode)PlayerPrefs.GetInt(FIRE_KEY);

        if (PlayerPrefs.HasKey(UP_KEY))
            upKey = (KeyCode)PlayerPrefs.GetInt(UP_KEY);

        if (PlayerPrefs.HasKey(DOWN_KEY))
            downKey = (KeyCode)PlayerPrefs.GetInt(DOWN_KEY);

        if (PlayerPrefs.HasKey(LEFT_KEY))
            leftKey = (KeyCode)PlayerPrefs.GetInt(LEFT_KEY);

        if (PlayerPrefs.HasKey(RIGHT_KEY))
            rightKey = (KeyCode)PlayerPrefs.GetInt(RIGHT_KEY);

        //if (PlayerPrefs.HasKey(BOMB_KEY))
        //    bombKey = (KeyCode)PlayerPrefs.GetInt(BOMB_KEY);
    }
}