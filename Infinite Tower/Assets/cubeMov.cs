using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cubeMov : MonoBehaviour
{
    private Vector2 startTouchPosition;  // Position of the start of the touch
    private Vector2 endTouchPosition;    // Position of the end of the touch
    private bool isTouching = false;     // Flag to check if a touch is ongoing
    public GameObject camera; // Reference to your camera
    public GameObject textPrefab; // Prefab for the instantiated text
    public Transform canvasTransform; // Transform of the canvas in world space
    public bool clonato = false;        // Flag to indicate if the object has been cloned
    public bool inCollision = false;    // Flag to indicate if the object is in collision
    private bool punteggioInstanziato = false; // Flag to check if the score has been instantiated
    private float collisionTimer = 0f;   // Timer for collision time
    private float collisionDuration = 2f; // Duration of the required collision (2 seconds)
    public GameObject stair;
    private Renderer objRenderer;
    private Color startColor;
    public Color targetColor; // Color towards which the object changes during collision
    public GameObject explode;
    private Rigidbody rb;  // The object's Rigidbody

    public GameObject Clone;
    public GameObject explodesound;
    public GameObject bouncesound;
    public GameObject GameOverscreen;

    public INgameScore INgameScore;

    public bool mobile = true; // Set this in the editor to switch between mobile and PC controls
    public float speedPC = 5f; // Speed for PC movement

    public void OnDestroy()
    {
        if (!clonato)
        {
            if (GameOverscreen != null) GameOverscreen.SetActive(true);
            if (INgameScore != null) INgameScore.sGameOver();
        }
    }

    public void Awake()
    {
        inCollision = false;
        clonato = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (mobile)
        {
            HandleTouchMovement();
        }
        if(!mobile)
        {
            HandleKeyboardMovement();
        }
    }

    private void HandleTouchMovement()
    {
        if (Input.touchCount > 0 && !inCollision && !clonato)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isTouching = true;
            }

            if (touch.phase == TouchPhase.Moved && isTouching)
            {
                endTouchPosition = touch.position;
                float deltaX = endTouchPosition.x - startTouchPosition.x;

                Vector3 newPosition = transform.position;
                newPosition.x += deltaX * Time.deltaTime / 2.5f;  // Multiply by deltaTime for smooth movement
                transform.position = newPosition;

                startTouchPosition = endTouchPosition;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }
    }

    public void HandleKeyboardMovement()
    {
        if (!inCollision && !clonato)
        {
            Vector3 newPosition = transform.position;
            float moveDirection = 0f; // Determine the direction of movement

            // Check for continuous key press for left movement (A or Left Arrow)
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveDirection = -1f; // Move left
            }

            // Check for continuous key press for right movement (D or Right Arrow)
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection = 1f; // Move right
            }

            // Calculate the new target position based on direction and speed
            newPosition.x += moveDirection * speedPC * Time.deltaTime;

            // Smoothly interpolate between the current position and the new position
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.1f);
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("camerades"))
        {
            InstantiateExplosion();
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Dist"))
        {
            if (clonato) Destroy(gameObject);
            InstantiateExplosion();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo"))
        {
            if (!clonato)  // If not cloned yet
            {
                Instantiate(bouncesound, new Vector3(1.7f, camera.transform.position.y + 13f, gameObject.transform.position.z), Quaternion.identity);
                Clone = gameObject;

                // Create a clone of this GameObject
                GameObject clon = Instantiate(Clone, new Vector3(Random.Range(-0.7f, 4f), camera.transform.position.y + 13f, gameObject.transform.position.z), Quaternion.identity);
                Renderer clonRenderer = clon.GetComponent<Renderer>();

                if (clonRenderer != null)
                {
                    Color randomColor = new Color(Random.value, Random.value, Random.value);
                    clonRenderer.material.color = randomColor; // Set a random color on the clone
                }

                clonato = true; // Mark that the object has been cloned
                inCollision = true;
            }

            // Initialize the start color at the beginning of the collision
            startColor = GetComponent<Renderer>().material.color;
            collisionTimer = 0f; // Reset the timer on collision
        }
    }

    public void punt()
    {
        if (!punteggioInstanziato)
        {
            punteggioInstanziato = true;

            // Verify that references are not null
            if (textPrefab != null && canvasTransform != null && camera != null)
            {
                // Instantiate the text prefab
                GameObject textInstance = Instantiate(textPrefab);

                // Convert the global position of the cube to canvas coordinates
                Vector3 worldPosition = transform.position;
                Vector2 viewportPosition = camera.GetComponent<Camera>().WorldToViewportPoint(worldPosition);

                // Ensure that the position is in local coordinates of the canvas
                Vector2 canvasPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)canvasTransform,
                    new Vector2(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height),
                    camera.GetComponent<Camera>(),
                    out canvasPosition
                );

                // Add a random value to canvasPosition.x to move the text along the X-axis
                canvasPosition.x += Random.Range(-200f, 200f);

                // Position the text prefab within the canvas
                RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();
                textRectTransform.SetParent(canvasTransform, false);
                textRectTransform.anchoredPosition = canvasPosition;
            }
            else
            {
                Debug.LogWarning("Text prefab, canvas or camera not assigned.");
            }
        }
    }

    // Method to instantiate the explosion with the cube's color
    private void InstantiateExplosion()
    {
        Renderer thisss = GetComponent<Renderer>();
        if (!rb.isKinematic) Instantiate(explodesound, new Vector3(1.7f, camera.transform.position.y + 13f, gameObject.transform.position.z), Quaternion.identity);
        GameObject explosionInstance = Instantiate(explode, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = explosionInstance.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            // Get the color of the cube's material
            Color cubeColor = thisss.material.color;

            // Modify the color of the particles
            var mainModule = particleSystem.main;
            mainModule.startColor = cubeColor;
        }
        else
        {
            Debug.LogWarning("No ParticleSystem found on the explode object.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ball"))
        {
            collisionTimer = -8;
            rb.isKinematic = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo"))
        {
            if (!rb.isKinematic)
            {
                collisionTimer += Time.deltaTime;
                Renderer thisss = GetComponent<Renderer>();
                float t = collisionTimer / collisionDuration;
                thisss.material.color = Color.Lerp(startColor, targetColor, t);

                if (collisionTimer >= collisionDuration)
                {
                    rb.isKinematic = true;  // Make the Rigidbody static
                    punt(); // Instantiate the score only the first time
                }
            }

            Vector3 rotation = transform.rotation.eulerAngles;
            if (Mathf.Abs(rotation.x) > 5 || Mathf.Abs(rotation.y) > 5 || Mathf.Abs(rotation.z) > 5)
            {
                collisionTimer = 0f;
                rb.isKinematic = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo"))
        {
            // Any actions to perform when exiting the collision
            // Reset the timer and starting color if needed
            startColor = GetComponent<Renderer>().material.color; // Optional, if you want to keep the current color
        }
    }
}
