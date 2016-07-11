using UnityEngine;

public class Lobby : MonoBehaviour
{
    private void OnGUI()
    {
        GUI.Label(new Rect(new Vector2(Screen.width * 0.5f, Screen.height * 0.3f), new Vector2(200f, 100f)), "Welcom To Lobby");

        GUI.Button(new Rect(new Vector2(Screen.width * 0.4f, Screen.height * 0.5f), new Vector2(200f, 100f)), "Normal Game");
    }
}
