﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
//contralodr que recebe a pontuação salva
public class ScoreTable : MonoBehaviour
{
    private Transform ScoreBarContainer;
    private Transform ScoreBarBody;
    private List<Transform> EntryTransformList;
    
    private readonly float ScoreHeight=20f;
    public int ScoreLenght = 10;//mostra apenas os 10 maiores pontuadores
    
    private void Awake()
    {
        ScoreBarContainer = transform.Find("BodyContainer");
        ScoreBarBody = ScoreBarContainer.Find("Body");
        ScoreBarBody.gameObject.SetActive(false);
        //se existir lista criada ele já a apresenta na tela
        if(File.Exists(Application.persistentDataPath + "/ScoreData.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/ScoreData.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            HighScoreList scoreList = (HighScoreList)bf.Deserialize(file);
            file.Close();
            scoreList.SortIt();//coloca os elementos em ordem

            EntryTransformList = new List<Transform>();
            //le a lista para colocar os elementos
            foreach(HighScore entry in scoreList.ScoreEntryList)
                CreateScore(entry, ScoreBarContainer, EntryTransformList);

        }       
    }

    private void CreateScore(HighScore scoreEntry, Transform container, List<Transform> transformList)
    {//constroe a tela de pontuação
        if(transformList.Count <= ScoreLenght)
        {
            Transform entry = Instantiate(ScoreBarBody, container);//instancia acada elementos da lista de pontuação caso existam
            RectTransform entryRect = entry.GetComponent<RectTransform>();
            entryRect.anchoredPosition = new Vector2(0, -ScoreHeight * transformList.Count);
            entry.gameObject.SetActive(true);
            //posicioana os elementos
            int rank = transformList.Count + 1;
            string pos;
            switch(rank)
            {
                case 1: pos = "1ST"; break;
                case 2: pos = "2ND"; break;
                case 3: pos = "3RD"; break;
                default: pos = transformList.Count + "TH"; break;
            }
            string name = scoreEntry.Name;
            int score = scoreEntry.Score;

            entry.transform.Find("PosText").GetComponent<Text>().text = pos;
            entry.transform.Find("NameText").GetComponent<Text>().text = name;
            entry.transform.Find("ScoreText").GetComponent<Text>().text = score.ToString();

            transformList.Add(entry);
        }
    }    
}
