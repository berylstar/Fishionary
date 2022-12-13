using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using System.Linq;
using OpenCvSharp;
public class Projection : MonoBehaviour
{
    WebCamTexture _webCamTexture;
    public static Texture2D jjj;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCamTexture = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);
        jjj = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = _webCamTexture;
    }
}
