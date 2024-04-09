using bosqmode.libvlc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WebcamVisors
{
    public class WebcamVisorComponent : MonoBehaviour
    {
        public SkinnedMeshRenderer playerVisor = null;
        public Material visorMaterial = null;
        public Material faceCamMaterial = null;

        private VLCPlayer vlcPlayer;
        private Texture2D vlcTexture;
        private string url = "<your-rtsp-stream-url-here>";
        private byte[] img;

        public void Awake()
        {
            playerVisor = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            visorMaterial = playerVisor.materials[2];
        }

        public void LateUpdate()
        {
            if (vlcPlayer == null)
            {
                vlcPlayer = new VLCPlayer(512, 512, url, false);
            }

            if (vlcTexture == null)
            {
                vlcTexture = new Texture2D(512, 512, TextureFormat.RGB24, false, false);
            }

            if (vlcPlayer != null && vlcTexture != null && vlcPlayer.CheckForImageUpdate(out img))
            {
                vlcTexture.LoadRawTextureData(img);
                vlcTexture.Apply(false);
            }

            if (faceCamMaterial == null)
            {
                faceCamMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                faceCamMaterial.mainTexture = vlcTexture;
                faceCamMaterial.color = new Color(0.69f, 0.69f, 0.69f, 1f); // Lazy brightness fix
                faceCamMaterial.SetFloat("_Smoothness", 0f);
            }

            if (playerVisor.materials[2] != faceCamMaterial)
            {
                List<Material> materials = playerVisor.materials.ToList();
                materials[2] = faceCamMaterial;
                playerVisor.materials = materials.ToArray();
            }
        }

        public void OnDestroy()
        {
            vlcPlayer?.Dispose();
        }
    }
}