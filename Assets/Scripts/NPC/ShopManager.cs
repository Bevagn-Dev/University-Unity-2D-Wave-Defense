using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

// Serializable class to define upgrade details
[System.Serializable]
public class ValueUpgrades
{
    // Name of the upgrade
    public string upgradeName;
    // Player values after the upgrade
    public PlayerValues playerValues;
    // Cost of the upgrade
    public int upgradeCost;         
}

public class ShopManager : MonoBehaviour
{
    // Array of upgrade options
    [Header("Upgrade Config")]
    public ValueUpgrades[] valueUpgrades;   

    [Header("Stats Text Elements")]
    // Text displaying current move stats
    [SerializeField] private TextMeshProUGUI currentMoveStats;
    // Text displaying current attack stats
    [SerializeField] private TextMeshProUGUI currentAttackStats;
    // Text displaying current health stats
    [SerializeField] private TextMeshProUGUI currentHealthStats;   

    [Space(10)]

    // Text displaying selected move stats
    [SerializeField] private TextMeshProUGUI selectedMoveStats;
    // Text displaying selected attack stats
    [SerializeField] private TextMeshProUGUI selectedAttackStats;
    // Text displaying selected health stats
    [SerializeField] private TextMeshProUGUI selectedHealthStats;

    // Text displaying current money
    [Header("Shop Text Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;

    // Parent object for the shop UI
    [Header("Shop UI")]
    public GameObject shopParent;

    // Reference to the PlayerUI script
    private PlayerUI playerUI;
    // Reference to the PlayerManager script
    private PlayerManager playerManager;

    // Currently selected upgrade
    private ValueUpgrades selectedUpgrade; 

    private void Awake()
    {
        // Get references
        playerUI = GetComponentInParent<PlayerUI>();
        playerManager = FindObjectOfType<PlayerManager>();

        // Disable shop UI initially
        shopParent.SetActive(false);
    }

    private void Update()
    {
        // Update current stats displayed in the shop UI
        UpdateCurrentStats();

        moneyText.SetText(playerManager.currentMoney.ToString());
    }

    // Update current stats displayed in the shop UI
    private void UpdateCurrentStats()
    {
        // Set current stats text
        currentMoveStats.SetText(
            $"Walk Speed: {playerManager.playerValues.walkSpeed} \n" +
            $"Sprint Speed: {playerManager.playerValues.sprintSpeed}"
        );

        currentAttackStats.SetText(
            $"Attack Damage: {playerManager.playerValues.attackDamage} \n" +
            $"Attack Radius: {playerManager.playerValues.attackRadius} \n" +
            $"Attack Rate: {playerManager.playerValues.attackRate}"
        );

        currentHealthStats.SetText(
            $"Max Health: {playerManager.playerValues.maxHealth} \n" +
            $"Regen Speed: {playerManager.playerValues.regenDelay} \n" +
            $"Regen Amount: {playerManager.playerValues.regenAmount}"
        );
    }

    // Show upgrade stats for the selected upgrade
    public void ShowUpgradeStats(int upgradeIndex)
    {
        selectedUpgrade = valueUpgrades[upgradeIndex];

        // Set selected stats text
        selectedMoveStats.SetText(
            $"Walk Speed: {selectedUpgrade.playerValues.walkSpeed} \n" +
            $"Sprint Speed: {selectedUpgrade.playerValues.sprintSpeed}"
        );

        selectedAttackStats.SetText(
            $"Attack Damage: {selectedUpgrade.playerValues.attackDamage} \n" +
            $"Attack Radius: {selectedUpgrade.playerValues.attackRadius} \n" +
            $"Attack Rate: {selectedUpgrade.playerValues.attackRate}"
        );

        selectedHealthStats.SetText(
            $"Max Health: {selectedUpgrade.playerValues.maxHealth} \n" +
            $"Regen Speed: {selectedUpgrade.playerValues.regenDelay} \n" +
            $"Regen Amount: {selectedUpgrade.playerValues.regenAmount}"
        );
    }

    // Buy the upgrade based on the upgrade index
    public void BuyUpgrade(int upgradeIndex)
    {
        if (upgradeIndex >= 0 && upgradeIndex < valueUpgrades.Length)
        {
            ShowUpgradeStats(upgradeIndex);
        }
    }

    // Methods to buy specific upgrades
    public void BuyRunner() => BuyUpgrade(0);
    public void BuyAllRounder() => BuyUpgrade(1);
    public void BuyTank() => BuyUpgrade(2);
    public void BuyAssassin() => BuyUpgrade(3);
    public void BuyGodTier() => BuyUpgrade(4);

    // Confirm the selected upgrade choice
    public void ConfirmChoice()
    {
        // Check if player has enough money and selected upgrade is different
        if (selectedUpgrade.upgradeCost <= playerManager.currentMoney && selectedUpgrade.playerValues != playerManager.playerValues)
        {
            // Deduct cost and apply upgrade
            playerManager.currentMoney -= selectedUpgrade.upgradeCost;
            playerManager.playerValues = selectedUpgrade.playerValues;

            // Update current stats
            UpdateCurrentStats();
        }
    }
}
