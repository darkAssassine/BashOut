using UnityEngine;

namespace BashOut.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private PlayerInput input;

        private float z;


        void Update()
        {
            //if (input.AttackDirection.x>0) 
            //{
            //    player.rotation = Quaternion.Euler(0, 0, 0);
            //}
            //if (input.AttackDirection.x < 0)
            //{
            //    player.rotation = Quaternion.Euler(0, 180, 0);
            //}
        }
    }
}
