using UnityEngine;

namespace WebcamVisors
{
    public class WebcamVisorComponent : MonoBehaviour
    {
        public PlayerVisor playerVisor = null;

        public void Awake()
        {
            playerVisor = gameObject.GetComponent<PlayerVisor>();
        }

        public void LateUpdate()
        {
            // Code goes here...
        }

        public void OnDestroy()
        {
            // CancelInvoke();
            // Destroy();
        }
    }
}