using System.Collections;
using UnityEngine;
using Unity.Barracuda;
using System;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Prediction : MonoBehaviour
{
    public Texture2D texture;
    public NNModel modelAsset;
    private Model _runtimeModel;
    private IWorker _engine;

    public Text textResult;

    public static float x0;     //À§Ä¡ÁÂÇ¥1
    public static float x1;     //À§Ä¡ÁÂÇ¥2
    public static float x2;     //À§Ä¡ÁÂÇ¥3
    public static float x3;     //À§Ä¡ÁÂÇ¥4
    public static float x4;     //Á¸ÀçÈ®·ü

    public static float fish01;     //°¥Ä¡
    public static float fish02;     //°¨¼ºµ¼
    public static float fish03;     //ºÎ½Ã¸®
    public static float fish04;     //¼þ¾î
    public static float fish05;     //¿ì·°
    public static float fish06;     //ÀÓ¿¬¼ö
    public static float fish07;     //Âüµ¼
    public static float fish08;     //ÇÐ²ÇÄ¡
    public static float fish09;     //°íµî¾î
    public static float fish10;     //±¤¾î
    public static float fish11;     //³ó¾î
    public static float fish12;     //°¡ÀÚ¹Ì
    public static float fish13;     //¸ÁµÕ¾î
    public static float fish14;     //¹Î¾î
    public static float fish15;     //º¬¿¡µ¼
    public static float fish16;     //º¼¶ô

    // °¥Ä¡, °¨¼ºµ¼, ºÎ½Ã¸®, ¼þ¾î, ¿ì·°, ÀÓ¿¬¼ö, Âüµ¼, ÇÐ²ÇÄ¡, °íµî¾î, ±¤¾î, ³ó¾î, °¡ÀÚ¹Ì, ¸ÁµÕ¾î, ¹Î¾î, º¬¿¡µ¼, º¼¶ô

    [HideInInspector] public int classNum;

    // public AudioSource SoundCamera;

    void Start()
    {
        _runtimeModel = ModelLoader.Load(modelAsset);

        try
        {
            _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.CPU);
            print($"{WorkerFactory.Device.CPU}");
        }
        catch (System.Exception)
        {
            print("Engine Out");
            _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.GPU);
        }
    }

    // °¡·Î, ¼¼·Î °ª ÁöÁ¤ÇØ¼­ Resize ÇÏ±â
    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        return result;
    }

    public void GETS()
    {
        StartCoroutine(Gettt());
        // SoundCamera.Play();
    }

    IEnumerator Gettt()
    {
        print("GETS!");

        try
        {
            texture = GetTextureFromCamera(Camera.main);

            try
            {
                texture = ScaleTexture(texture, 640, 640);
                // print(texture);
                print("Succes");
            }
            catch (Exception ex)
            {
                print("texture ERROR");
                print(ex);
            }
        }
        catch (Exception)
        {
            print("Fail");
        }

        var inputX = new Tensor(texture, 3);
        Tensor OutputY = _engine.Execute(inputX).PeekOutput();
        inputX.Dispose();


        float temp0 = 0;
        int indexofob = -1;
        int indexofob1 = -1;
        int indexofob2 = -1;

        for (int i = 0; i < 25200; i++)
        {
            float temp1 = (float)OutputY[0, 0, 4, i];
            if (temp0 < temp1)
            {
                temp0 = temp1;

                indexofob2 = indexofob1;    //ÀüÀü Á¸ÀçÈ®·üÀÇ ÃÖ´ñ°ªÀÇ ÀÎµ¦½º ÀúÀå
                indexofob1 = indexofob;     // Á÷Àü Á¸ÀçÈ®·üÀÇ ÃÖ´ñ°ªÀÇ ÀÎµ¦½º ÀúÀå
                indexofob = i;              //ÇöÀç Á¸ÀçÈ®·üÀÇ ÃÖ´ñ°ªÀÇ ÀÎµ¦½º ÀúÀå
            }
        }

        x0 = OutputY[0, 0, 0, indexofob];
        x1 = OutputY[0, 0, 1, indexofob];
        x2 = OutputY[0, 0, 2, indexofob];
        x3 = OutputY[0, 0, 3, indexofob];

        //NMS ±¸Çö
        x4 = (OutputY[0, 0, 4, indexofob] + OutputY[0, 0, 4, indexofob1] + OutputY[0, 0, 4, indexofob2]) / 3;

        fish01 = (OutputY[0, 0, 5, indexofob] + OutputY[0, 0, 5, indexofob1] + OutputY[0, 0, 5, indexofob2]) / 3;
        fish02 = (OutputY[0, 0, 6, indexofob] + OutputY[0, 0, 6, indexofob1] + OutputY[0, 0, 6, indexofob2]) / 3;
        fish03 = (OutputY[0, 0, 7, indexofob] + OutputY[0, 0, 7, indexofob1] + OutputY[0, 0, 7, indexofob2]) / 3;
        fish04 = (OutputY[0, 0, 8, indexofob] + OutputY[0, 0, 8, indexofob1] + OutputY[0, 0, 8, indexofob2]) / 3;
        fish05 = (OutputY[0, 0, 9, indexofob] + OutputY[0, 0, 9, indexofob1] + OutputY[0, 0, 9, indexofob2]) / 3;
        fish06 = (OutputY[0, 0, 10, indexofob] + OutputY[0, 0, 10, indexofob1] + OutputY[0, 0, 10, indexofob2]) / 3;
        fish07 = (OutputY[0, 0, 11, indexofob] + OutputY[0, 0, 11, indexofob1] + OutputY[0, 0, 11, indexofob2]) / 3;
        fish08 = (OutputY[0, 0, 12, indexofob] + OutputY[0, 0, 12, indexofob1] + OutputY[0, 0, 12, indexofob2]) / 3;
        fish09 = (OutputY[0, 0, 13, indexofob] + OutputY[0, 0, 13, indexofob1] + OutputY[0, 0, 13, indexofob2]) / 3;
        fish10 = (OutputY[0, 0, 14, indexofob] + OutputY[0, 0, 14, indexofob1] + OutputY[0, 0, 14, indexofob2]) / 3;
        fish11 = (OutputY[0, 0, 15, indexofob] + OutputY[0, 0, 15, indexofob1] + OutputY[0, 0, 15, indexofob2]) / 3;
        fish12 = (OutputY[0, 0, 16, indexofob] + OutputY[0, 0, 16, indexofob1] + OutputY[0, 0, 16, indexofob2]) / 3;
        fish13 = (OutputY[0, 0, 17, indexofob] + OutputY[0, 0, 17, indexofob1] + OutputY[0, 0, 17, indexofob2]) / 3;
        fish14 = (OutputY[0, 0, 18, indexofob] + OutputY[0, 0, 18, indexofob1] + OutputY[0, 0, 18, indexofob2]) / 3;
        fish15 = (OutputY[0, 0, 19, indexofob] + OutputY[0, 0, 19, indexofob1] + OutputY[0, 0, 19, indexofob2]) / 3;
        fish16 = (OutputY[0, 0, 20, indexofob] + OutputY[0, 0, 20, indexofob1] + OutputY[0, 0, 20, indexofob2]) / 3;


        if (x4 > 0.02693)
        {
            float[] temp_float = { fish01, fish02, fish03, fish04, fish05, fish06, fish07, fish08, fish09, fish10, fish11, fish12, fish13, fish14, fish15, fish16 };
            float tempindex = -1;
            int tempindex2 = -1;

            for (int iy = 0; iy < 7; iy++)
            {
                if (tempindex < temp_float[iy])
                {
                    tempindex = temp_float[iy];
                    tempindex2 = iy + 1;        //+1 ÇØ¼­ 0 ÀÏ¶§ ÀÎ½ÄÇÏ´Â°É ¹æÁö
                }
            }
            classNum = tempindex2;
        }
        else
        {
            classNum = 0;
        }

        print("À§Ä¡ÁÂÇ¥ 1: " + x0);
        print("À§Ä¡ÁÂÇ¥ 2: " + x1);
        print("À§Ä¡ÁÂÇ¥ 3: " + x2);
        print("À§Ä¡ÁÂÇ¥ 4: " + x3);
        print("°´Ã¼ ¿©ºÎ È®·ü: " + x4);

        // °¥Ä¡, °¨¼ºµ¼, ºÎ½Ã¸®, ¼þ¾î, ¿ì·°, ÀÓ¿¬¼ö, Âüµ¼, ÇÐ²ÇÄ¡, °íµî¾î, ±¤¾î, ³ó¾î, °¡ÀÚ¹Ì, ¸ÁµÕ¾î, ¹Î¾î, º¬¿¡µ¼, º¼¶ô

        if (classNum == 1) { textResult.text = $"°¥Ä¡"; }
        else if (classNum == 2) { textResult.text = $"°¨¼ºµ¼"; }
        else if (classNum == 3) { textResult.text = $"ºÎ½Ã¸®"; }
        else if (classNum == 4) { textResult.text = $"¼þ¾î"; }
        else if (classNum == 5) { textResult.text = $"¿ì·°"; }
        else if (classNum == 6) { textResult.text = $"ÀÓ¿¬¼ö"; }
        else if (classNum == 7) { textResult.text = $"Âüµ¼"; }
        else if (classNum == 8) { textResult.text = $"ÇÐ²ÇÄ¡"; }
        else if (classNum == 9) { textResult.text = $"°íµî¾î"; }
        else if (classNum == 10) { textResult.text = $"±¤¾î"; }
        else if (classNum == 11) { textResult.text = $"³ó¾î"; }
        else if (classNum == 12) { textResult.text = $"°¡ÀÚ¹Ì"; }
        else if (classNum == 13) { textResult.text = $"¸ÁµÕ¾î"; }
        else if (classNum == 14) { textResult.text = $"¹Î¾î"; }
        else if (classNum == 15) { textResult.text = $"º¬¿¡µ¼"; }
        else if (classNum == 16) { textResult.text = $"º¼¶ô"; }
        else { textResult.text = $"???"; }

        print(classNum);

        yield return 0;
    }

    private static Texture2D GetTextureFromCamera(Camera mCamera)
    {
        UnityEngine.Rect rect = new UnityEngine.Rect(0, 0, mCamera.pixelWidth, mCamera.pixelHeight);
        RenderTexture renderTexture = new RenderTexture(mCamera.pixelWidth, mCamera.pixelHeight, 24);
        Texture2D screenShot = new Texture2D(mCamera.pixelWidth, mCamera.pixelHeight, TextureFormat.RGBA32, false);

        mCamera.targetTexture = renderTexture;
        mCamera.Render();

        RenderTexture.active = renderTexture;

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        mCamera.targetTexture = null;
        RenderTexture.active = null;

        return screenShot;
    }

    private void OnDestroy()
    {
        _engine.Dispose();
    }
}