using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject selectPanel;
    public GameObject capturePanel;
    public GameObject photoPanel;  // New panel to show the captured photo

    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject gameObject3;
    public GameObject gameObject3_1;
    public GameObject gameObject3_2;
    public GameObject gameObject3_3;

    public Button submitButton;
    public PhotoBooth photoBooth;  // Reference to PhotoBooth script

    private void Start()
    {
        selectPanel.SetActive(false);
        capturePanel.SetActive(false);
        photoPanel.SetActive(false);  // Hide photo panel initially

        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        startPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        startPanel.SetActive(false);

        selectPanel.SetActive(true);

        submitButton.onClick.AddListener(() =>
        {
            StartCoroutine(CaptureSequence());
        });
    }

    IEnumerator CaptureSequence()
    {
        selectPanel.SetActive(false);
        capturePanel.SetActive(true);

        gameObject1.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameObject1.SetActive(false);

        gameObject2.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject2.SetActive(false);

        gameObject3.SetActive(true);

        gameObject3_1.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject3_1.SetActive(false);

        gameObject3_2.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject3_2.SetActive(false);

        gameObject3_3.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject3_3.SetActive(false);

        gameObject3.SetActive(false);

        // Capture the photo
        photoBooth.CapturePhoto();

        // Show the next panel after photo is taken
        photoBooth.ShowSavedPhoto();
        capturePanel.SetActive(false);
        photoPanel.SetActive(true);
    }
}
