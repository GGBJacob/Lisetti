using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUIAnim : MonoBehaviour
{
    public Sprite[] images; // Tablica przechowuj¹ca obrazy
    private int currentImageIndex = 0; // Indeks bie¿¹cego obrazu
    private float elapsedTime = 0f;
    private float timePerFrame = 1f/15f; //1/liczba klatek na sekundê
    private Image displayImage; // Komponent Image, który bêdzie zmieniany

    void Start()
    {
        displayImage = GetComponent<Image>(); // Pobranie komponentu Image z tego samego obiektu
        UpdateImage(); // Wywo³anie funkcji aktualizuj¹cej obrazek na pocz¹tku
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timePerFrame)
        {
            elapsedTime = 0f;
            if (images != null && images.Length > 0)
            {
                currentImageIndex = (currentImageIndex + 1) % images.Length;
                UpdateImage();
            }
        }
    }

    void UpdateImage()
    {
        // Sprawdzenie, czy obrazki oraz komponent Image s¹ dostêpne
        if (displayImage != null && images != null && images.Length > 0)
        {
            // Przypisanie nowego obrazka z tablicy na podstawie bie¿¹cego indeksu
            displayImage.sprite = images[currentImageIndex];
        }
    }
}
