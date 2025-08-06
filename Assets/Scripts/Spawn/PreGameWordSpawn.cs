using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameWordSpawn : MonoBehaviour
{
    //access the letter spawn script
    public LetterSpawnScript letterSpawnScript;
    //row 1
    public Vector3 spawnPosition0;
    public Vector3 spawnPosition1;
    public Vector3 spawnPosition2;
    public Vector3 spawnPosition3;
    public Vector3 spawnPosition4;
    public Vector3 spawnPosition5;
    //row 2
    public Vector3 spawnPosition6;
    public Vector3 spawnPosition7;
    public Vector3 spawnPosition8;
    public Vector3 spawnPosition9;
    public Vector3 spawnPosition10;
    public Vector3 spawnPosition11;
    //row 3
    public Vector3 spawnPosition12;
    public Vector3 spawnPosition13;
    public Vector3 spawnPosition14;
    public Vector3 spawnPosition15;
    public Vector3 spawnPosition16;
    public Vector3 spawnPosition17;
    private Dictionary<char, GameObject> letterPrefabs = new Dictionary<char, GameObject>();
    public GameObject bombLetterB;
    GameObject zElectric;


    void Awake()
    {
        letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
    }
    void Start()
    {
        StartCoroutine(WordCreated());
        for (int i = 0; i < 26; i++)
        {
            char letterChar = (char)('A' + i);
            letterPrefabs[letterChar] = letterSpawnScript.letterDataList[i].prefab;
        }
    }

    IEnumerator WordCreated()
    {
        yield return new WaitForEndOfFrame();
        //        Debug.Log("PreGameWordSpawn: WordCreated called" + letterSpawnScript.level);
        List<Vector3> row1 = new List<Vector3>();
        row1.Add(spawnPosition0);
        row1.Add(spawnPosition1);
        row1.Add(spawnPosition2);
        row1.Add(spawnPosition3);
        row1.Add(spawnPosition4);
        row1.Add(spawnPosition5);
        List<Vector3> row2 = new List<Vector3>();
        row2.Add(spawnPosition6);
        row2.Add(spawnPosition7);
        row2.Add(spawnPosition8);
        row2.Add(spawnPosition9);
        row2.Add(spawnPosition10);
        row2.Add(spawnPosition11);
        List<Vector3> row3 = new List<Vector3>();
        row3.Add(spawnPosition12);
        row3.Add(spawnPosition13);
        row3.Add(spawnPosition14);
        row3.Add(spawnPosition15);
        row3.Add(spawnPosition16);
        row3.Add(spawnPosition17);

        zElectric = letterSpawnScript.letterDataList[25].prefab;

        if (GameManager.Instance.GMLevel == 16 || GameManager.Instance.GMLevel == 36 || GameManager.Instance.GMLevel == 56 || GameManager.Instance.GMLevel == 76 || GameManager.Instance.GMLevel == 96)
        {
            //spell chain in the spawn positions 6 through 10
            Instantiate(letterPrefabs['C'], spawnPosition6, Quaternion.identity);
            Instantiate(letterPrefabs['H'], spawnPosition7, Quaternion.identity);
            Instantiate(letterPrefabs['A'], spawnPosition8, Quaternion.identity);
            Instantiate(letterPrefabs['I'], spawnPosition9, Quaternion.identity);
            GameObject electricNLetter = Instantiate(letterPrefabs['N'], spawnPosition10, Quaternion.identity);
            electricNLetter.GetComponent<Letter>().isElectric = true;
            foreach (Vector3 position in row1)
            {
                GameObject electricZLetter = Instantiate(zElectric, position, Quaternion.identity);
                electricZLetter.GetComponent<Letter>().isElectric = true;
            }
        }
        if (GameManager.Instance.GMLevel == 17 || GameManager.Instance.GMLevel == 37 || GameManager.Instance.GMLevel == 57 || GameManager.Instance.GMLevel == 77 || GameManager.Instance.GMLevel == 97)
        {
            foreach (Vector3 position in row1)
            {
                GameObject electricZLetter = Instantiate(zElectric, position, Quaternion.identity);
                electricZLetter.GetComponent<Letter>().isElectric = true;
            }
            foreach (Vector3 position in row2)
            {
                GameObject electricZLetter = Instantiate(zElectric, position, Quaternion.identity);
                electricZLetter.GetComponent<Letter>().isElectric = true;
            }
        }
        if (GameManager.Instance.GMLevel == 18 || GameManager.Instance.GMLevel == 38 || GameManager.Instance.GMLevel == 58 || GameManager.Instance.GMLevel == 78 || GameManager.Instance.GMLevel == 98)
        {
            foreach (Vector3 position in row1)
            {
                GameObject electricZLetter = Instantiate(zElectric, position, Quaternion.identity);
                electricZLetter.GetComponent<Letter>().isElectric = true;
            }
            foreach (Vector3 position in row2)
            {
                GameObject electricZLetter = Instantiate(zElectric, position, Quaternion.identity);
                electricZLetter.GetComponent<Letter>().isElectric = true;
            }
            foreach (Vector3 position in row3)
            {
                GameObject electricZLetter = Instantiate(zElectric, position, Quaternion.identity);
                electricZLetter.GetComponent<Letter>().isElectric = true;
            }
        }
        if (GameManager.Instance.GMLevel == 11 || GameManager.Instance.GMLevel == 31 || GameManager.Instance.GMLevel == 51 || GameManager.Instance.GMLevel == 71 || GameManager.Instance.GMLevel == 91)
        {
            bombLetterB = letterPrefabs['B'];
            Instantiate(letterPrefabs['B'], spawnPosition0, Quaternion.identity);
            Instantiate(letterPrefabs['O'], spawnPosition1, Quaternion.identity);
            Instantiate(letterPrefabs['M'], spawnPosition2, Quaternion.identity);
            GameObject Bomb = Instantiate(bombLetterB, spawnPosition10, Quaternion.identity);
            Bomb.GetComponent<Letter>().letterIsBomb = true;

            Debug.Log("PreGameWordSpawn: Letter B created and is a bombLetterB" + Bomb.name + "letter is a bomb" + Bomb.GetComponent<Letter>().letterIsBomb);

            //zzs to be blown up
            //spawn the letter z in position 4 5 6 9 and 11 15 and 17
            Instantiate(letterPrefabs['Z'], spawnPosition3, Quaternion.identity);
            Instantiate(letterPrefabs['Z'], spawnPosition4, Quaternion.identity);
            Instantiate(letterPrefabs['Z'], spawnPosition5, Quaternion.identity);
            Instantiate(letterPrefabs['Z'], spawnPosition9, Quaternion.identity);
            Instantiate(letterPrefabs['Z'], spawnPosition11, Quaternion.identity);
            Instantiate(letterPrefabs['Z'], spawnPosition15, Quaternion.identity);
            Instantiate(letterPrefabs['Z'], spawnPosition17, Quaternion.identity);
        }
        HashSet<int> excludedLevels = new HashSet<int>
        {
            11, 31, 51, 71, 91,
            16, 36, 56, 76, 96,
            17, 37, 57, 77, 97,
            18, 38, 58, 78, 98
        };
        if (!excludedLevels.Contains(GameManager.Instance.GMLevel))
        {
            //randomly select from one of the three words: good luck, keep going, faster
            int randomWord = UnityEngine.Random.Range(0, 7);


            if (randomWord == 0)
            {
                GoodLuck();
            }
            else if (randomWord == 1)
            {
                KeepGoing();
            }
            else if (randomWord == 2)
            {
                Faster();
            }
            else if (randomWord == 3)
            {
                PuzzleMaster();
            }
            else if (randomWord == 4)
            {
                Speedy();
            }
            else if (randomWord == 5)
            {
                CrunchTime();
            }
            else if (randomWord == 6)
            {
                OneTough();
            }
        }
    }

    void Faster()
    {
        //spawn the letter F in position 0
        Instantiate(letterPrefabs['F'], spawnPosition0, Quaternion.identity);
        //spawn the letter A in position 1
        Instantiate(letterPrefabs['A'], spawnPosition1, Quaternion.identity);
        //spawn the letter S in position 2
        Instantiate(letterPrefabs['S'], spawnPosition2, Quaternion.identity);
        //spawn the letter T in position 3
        Instantiate(letterPrefabs['T'], spawnPosition3, Quaternion.identity);
        //spawn the letter E in position 4
        Instantiate(letterPrefabs['E'], spawnPosition4, Quaternion.identity);
        //spawn the letter R in position 5
        Instantiate(letterPrefabs['R'], spawnPosition5, Quaternion.identity);
    }
    void KeepGoing()
    {
        Instantiate(letterPrefabs['G'], spawnPosition0, Quaternion.identity);
        Instantiate(letterPrefabs['O'], spawnPosition1, Quaternion.identity);
        Instantiate(letterPrefabs['I'], spawnPosition2, Quaternion.identity);
        Instantiate(letterPrefabs['N'], spawnPosition3, Quaternion.identity);
        Instantiate(letterPrefabs['K'], spawnPosition6, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition7, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition8, Quaternion.identity);
        Instantiate(letterPrefabs['P'], spawnPosition9, Quaternion.identity);
        Instantiate(letterPrefabs['G'], spawnPosition4, Quaternion.identity);
    }
    void GoodLuck()
    {
        //spawn the letter G in position 0
        Instantiate(letterPrefabs['L'], spawnPosition0, Quaternion.identity);
        //spawn the letter O in position 1
        Instantiate(letterPrefabs['U'], spawnPosition1, Quaternion.identity);
        //spawn the letter O in position 2
        Instantiate(letterPrefabs['C'], spawnPosition2, Quaternion.identity);
        //spawn the letter D in position 3
        Instantiate(letterPrefabs['K'], spawnPosition3, Quaternion.identity);
        //spawn the letter L in position 4
        Instantiate(letterPrefabs['G'], spawnPosition6, Quaternion.identity);
        //spawn the letter U in position 5
        Instantiate(letterPrefabs['O'], spawnPosition7, Quaternion.identity);
        //spawn the letter C in position 6
        Instantiate(letterPrefabs['O'], spawnPosition8, Quaternion.identity);
        //spawn the letter K in position 7
        Instantiate(letterPrefabs['D'], spawnPosition9, Quaternion.identity);
    }
    void PuzzleMaster()
    {
        //spawn MASTER then PUZZLE
        Instantiate(letterPrefabs['M'], spawnPosition0, Quaternion.identity);
        Instantiate(letterPrefabs['A'], spawnPosition1, Quaternion.identity);
        Instantiate(letterPrefabs['S'], spawnPosition2, Quaternion.identity);
        Instantiate(letterPrefabs['T'], spawnPosition3, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition4, Quaternion.identity);
        Instantiate(letterPrefabs['R'], spawnPosition5, Quaternion.identity);
        Instantiate(letterPrefabs['P'], spawnPosition6, Quaternion.identity);
        Instantiate(letterPrefabs['U'], spawnPosition7, Quaternion.identity);
        Instantiate(letterPrefabs['Z'], spawnPosition8, Quaternion.identity);
        Instantiate(letterPrefabs['Z'], spawnPosition9, Quaternion.identity);
        Instantiate(letterPrefabs['L'], spawnPosition10, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition11, Quaternion.identity);
    }
    void Speedy()
    {
        Instantiate(letterPrefabs['S'], spawnPosition0, Quaternion.identity);
        Instantiate(letterPrefabs['P'], spawnPosition1, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition2, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition3, Quaternion.identity);
        Instantiate(letterPrefabs['D'], spawnPosition4, Quaternion.identity);
        Instantiate(letterPrefabs['Y'], spawnPosition5, Quaternion.identity);
    }
    void CrunchTime()
    {
        Instantiate(letterPrefabs['T'], spawnPosition0, Quaternion.identity);
        Instantiate(letterPrefabs['I'], spawnPosition1, Quaternion.identity);
        Instantiate(letterPrefabs['M'], spawnPosition2, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition3, Quaternion.identity);
        Instantiate(letterPrefabs['C'], spawnPosition6, Quaternion.identity);
        Instantiate(letterPrefabs['R'], spawnPosition7, Quaternion.identity);
        Instantiate(letterPrefabs['U'], spawnPosition8, Quaternion.identity);
        Instantiate(letterPrefabs['N'], spawnPosition9, Quaternion.identity);
        Instantiate(letterPrefabs['C'], spawnPosition4, Quaternion.identity);
        Instantiate(letterPrefabs['H'], spawnPosition5, Quaternion.identity);
    }
    void OneTough()
    {
        Instantiate(letterPrefabs['O'], spawnPosition0, Quaternion.identity);
        Instantiate(letterPrefabs['N'], spawnPosition1, Quaternion.identity);
        Instantiate(letterPrefabs['E'], spawnPosition2, Quaternion.identity);
        Instantiate(letterPrefabs['T'], spawnPosition6, Quaternion.identity);
        Instantiate(letterPrefabs['O'], spawnPosition7, Quaternion.identity);
        Instantiate(letterPrefabs['U'], spawnPosition8, Quaternion.identity);
        Instantiate(letterPrefabs['G'], spawnPosition3, Quaternion.identity);
        Instantiate(letterPrefabs['H'], spawnPosition4, Quaternion.identity);
    }
}
