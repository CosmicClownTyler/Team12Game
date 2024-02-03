using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles UI Interactions. 
public class UIHandler : MonoBehaviour
{
    // The dropdown to choose between either 2D (noise) or 3D (terrain) generation. 
    public TMP_Dropdown generationDropdown;
    // This is equal to the choice selected in the dropdown menu before a new one is selected. 
    // It is used to determine which animation to call for the camera, because a before and after is required. 
    private int previousGenerationChoice;

    // The dropdown to choose the type of 2D (noise) generation. 
    public TMP_Dropdown noiseDropdown;

    // The dropdown to choose the type of 3D (terrain) generation. 
    public TMP_Dropdown terrainDropdown;

    // The text labelling the noise and terrain dropdowns. 
    public TMP_Text noiseTerrainText;

    // The input field for the seed. 
    public TMP_InputField seedInput;

    // The Noise object to generate the various types of noise. 
    //public Noise noiseObject;

    // The main (and only) camera. 
    public Camera mainCamera;

    // The animator for the camera. 
    private Animator cameraAnimator;

    // Start is called before the first frame update. 
    private void Start()
    {
        // Add listeners to each dropdown. 
        generationDropdown.onValueChanged.AddListener(delegate { GenTypeChange(); });
        noiseDropdown.onValueChanged.AddListener(delegate { NoiseTypeChange(); });
        terrainDropdown.onValueChanged.AddListener(delegate { TerrainTypeChange(); });

        // Add a listener to the sliders. 
        seedInput.onValueChanged.AddListener(delegate { SeedChange(); });

        // Get the camera's animator. 
        cameraAnimator = mainCamera.GetComponent<Animator>();

        // Set the previous generation choice to 0 as that is the default selection. 
        previousGenerationChoice = 0;
    }

    // The listener for when the generation type changes from the dropdown. 
    private void GenTypeChange()
    {
        // If the currently selected choice is 0 AKA 'None'. 
        if (generationDropdown.value == 0)
        {
            // Set the text for the Noise/Terrain Text. 
            noiseTerrainText.text = "";

            // Turn off both the noise generation and terrain generation dropdowns. 
            noiseDropdown.gameObject.SetActive(false);
            terrainDropdown.gameObject.SetActive(false);

            // Switch through the previous choice. 
            switch (previousGenerationChoice)
            {
                // If the previous choice was 0, stay at 0. 
                case 0:
                    cameraAnimator.SetTrigger("Stay0");
                    break;
                // If the previous choice was 1, move from 1 to 0. 
                case 1:
                    cameraAnimator.SetTrigger("From1To0");
                    break;
                // If the previous choice was 2, move from 2 to 0. 
                case 2:
                    cameraAnimator.SetTrigger("From2To0");
                    break;
            }
        }
        // Else if the currently selected choice is 1, AKA '2D'. 
        else if (generationDropdown.value == 1)
        {
            // Set the text for the Noise/Terrain Text. 
            noiseTerrainText.text = "Noise: ";

            // Turn on the noise generation dropdown and turn off the terrain generation dropdown. 
            noiseDropdown.gameObject.SetActive(true);
            terrainDropdown.gameObject.SetActive(false);

            // Switch through the previous choice. 
            switch (previousGenerationChoice)
            {
                // If the previous choice was 0, move from 0 to 1. 
                case 0:
                    cameraAnimator.SetTrigger("From0To1");
                    break;
                // If the previous choice was 1, stay at 1. 
                case 1:
                    cameraAnimator.SetTrigger("Stay1");
                    break;
                // If the previous choice was 2, move from 2 to 1. 
                case 2:
                    cameraAnimator.SetTrigger("From2To1");
                    break;
            }
        }
        // Else if the currently selected choice is 2, AKA '3D'. 
        else if (generationDropdown.value == 2)
        {
            // Set the text for the Noise/Terrain Text. 
            noiseTerrainText.text = "Terrain: ";

            // Turn on the terrain generation dropdown and turn off the noise generation dropdown. 
            noiseDropdown.gameObject.SetActive(false);
            terrainDropdown.gameObject.SetActive(true);

            // Switch through the previous choice. 
            switch (previousGenerationChoice)
            {
                // If the previous choice was 0, move from 0 to 2. 
                case 0:
                    cameraAnimator.SetTrigger("From0To2");
                    break;
                // If the previous choice was 1, move from 1 to 2. 
                case 1:
                    cameraAnimator.SetTrigger("From1To2");
                    break;
                // If the previous choice was 2, stay at 2. 
                case 2:
                    cameraAnimator.SetTrigger("Stay2");
                    break;
            }
        }

        // Set the previous choice to the current choice, so that next time the current choice changes,
        // the previous choice is accurate. 
        previousGenerationChoice = generationDropdown.value;
    }

    // The listener for when the noise type changes. 
    private void NoiseTypeChange()
    {
        // The noise type chosen through the dropdown. 
        //Noise.NoiseType noiseType;

        // Choose the noise type based on the value chosen from the dropdown. 
        switch (noiseDropdown.value)
        {
            case 1:
                //noiseType = Noise.NoiseType.Perlin;
                break;
            case 2:
                //noiseType = Noise.NoiseType.VoronoiDiagram;
                break;
            case 0:
            default:
                //noiseType = Noise.NoiseType.None;
                break;
        }

        // Set the noise choice in the noise object. 
        //noiseObject.NoiseChoice = noiseType;
    }

    // The listener for when the terrain type changes. 
    private void TerrainTypeChange()
    {

    }

    // The listener for when the seed changes. 
    private void SeedChange()
    {
        try
        {
            //noiseObject.Seed = float.Parse(seedInput.text);
        }
        catch (FormatException e)
        {
            Debug.Log("The seed must be a valid number. \n" + e.StackTrace);
        }
    }

}