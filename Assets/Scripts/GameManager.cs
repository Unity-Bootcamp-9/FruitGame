using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public SwipeEventAsset SwipeEventAsset;
    public Button ResetButton;
    public ThrowFruit fruit;
    public Camera mainCamera;

    public Slider maxXSlider;
    public Slider minYSlider;

    public TextMeshProUGUI checkDropText;

    public TextMeshProUGUI Text;

    public TextMeshProUGUI direction;

    GameObject newFruit;

    // Start is called before the first frame update
    void Start()
    {
        newFruit = mainCamera.GetComponentInChildren<ThrowFruit>().gameObject;
        ResetButton.onClick.AddListener(() =>
        {
            newFruit = Instantiate(fruit.gameObject);
            newFruit.transform.SetParent(mainCamera.transform);
            newFruit.transform.localPosition = new Vector3(0, -0.02f, 0.2f);
            newFruit.transform.rotation = new Quaternion(0,0,0,0);

            newFruit.GetComponent<ThrowFruit>()._maxXThreshold = maxXSlider.value;
            newFruit.GetComponent<ThrowFruit>()._minYThreshold = minYSlider.value;

            newFruit.GetComponent<ThrowFruit>().gameManager = this;
        });

        maxXSlider.value = newFruit.GetComponent<ThrowFruit>()._maxXThreshold;
        minYSlider.value = newFruit.GetComponent<ThrowFruit>()._minYThreshold;

        maxXSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Swipe Max X Axis: {maxXSlider.value}";

        minYSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Swipe Min Y Axis: {minYSlider.value}";

        maxXSlider.onValueChanged.AddListener((float value) => 
        {
            newFruit = mainCamera.GetComponentInChildren<ThrowFruit>().gameObject;
            newFruit.GetComponent<ThrowFruit>()._maxXThreshold = value;
            maxXSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Swipe Max X Axis: {maxXSlider.value}";
        });

        minYSlider.onValueChanged.AddListener((float value) =>
        {
            newFruit = mainCamera.GetComponentInChildren<ThrowFruit>().gameObject;
            newFruit.GetComponent<ThrowFruit>()._minYThreshold = value;
            minYSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Swipe Min Y Axis: {minYSlider.value}";
        });

        SwipeEventAsset.eventRaised += (o, args) =>
        {
            StartCoroutine(Test());
        };
    }

    IEnumerator Test()
    {
        Text.text = "Swipe Raised";
        yield return new WaitForSeconds(2);
        Text.text = null;
    }
}
