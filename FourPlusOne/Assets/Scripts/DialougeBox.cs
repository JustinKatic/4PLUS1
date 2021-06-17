using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeBox : MonoBehaviour
{
    public BoxCollider TextSpace;
    public Text3D TextElement;

    public Rigidbody BoxBody;

    [Min(0.2f)]
    public float TimeBeforeNextMessage = 2f;

    public List<string> Messages;
    private int messageIndex = 0;

    private float BetweenMessageTimer = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        BetweenMessageTimer = 0;
        messageIndex = 0;
        if (TextSpace != null)
        {
            if(TextElement != null)
            {
                //Accuratly place text object
                //TextElement.MaxWidth = TextSpace.bounds.size.x * 0.9f;

                if(Messages.Count > 0)
                TextElement.Text = Messages[0];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TextElement.TypeWrittingDone)
        BetweenMessageTimer += Time.deltaTime;

        if(BetweenMessageTimer >= TimeBeforeNextMessage)
        {
            messageIndex++;
            if (messageIndex >= Messages.Count)
            {
                enabled = false;
            }
            else
            {
                BetweenMessageTimer = 0;
                TextElement.Text = Messages[messageIndex];
            }
        }
    }

    public void InterupDialouge()
    {
        TextElement.UseTypewritter = false;
        enabled = false;

        if(BoxBody != null)
        {
            BoxBody.isKinematic = false;
            BoxBody.freezeRotation = false;
        }

        //Highly inefficent code
        Rigidbody[] textBodies = TextElement.GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < textBodies.Length; i++)
        {
            textBodies[i].isKinematic = false;
            textBodies[i].useGravity  = false;
        }
    }
}
