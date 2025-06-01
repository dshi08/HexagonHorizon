using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel = 100;

    public Slider xpBar; // Drag the Slider from the UI here in the Inspector
    public PlayerMovement playerMovement; // Assign your Player GameObject with PlayerMovement here

    public float slideSpeed = 0.5f; // How fast the XP bar slides (higher is faster)

    private Coroutine xpSlideCoroutine; // To keep track of the running coroutine

    void Start()
    {
        // Set the initial value without sliding, to avoid a visual glitch on start
        if (xpBar != null)
        {
            xpBar.value = (float)currentXP / xpToNextLevel;
        }

        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement script not found! Please assign it in the Inspector.");
            }
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        // Instead of calling UpdateXPBar directly, start the sliding coroutine
        UpdateXPBarSmoothly();
    }

    void LevelUp()
    {

        currentXP -= xpToNextLevel;
        level++;
        xpToNextLevel += 50; // Increase XP needed for next level

        FindObjectOfType<GridManager>().IncreaseRevealRadius();


    }

    // This method initiates the smooth update
    void UpdateXPBarSmoothly()
    {
        if (xpBar != null)
        {
            // If a slide is already in progress, stop it to start a new one
            if (xpSlideCoroutine != null)
            {
                StopCoroutine(xpSlideCoroutine);
            }
            // Start the new smooth slide coroutine
            xpSlideCoroutine = StartCoroutine(SlideXPBar(xpBar.value, (float)currentXP / xpToNextLevel));
        }
    }

    // Coroutine for smooth XP bar sliding
    IEnumerator SlideXPBar(float startValue, float targetValue)
    {
        float timer = 0f;
        while (timer < slideSpeed)
        {
            timer += Time.deltaTime;

            xpBar.value = Mathf.Lerp(startValue, targetValue, timer / slideSpeed);
            yield return null;
        }
        xpBar.value = targetValue;
        xpSlideCoroutine = null;
    }
}