using System.Collections.Generic;
using System.Linq;


[System.Serializable]//serialização para montar a lista de pontuação
public class HighScoreList
{
    public List<HighScore> ScoreEntryList;
    
    public void SortIt()
    {
        ScoreEntryList=ScoreEntryList.OrderByDescending(o => o.Score).ToList();
    }
}
