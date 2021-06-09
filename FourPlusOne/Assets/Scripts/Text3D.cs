using System;
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
    public float VerticalSpacing = 0.25f;

    [TextArea(5,5)]
    public string StartText = "";

    public string Text
    {
        get
        {
            return text;
        }

        set
        {
            text = value;
            CleanText();
        }
    }

    [HideInInspector]
    public bool TypeWrittingDone = false;

    private string text = "";

    [Header("Typewritter Settings")]
    public bool UseTypewritter = true;

    [Min(0.01f)]
    public float TimeBetweeenLetters = 0.1f;
    public float MaxWidth = 4;

    private float typewritterTimer = 0;
    private int charAvailable = 0;

    private Dictionary<char, LetterInfo> storedLetters;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position - (transform.right * MaxWidth));
    }

    private void Start()
    {
        Text = StartText;
        CreateLetters();
    }

    private void CreateLetters()
    {
        if (storedLetters != null) return;
        //Log all letters
        storedLetters = new Dictionary<char, LetterInfo>();
        for(int i = 0; i < Letters.Count; i++)
        {
            storedLetters[Letters[i].name[0]] = new LetterInfo(Letters[i]);
        }

        storedLetters[' '] = new LetterInfo();

        CleanText();
    }

    private void CleanText()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        float XDiff = 0;
        float YDiff = 0;
        charAvailable = 0;
        TypeWrittingDone = false;

        bool sameWord = false;

        CreateLetters();

        for (int i = 0; i < text.Length; i++)
        {
            if (!storedLetters.ContainsKey(text.ToUpper()[i])) continue;
            LetterInfo letter = storedLetters[text.ToUpper()[i]];

            //Create the actual letter
            if (letter.LetterObject != null)
            {
                //Get World Width
                int length = 0;
                float temp = 0;

                if (!sameWord)
                {
                    while (i < text.Length && text[i] != ' ' && storedLetters.ContainsKey(text.ToUpper()[i]))
                    {
                        temp += storedLetters[text.ToUpper()[i]].LetterBounds.size.x + Kerning;
                        length++;
                        i++; 
                    }
                    sameWord = true;
                }

                XDiff -= temp;

                if (Mathf.Abs(XDiff) >= MaxWidth)
                {
                    YDiff -= VerticalSpacing;
                    XDiff = 0;
                }
                else
                {
                    XDiff += temp;
                }

                i -= length;

                GameObject obje = Instantiate(letter.LetterObject, transform.TransformPoint(new Vector3(XDiff, YDiff, 0)), transform.rotation, transform);
                if (UseTypewritter) obje.SetActive(false);
                XDiff -= letter.LetterBounds.size.x + Kerning;

            }
            else if (text[i] == ' ')
            {
                XDiff -= letter.LetterBounds.size.x + (Kerning * 4f);
                sameWord = false;
            }
        }
    }

    private void Update()
    {
        if (UseTypewritter && !TypeWrittingDone)
        {
            typewritterTimer += Time.deltaTime;

            while(typewritterTimer >= TimeBetweeenLetters)
            {
                if (charAvailable >= transform.childCount)
                {
                    charAvailable = 0;
                    typewritterTimer = 0;
                    TypeWrittingDone = true;
                    break;
                }

                typewritterTimer -= TimeBetweeenLetters;
                transform.GetChild(charAvailable).gameObject.SetActive(true);

                charAvailable++;
            }
        }
    }
}
