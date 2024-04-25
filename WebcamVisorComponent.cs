using bosqmode.libvlc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace WebcamVisors
{
    public class WebcamVisorComponent : MonoBehaviour
    {
        public PlayerVisor playerVisor = null;
        public SkinnedMeshRenderer playerVisorRenderer = null;
        public List<Material> playerVisorMaterials = null;
        public Material visorMaterial = null;
        public Material faceCamMaterial = null;
        public WebCamTexture webcamTexture = null;
        public VLCPlayer vlcPlayer;
        public Texture2D vlcTexture;
        public byte[] vlcRawImage;
        public string webcamType = null;
        public string webcamSource = null;

        public void EstablishVLCPlayer()
        {
            vlcPlayer = new VLCPlayer(512, 512, webcamSource, false);
            vlcTexture = new Texture2D(512, 512, TextureFormat.RGB24, false, false);
            faceCamMaterial.mainTexture = vlcTexture;
        }

        public void EstablishLocalWebcam()
        {
            webcamTexture = new WebCamTexture(webcamSource);
            faceCamMaterial.mainTexture = webcamTexture;
        }

        public void Start()
        {
            playerVisor = gameObject.GetComponent<PlayerVisor>();
            playerVisorRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            playerVisorMaterials = playerVisorRenderer.materials.ToList();
            visorMaterial = playerVisorRenderer.materials[2];
            faceCamMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            faceCamMaterial.color = new Color(0.69f, 0.69f, 0.69f, 1f); // Lazy brightness fix
            faceCamMaterial.SetFloat("_Smoothness", 0f);
            playerVisorMaterials[2] = faceCamMaterial;
            playerVisorRenderer.materials = playerVisorMaterials.ToArray();

            switch (playerVisor.visorColorIndex)
            {
                case 0: // Yellow
                    webcamType = WebcamVisors.configWebcamTypeA.Value;
                    webcamSource = WebcamVisors.configWebcamSourceA.Value;
                    break;
                case 1: // Orange
                    webcamType = WebcamVisors.configWebcamTypeB.Value;
                    webcamSource = WebcamVisors.configWebcamSourceB.Value;
                    break;
                case 2: // Red
                    webcamType = WebcamVisors.configWebcamTypeC.Value;
                    webcamSource = WebcamVisors.configWebcamSourceC.Value;
                    break;
                case 3: // Pink
                    webcamType = WebcamVisors.configWebcamTypeD.Value;
                    webcamSource = WebcamVisors.configWebcamSourceD.Value;
                    break;
                default:
                    Destroy(this);
                    break;
            }

            switch (webcamType)
            {
                case "Local":
                    EstablishLocalWebcam();
                    break;
                case "URL":
                    EstablishVLCPlayer();
                    break;
                default:
                    Destroy(this);
                    break;
            }
        }

        public void Update()
        {
            if (webcamType == "Local" && !webcamTexture.isPlaying)
            {
                webcamTexture.Play();
            }

            if (webcamType == "URL" && vlcTexture != null && vlcPlayer != null && vlcPlayer.CheckForImageUpdate(out vlcRawImage))
            {
                vlcTexture.LoadRawTextureData(vlcRawImage);
                vlcTexture.Apply(false);
            }
        }

        public void OnDestroy()
        {
            playerVisorMaterials[2] = visorMaterial;
            playerVisorRenderer.materials = playerVisorMaterials.ToArray();
            CancelInvoke();
            vlcPlayer?.Dispose();
        }
    }
}