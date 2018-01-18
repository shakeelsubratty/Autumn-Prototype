using UnityEngine;
using System.Collections;

public class env_GameMenu : MonoBehaviour {

    public float MasterVolume = 1.0F;
    public float MusicVolume = 1.0F;
    public float EffectsVolume = 1.0F;
    public float VoiceoverVolume = 1.0F;
    public bool Subtitles = false;

    public bool showMenu = false;
    private bool isMenuOpen = false;
    public bool showOptions = false;
    private bool isOptionsOpen = false;
    public bool showAudio = false;
    private bool isAudioOpen = false;

    public Rect GameMenuRect = new Rect(550, 200, 200, 140);
    public Rect OptionsRect = new Rect(550, 200, 200, 180);
    public Rect AudioRect = new Rect(550, 200, 220, 300);

    void Start()
    {
        CenterRectangle(GameMenuRect);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (showMenu == false)
            {
                showMenu = true;
                Screen.lockCursor = false;
                Screen.showCursor = true;
                PauseGame(true);
            }
            else if (showMenu == true)
            {
                showMenu = false;
                showOptions = false;
                showAudio = false;
                Screen.lockCursor = true;
                Screen.showCursor = false;
                PauseGame(false);
            }
        }
        audio.volume = MasterVolume;

        AudioListener.volume = MusicVolume;
    }
    void OnGUI()
    {
        if (showMenu == true)
        {
            GUI.Window(1, GameMenuRect, GameMenuFunction, "Menu"); //WindowID 1 because FPS Counter
            isMenuOpen = true;
            isOptionsOpen = false;
            isAudioOpen = false;
        }
        else if(showOptions == true)
        {
            GUI.Window(1, OptionsRect, GameOptionsFunction, "Options");
            isMenuOpen = false;
            isOptionsOpen = true;
            isAudioOpen = false;
        }
        else if (showAudio == true)
        {
            GUI.Window(1, AudioRect, GameAudioFunction, "Audio");
            isMenuOpen = false;
            isOptionsOpen = false;
            isAudioOpen = true;
        }
        else if(showMenu == false && showOptions == false)
        {
            isMenuOpen = false;
            isOptionsOpen = false;
        }
    }
    void GameMenuFunction(int WindowID)
    {
        if(GUI.Button(new Rect(10, 20, 180, 30), "Return to Game"))
        {
            showMenu = false;
            Screen.lockCursor = true;
            Screen.showCursor = false;
            PauseGame(false);
        }

        if(GUI.Button(new Rect(10, 60, 180, 30), "Options"))
        {
            showMenu = false;
            showOptions = true;
        }

        if(GUI.Button(new Rect(10, 100, 180, 30), "Exit Game"))
        {
            Application.Quit();
        }
    }
    void GameOptionsFunction(int WindowID)
    {
        if (GUI.Button(new Rect(10, 20, 180, 30), "Graphics"))
        {

        }

        if (GUI.Button(new Rect(10, 60, 180, 30), "Audio"))
        {
            showOptions = false;
            showAudio = true;

        }
        if (GUI.Button(new Rect(10, 100, 180, 30), "Keybinding"))
        {

        }
        if (GUI.Button(new Rect(10, 140, 180, 30), "Return to Menu"))
        {
            showOptions = false;
            showMenu = true;
        }
    }
    void GameAudioFunction(int WindowID)
    {
        GUI.Label(new Rect(10, 25, 100, 20), "Master Volume");
        MasterVolume = GUI.HorizontalSlider(new Rect(10, 50, 200, 20), MasterVolume, 0.0F, 1.0F);
        GUI.Label(new Rect(10, 65, 100, 20), "Music Volume");
        MusicVolume = GUI.HorizontalSlider(new Rect(10, 90, 200, 20), MusicVolume, 0.0F, 1.0F);
        GUI.Label(new Rect(10, 105, 100, 20), "Effects Volume");
        EffectsVolume = GUI.HorizontalSlider(new Rect(10, 130, 200, 20), EffectsVolume, 0.0F, 1.0F);
        GUI.Label(new Rect(10, 145, 100, 20), "Voiceover Volume");
        VoiceoverVolume = GUI.HorizontalSlider(new Rect(10, 170, 200, 20), VoiceoverVolume, 0.0F, 1.0F);
        Subtitles = GUI.Toggle(new Rect(10, 195, 80, 20), Subtitles, "Subtitles");

        if (GUI.Button(new Rect(10, 260, 200, 20), "Return to Options"))
        {
            showAudio = false;
            showOptions = true;
            showMenu = false;
        }
    }
    void PauseGame(bool pause)
    {
        if(pause == true)
        {
            Time.timeScale = 0;
            GetComponent<TP_Camera>().enabled = false; // avoids fast moving camera after pausing 
        }
        else if(pause == false)
        {
            Time.timeScale = 1;
            GetComponent<TP_Camera>().enabled = true;
        }
        else
        {
            Time.timeScale = 0;
            GetComponent<TP_Camera>().enabled = false;
        }
    }
    Rect CenterRectangle(Rect SpecRect)
    {
        SpecRect.x = (Screen.width - SpecRect.width) / 2;
        SpecRect.y = (Screen.height - SpecRect.height) / 2;

        return SpecRect;
    }
}
