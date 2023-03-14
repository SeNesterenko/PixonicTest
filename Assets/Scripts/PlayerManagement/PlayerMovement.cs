using UnityEngine;

namespace PlayerManagement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 2f;

        public void Move(float x, float y)
        {
            var moveStep = Time.deltaTime * _moveSpeed;
            var rotationStep = Time.deltaTime * _rotationSpeed;
        
            transform.Translate(Vector3.up * moveStep * y);
            transform.Rotate(new Vector3(0, 0, x) * -90 * rotationStep);
        }
    }
}