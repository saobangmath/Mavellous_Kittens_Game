using UnityEngine;

/// <summary>
/// This class provides the animation for the Whirlwind thrown by the player.
/// </summary>
public class WhirlwindScript : MonoBehaviour
{
        private Transform _currPos;
        
        /// <summary>
        /// In the Start() method, the current position of the Whirlwind is initialised.
        /// </summary>
        // Start is called before the first frame update
        void Start()
        {
                _currPos = transform;
        }

        /// <summary>
        /// This method controls the motion of the whirlwind.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
                Vector3 _nextPos = _currPos.position;
                _nextPos.x += 3.75f *Time.deltaTime;
                transform.position = _nextPos;
        }

        /// <summary>
        /// This method destroys the whirlwind when it has hit the enemy.
        /// </summary>
        private void OnCollisionEnter2D(Collision2D other)
        {
                if (other.gameObject.CompareTag("Enemy"))
                {
                        Destroy(this.gameObject);
                }
        }
}

