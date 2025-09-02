using System.Collections.Generic;
using UnityEngine;
using TMPro;  // <- dodajemy namespace TextMeshPro

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer; // Obiekt w Canvasie (np. pusty GameObject)
    public GameObject textPrefab;    // Prefab z komponentem TMP_Text

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();

        if (Input.GetKeyDown(KeyCode.T))
        {
            Show("Testowy tekst", 30, Color.yellow, Camera.main.transform.position + Vector3.forward * 2, Vector3.up * 10f, 3f);
        }
    }

    public void Show(string msg, int fontSize, Color color, Vector3 positionIgnored, Vector3 motion, float duration)
    {
        // Ukryj wszystkie aktywne teksty, �eby nie nak�ada�y si� na siebie
        foreach (var ft in floatingTexts)
        {
            if (ft.active)
                ft.Hide();
        }

        FloatingText floatingText = GetFloatingText();
        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;

        // Sta�a pozycja ekranu (�rodek ekranu, 75% wysoko�ci)
        Vector2 fixedScreenPosition = new Vector2(Screen.width / 2f, Screen.height * 0.75f);

        RectTransform containerRect = textContainer.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,
            fixedScreenPosition,
            null,
            out Vector2 localPoint);

        RectTransform textRect = floatingText.go.GetComponent<RectTransform>();
        if (textRect != null)
        {
            textRect.anchoredPosition = localPoint;
        }
        else
        {
            Debug.LogError("Prefab textPrefab nie ma RectTransform!");
        }

        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }



    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform, false); // zachowaj skal� itp.

            txt.txt = txt.go.GetComponent<TMP_Text>();
            if (txt.txt == null)
                Debug.LogError("Prefab textPrefab nie ma komponentu TMP_Text!");

            floatingTexts.Add(txt);
        }

        return txt;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
