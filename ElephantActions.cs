using System;
using System.Collections.Generic;
using GenerativeAudio;

public class ElephantActions
{
    public void ElephantStart()
    {
        DialogueParameters dialogueParameters =
            new DialogueParameters
            {
                Context = new string[] { "You have just entered the shop and are looking to buy some things" },
                Guidance = "Greet the assistant by name and crack a joke",
                LookingForALaugh = true
            };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void AskForClothes(ClothesType clothesType, Describer describer)
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context =
                new string[] { "You want some " + clothesType.ToString() + " that are " + describer.ToString() },
            Guidance = "Ask the assistant if they have any " + clothesType.ToString() + " that are " +
                       describer.ToString(),
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void ReceiveItem(ClothesType recClothesType, Describer recDescriber)
    {
        var wantedClothes = (ClothesType)ElephantController.Instance.wantedClothesType;
        var wantedDesc = (Describer)ElephantController.Instance.wantedDescriber;
        var contexts = new List<string>();
        if (
            ElephantController.Instance.wantedClothesType != null &&
            ElephantController.Instance.wantedDescriber != null
        )
        {
            contexts.Add(ContextPrompts.AskedFor(wantedClothes, wantedDesc));
            contexts.Add(ContextPrompts.BeenGiven(
                    wantedClothes == recClothesType && wantedDesc == recDescriber,
                    recClothesType, recDescriber
                ));
        }
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = contexts.ToArray(), Guidance = "React to receiving the item", LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void GiveTip()
    {
        var amount = GameManager.Instance.RapportPercent * 10;
        GameManager.Instance.AddTip(amount);
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "the assistant gave you what you wanted" },
            Guidance = "Thank the assistant and give them a tip of £" + (amount),
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }
}
