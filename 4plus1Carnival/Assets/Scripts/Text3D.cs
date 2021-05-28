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
    public string Text = "";

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
        storedLetters = new Dictionary<char, LetterInfo>();
        for(int i = 0; i < Letters.Count; i++)
        {
            storedLetters[Letters[i].name[0]] = new LetterInfo(Letters[i]);
        }

        storedLetters[' '] = new LetterInfo();

        float XDiff = 0;
        float YDiff = 0;

        for(int i = 0; i < Text.Length; i++)
        {
            if (!storedLetters.ContainsKey(Text.ToUpper()[i])) continue;
            LetterInfo letter = storedLetters[Text.ToUpper()[i]];

            //Create the actual letter
            if(letter.LetterObject != null)
            {
                //Get World Width
                int length = 0;
                float temp = 0;

                while (i <= Text.Length && Text[i] != ' ' && storedLetters.ContainsKey(Text.ToUpper()[i]))
                {
                    temp += storedLetters[Text.ToUpper()[i]].LetterBounds.size.x + Kerning;
                    length++;
                    i++;
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

                GameObject obje = Instantiate(letter.LetterObject, transform.TransformPoint(new Vector3(XDiff, YDiff, 0)), Quaternion.identity, transform);
                if (UseTypewritter) obje.SetActive(false);
                XDiff -= letter.LetterBounds.size.x + Kerning;

            }
            else if(Text[i] == ' ')
            {
                XDiff -= letter.LetterBounds.size.x + (Kerning * 4f);
            }
        }
    }

    private void Update()
    {
        if (UseTypewritter)
        {
            typewritterTimer += Time.deltaTime;

            while(typewritterTimer >= TimeBetweeenLetters)
            {
                typewritterTimer -= TimeBetweeenLetters;
                transform.GetChild(charAvailable++).gameObject.SetActive(true);

                if(charAvailable >= transform.childCount)
                {
                    UseTypewritter = false;
                }
            }
        }
    }
}
