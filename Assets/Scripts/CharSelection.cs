using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharSelection : MonoBehaviour
{
  private int selectedEnemyIndex;
  private Color desiredColor;

  [Header ("Enemy List")]
  [SerializeField] private List<EnemySelectObject> enemyList = new List<EnemySelectObject>();


  [Header("UI References")]
  //[SerializeField] private TextMeshProGUI charName;
  [SerializeField] private Image characterSplash;
  [SerializeField] private Image backgroundColor;


  private void Start()
  {
    UpdateCharacterSelectionUI();
  }

  public void LeftArrow()
  {
    selectedEnemyIndex--;
    if (selectedEnemyIndex < 0)
    {
      selectedEnemyIndex = enemyList.Count - 1;
    }

    UpdateCharacterSelectionUI();
  }

    public void RightArrow()
  {
    selectedEnemyIndex++;
    if (selectedEnemyIndex == enemyList.Count)
    {
      selectedEnemyIndex = 0;
    }

    UpdateCharacterSelectionUI();
  }

  private void UpdateCharacterSelectionUI()
  {
      //Splash, Name, Desired Color
      characterSplash.sprite = enemyList[selectedEnemyIndex].splash;
      desiredColor = enemyList[selectedEnemyIndex].EnemyBGColor;

  }

  [System.Serializable]
  public class EnemySelectObject
  {
      public Sprite splash;
      public Color EnemyBGColor;
  }
}


