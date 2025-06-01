using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel = 100;

    public Slider xpBar; // Drag the Slider from the UI here in the Inspector

    void Start()
    {
        UpdateXPBar();
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        UpdateXPBar();
        GetComponent<PlayerXP>().GainXP(25);
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;
        xpToNextLevel += 50; // Optional: Make each level require more XP
    }

    void UpdateXPBar()
    {
        if (xpBar != null)
        {
            xpBar.value = (float)currentXP / xpToNextLevel;
        }
    }
}