using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public int Exp;
    public int Level;
    public int Hp;
    public int MaxHp;
    public int Bread;
    public int MaxBread;
    public bool UseMonster;
    public bool UseButter;
    public int MonsterAmount;
    public int ButterAmount;
    public List<Ability> Abilities;
}

public class PlayerManager
{
    private PlayerController _playerController;
    private PlayerStat _playerStat;

    private UI_InGame _uI_InGame;
    private UI_Abilities _uI_Abilities;

    private Ability?[] _abilityCandidates;
    private Dictionary<int, int> _expTable = new()
    {
        {1, 10},
        {2, 15},
        {3, 25},
        {4, 40}
    };
    private int _maxLevel = 5;

    public PlayerController PlayerController { get { return _playerController; } set { _playerController = value; } }
    public PlayerStat PlayerStat { get { return _playerStat; } set { _playerStat = value; } }
    public UI_InGame UI_InGame { get { return _uI_InGame; } set { _uI_InGame = value; } }
    public UI_Abilities UI_Abilities { get { return _uI_Abilities; } set { _uI_Abilities = value; } }
    public Ability?[] AbilityCandidates { get { return _abilityCandidates; } set { _abilityCandidates = value; } }

    public void Init()
    {
        _playerStat = new()
        {
            Exp = 0,
            Level = 1,
            Hp = 5,
            MaxHp = 5,
            Bread = 5,
            MaxBread = 5,
            UseMonster = false,
            UseButter = false,
            MonsterAmount = 5,
            ButterAmount = 5,
            Abilities = new()
        };
        _abilityCandidates = new Ability?[6];
        GetRandomAbilities(new List<Ability>() { Ability.Vigilante });
        _abilityCandidates[UnityEngine.Random.Range(0, _abilityCandidates.Length)] = Ability.Vigilante;
    }

    public int GetMaxExp()
    {
        return _expTable[_playerStat.Level];
    }

    public void AcquireExp(int exp)
    {
        int maxExp = _expTable[_playerStat.Level];
        _playerStat.Exp += exp;
        bool levelUp = false;
        int increasedLevel = 0;

        while (maxExp <= _playerStat.Exp)
        {
            _playerStat.Level += 1;
            _playerStat.Exp -= maxExp;
            maxExp = _expTable[_playerStat.Level];
            increasedLevel++;
            levelUp = true;
        }

        if (levelUp)
        {
            // Turn on ability select window by level with increasedLevel
            EnableAbilities();
        }
    }

    public void SetAbility(Ability ability)
    {
        _playerStat.Abilities.Add(ability);
    }

    public bool UseMonsterBuff()
    {
        if (_playerStat.UseMonster || _playerStat.MonsterAmount <= 0)
            return false;
        _playerStat.UseMonster = true;
        _playerStat.MonsterAmount--;
        return true;
    }

    public void TurnOffMonsterBuff()
    {
        _playerStat.UseMonster = false;
    }

    public bool UseButterBuff()
    {
        if (_playerStat.UseButter || _playerStat.ButterAmount <= 0)
            return false;
        _playerStat.UseButter = true;
        _playerStat.ButterAmount--;
        return true;
    }

    public void TurnOffButterBuff()
    {
        _playerStat.UseButter = false;
    }

    public void EnableAbilities()
    {
        UI_Abilities.gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
            UI_Abilities.UI_AbilityCards[i].SetCard(i);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableAbilities()
    {
        UI_Abilities.gameObject.SetActive(false);
        GetRandomAbilities(_playerStat.Abilities);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void GetRandomAbilities(List<Ability> excludedAbilities)
    {
        List<Ability> pool = ((Ability[])Enum.GetValues(typeof(Ability)))
            .Where(ability => ability != Ability.Unknown &&
                !excludedAbilities.Contains(ability)).ToList();

        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        Array.Clear(_abilityCandidates, 0, _abilityCandidates.Length);

        int count = Mathf.Min(_abilityCandidates.Length, pool.Count);

        for (int i = 0; i < count; i++)
            _abilityCandidates[i] = pool[i];
    }
}