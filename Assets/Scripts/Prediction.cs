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

    public static float x0;     //��ġ��ǥ1
    public static float x1;     //��ġ��ǥ2
    public static float x2;     //��ġ��ǥ3
    public static float x3;     //��ġ��ǥ4
    public static float x4;     //����Ȯ��

    public static float fish01;     //��ġ
    public static float fish02;     //������
    public static float fish03;     //�νø�
    public static float fish04;     //����
    public static float fish05;     //�췰
    public static float fish06;     //�ӿ���
    public static float fish07;     //����
    public static float fish08;     //�в�ġ
    public static float fish09;     //����
    public static float fish10;     //����
    public static float fish11;     //���
    public static float fish12;     //���ڹ�
    public static float fish13;     //���վ�
    public static float fish14;     //�ξ�
    public static float fish15;     //������
    public static float fish16;     //����

    // ��ġ, ������, �νø�, ����, �췰, �ӿ���, ����, �в�ġ, ����, ����, ���, ���ڹ�, ���վ�, �ξ�, ������, ����

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

    // ����, ���� �� �����ؼ� Resize �ϱ�
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

                indexofob2 = indexofob1;    //���� ����Ȯ���� �ִ��� �ε��� ����
                indexofob1 = indexofob;     // ���� ����Ȯ���� �ִ��� �ε��� ����
                indexofob = i;              //���� ����Ȯ���� �ִ��� �ε��� ����
            }
        }

        x0 = OutputY[0, 0, 0, indexofob];
        x1 = OutputY[0, 0, 1, indexofob];
        x2 = OutputY[0, 0, 2, indexofob];
        x3 = OutputY[0, 0, 3, indexofob];

        //NMS ����
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
                    tempindex2 = iy + 1;        //+1 �ؼ� 0 �϶� �ν��ϴ°� ����
                }
            }
            classNum = tempindex2;
        }
        else
        {
            classNum = 0;
        }

        print("��ġ��ǥ 1: " + x0);
        print("��ġ��ǥ 2: " + x1);
        print("��ġ��ǥ 3: " + x2);
        print("��ġ��ǥ 4: " + x3);
        print("��ü ���� Ȯ��: " + x4);

        // ��ġ, ������, �νø�, ����, �췰, �ӿ���, ����, �в�ġ, ����, ����, ���, ���ڹ�, ���վ�, �ξ�, ������, ����

        if (classNum == 1) { textResult.text = $"��ġ"; }
        else if (classNum == 2) { textResult.text = $"������"; }
        else if (classNum == 3) { textResult.text = $"�νø�"; }
        else if (classNum == 4) { textResult.text = $"����"; }
        else if (classNum == 5) { textResult.text = $"�췰"; }
        else if (classNum == 6) { textResult.text = $"�ӿ���"; }
        else if (classNum == 7) { textResult.text = $"����"; }
        else if (classNum == 8) { textResult.text = $"�в�ġ"; }
        else if (classNum == 9) { textResult.text = $"����"; }
        else if (classNum == 10) { textResult.text = $"����"; }
        else if (classNum == 11) { textResult.text = $"���"; }
        else if (classNum == 12) { textResult.text = $"���ڹ�"; }
        else if (classNum == 13) { textResult.text = $"���վ�"; }
        else if (classNum == 14) { textResult.text = $"�ξ�"; }
        else if (classNum == 15) { textResult.text = $"������"; }
        else if (classNum == 16) { textResult.text = $"����"; }
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