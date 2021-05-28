using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3D : MonoBehaviour
{
    private struct LetterInfo
    {
        public GameObject LetterObject;
        public Bounds LetterBounds;
        public LetterInfo(GameObject letter)
        {
            LetterObject = letter;
            LetterBounds = (LetterObject.GetComponentInChildren<MeshFilter>().sharedMesh.bounds);
        }
    }

    public List<GameObject> Letters;

    [Tooltip("The space between letters")]
    public float Kerning = 0.05f;

    [TextArea(5,5)]
    public string Text = "";

    private Dictionary<char, LetterInfo> storedLetters;

    private void Start()
    {
        storedLetters = new Dictionary<char, LetterInfo>();
        for(int i = 0; i < Letters.Count; i++)
        {
            storedLetters[Letters[i].name[0]] = new LetterInfo(Letters[i]);
        }

        storedLetters[' '] = new LetterInfo();

        float XDiff = 0;

        for(int i = 0; i < Text.Length; i++)
        {
            if (!storedLetters.ContainsKey(Text.ToUpper()[i])) continue;
            LetterInfo letter = storedLetters[Text.ToUpper()[i]];

            //Create the actual letter
            if(letter.LetterObject != null)
            {
                Instantiate(letter.LetterObject, transform.TransformPoint(new Vector3(XDiff, 0, 0)), Quaternion.identity, transform);
                XDiff -= letter.LetterBounds.size.x + Kerning;
            }
            else if(Text[i] == ' ')
            {
                XDiff -= letter.LetterBounds.size.x + (Kerning * 4f);
            }
        }
    }
}
