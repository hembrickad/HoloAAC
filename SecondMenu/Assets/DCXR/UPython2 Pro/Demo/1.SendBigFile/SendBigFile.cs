using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace DCXR.Demo
{
    public class SendBigFile : MonoBehaviour
    {
        [SerializeField] UPython2ChannelSO bigfile;

        public Camera camera;

        Texture2D screenShot;
        RenderTexture renderTexture; 

        private void Start()
        {
            screenShot = new Texture2D(1920,1080, TextureFormat.ARGB32, false);
            renderTexture = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                CaptureCamera();
            }
        }


        public async void CaptureCamera()
        {

            camera.targetTexture = renderTexture;
            camera.Render();

            await Task.Yield();

            RenderTexture.active = camera.targetTexture;
            screenShot.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
            screenShot.Apply();

            await Task.Yield();


            byte[] bytes = screenShot.EncodeToJPG();


            var t = bigfile.ASendBigFile("screenshot.jpg", bytes);
            await Task.WhenAll(t);


            camera.targetTexture = null;
            RenderTexture.active = null;

            await Task.Yield();


            Debug.Log(string.Format("Image sent to \"screenshot.jpg\""));
        }
    }
}