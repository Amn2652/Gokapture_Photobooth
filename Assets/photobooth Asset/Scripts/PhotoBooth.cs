using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PhotoBooth : MonoBehaviour
{
    public RawImage cameraPreview;
    public Image savedPhotoDisplay;
    public Image overlayImage;
    private WebCamTexture webcamTexture;
    private Texture2D capturedPhoto;
    private string filePath;

    void Start()
    {
        webcamTexture = new WebCamTexture(720, 1080);
        cameraPreview.texture = webcamTexture;
        cameraPreview.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    public void CapturePhoto()
    {
        capturedPhoto = new Texture2D(webcamTexture.width, webcamTexture.height);
        capturedPhoto.SetPixels(webcamTexture.GetPixels());
        capturedPhoto.Apply();

        AddOverlayToPhoto();

        cameraPreview.texture = capturedPhoto;
        webcamTexture.Stop();
        SavePhoto();
    }

    public void AddOverlayToPhoto()
    {
        Texture2D overlayTexture = overlayImage.sprite.texture;
        Texture2D resizedOverlayTexture = new Texture2D(capturedPhoto.width, capturedPhoto.height);

        float overlayWidth = overlayTexture.width;
        float overlayHeight = overlayTexture.height;
        float scaleX = (float)capturedPhoto.width / overlayWidth;
        float scaleY = (float)capturedPhoto.height / overlayHeight;
        float scale = Mathf.Min(scaleX, scaleY);

        for (int y = 0; y < capturedPhoto.height; y++)
        {
            for (int x = 0; x < capturedPhoto.width; x++)
            {
                int overlayX = Mathf.FloorToInt(x / scale);
                int overlayY = Mathf.FloorToInt(y / scale);

                if (overlayX < overlayWidth && overlayY < overlayHeight)
                {
                    resizedOverlayTexture.SetPixel(x, y, overlayTexture.GetPixel(overlayX, overlayY));
                }
                else
                {
                    resizedOverlayTexture.SetPixel(x, y, Color.clear);
                }
            }
        }

        resizedOverlayTexture.Apply();

        Color[] photoPixels = capturedPhoto.GetPixels();
        Color[] overlayPixels = resizedOverlayTexture.GetPixels();

        for (int i = 0; i < photoPixels.Length; i++)
        {
            Color overlayPixel = overlayPixels[i];
            if (overlayPixel.a > 0)
            {

                photoPixels[i] = Color.Lerp(photoPixels[i], overlayPixel, overlayPixel.a);
            }
        }

        capturedPhoto.SetPixels(photoPixels);
        capturedPhoto.Apply();
        Destroy(resizedOverlayTexture);
    }

    public async void SavePhoto()
    {
        if (capturedPhoto != null)
        {
            byte[] bytes = capturedPhoto.EncodeToPNG();
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";
            filePath = downloadsPath + "/Photo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";


            await File.WriteAllBytesAsync(filePath, bytes);
            ShowSavedPhoto();
        }
    }

    public void ShowSavedPhoto()
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D savedPhoto = new Texture2D(1, 1);
        savedPhoto.LoadImage(imageBytes);
        Sprite savedPhotoSprite = Sprite.Create(savedPhoto, new Rect(0, 0, savedPhoto.width, savedPhoto.height), new Vector2(0.5f, 0.5f));
        savedPhotoDisplay.sprite = savedPhotoSprite;
    }


    public void ExitApp()
    {
        Application.Quit();
    }
}