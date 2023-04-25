using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[System.Serializable]
public class OptionalLetterSegment {
    public string keyword;
    [TextArea(5,10)] public string sentence;
}

public class ExitLetterBuilder : MonoBehaviour
{

    [TextArea(5,10)] [SerializeField] private string letterStart;
    [TextArea(5,10)] [SerializeField] private string letterEnd;

    [SerializeField] private List<OptionalLetterSegment> optionalSegments;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro text = GetComponent<TextMeshPro>();

        List<string> segments = new List<string>();

        segments.Add(letterStart);
        foreach (OptionalLetterSegment seg in optionalSegments) {
            string segKeyword = seg.keyword;
            if ((from id in SelectionManager.takenObjectIds where id.Contains(segKeyword) select id).Count() > 0) {
                segments.Add(seg.sentence);
            } 
        }
        segments.Add(letterEnd);

        string wholeText = string.Join(" ", segments);

        text.text = wholeText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
