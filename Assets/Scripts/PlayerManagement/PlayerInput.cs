using UnityEngine;

namespace PlayerManagement
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerScroller))]
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerScroller _playerScroller;

        private void Update()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            var mouseScroll = Input.GetAxis("Mouse ScrollWheel");
    
            _playerMovement.Move(x, y);
            _playerScroller.Scroll(mouseScroll);
        }
    }
}
