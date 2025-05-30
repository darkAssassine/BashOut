using UnityEngine;

namespace BashOut.FrameRate
{
    public class LimitFrameWork : MonoBehaviour
    {
        [SerializeField] private int FrameRate;
        void Start()
        {
            Application.targetFrameRate = FrameRate;
        }
    }
}
