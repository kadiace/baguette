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
}