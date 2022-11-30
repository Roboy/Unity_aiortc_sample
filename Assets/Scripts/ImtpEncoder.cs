using System;
using UnityEngine;
using UnityEngine.UI;

public class ImtpEncoder : MonoBehaviour
{
    [SerializeField] private RawImage leftEye;
    [SerializeField] private RawImage rightEye;
    [SerializeField] private GameObject sphere;
    private Texture2D lastReceivedTexture;

    public void SetLastReceivedTexture(Texture lastReceivedTexture_)
    {
        if (lastReceivedTexture_ != null)
        {
            lastReceivedTexture = GetTexture(lastReceivedTexture_);
            Update();
        }
    }

    private Texture2D GetTexture(Texture mainTexture)
    {
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
 
        RenderTexture currentRT = RenderTexture.active;
 
        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);
 
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
 
        
        RenderTexture.active = currentRT;
        renderTexture.Release();
        return texture2D;
    }
    
    bool CompareColors(Color[] old_, Color[] new_)
    {
        bool same = true;
        for (int i = 0; i < old_.Length; i++)
        {
            if (old_[i] != new_[i])
            {
                same = false;
            }   
        }
        return same;
    }
    
    Texture2D GetEyeTexture2D(Texture2D originalTexture, string side)
    {
        var width = originalTexture.width / 2;
        var height = originalTexture.height;
        var rightEyeOffset = originalTexture.width / 2 - 1;
        var croppedTexture = new Texture2D(width, height);
        croppedTexture.SetPixels(originalTexture.GetPixels(side == "left" ? 0 : rightEyeOffset, 0, width, height)); 
        croppedTexture.Apply();
        return croppedTexture;
    }

    void RenderSphere(GameObject sphere, Texture2D texture2D)
    {
        Material runtimeMaterial = sphere.GetComponent<Renderer>().material;
        runtimeMaterial.mainTexture = texture2D;
        sphere.GetComponent<Renderer>().material = runtimeMaterial;
    }
    private void Update()
    {
        leftEye.texture = lastReceivedTexture;// GetEyeTexture2D(lastReceivedTexture, "left");
        rightEye.texture = lastReceivedTexture;//GetEyeTexture2D(lastReceivedTexture, "right");
        RenderSphere(sphere, GetEyeTexture2D(lastReceivedTexture, "right"));
    }
}