using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUIAnim : MonoBehaviour
{
    public Sprite[] images; // Tablica przechowuj�ca obrazy
    private int currentImageIndex = 0; // Indeks bie��cego obrazu
    private float elapsedTime = 0f;
    private float timePerFrame = 1f/15f; //1/liczba klatek na sekund�
    private Image displayImage; // Komponent Image, kt�ry b�dzie zmieniany

    void Start()
    {
        displayImage = GetComponent<Image>(); // Pobranie komponentu Image z tego samego obiektu
        UpdateImage(); // Wywo�anie funkcji aktualizuj�cej obrazek na pocz�tku
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
        // Sprawdzenie, czy obrazki oraz komponent Image s� dost�pne
        if (displayImage != null && images != null && images.Length > 0)
        {
            // Przypisanie nowego obrazka z tablicy na podstawie bie��cego indeksu
            displayImage.sprite = images[currentImageIndex];
        }
    }
}
