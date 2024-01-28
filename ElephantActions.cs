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
        var guidance = Guidance.YouWantClothes(clothesType, describer);
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context =
                new string[] { "You want some more clothes" },
            Guidance = guidance,
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void ReceiveItem(ClothesType recClothesType, Describer recDescriber)
    {
        if (ElephantController.Instance.wantedClothesType == null) return;
        if (ElephantController.Instance.wantedDescriber == null) return;
        
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

    public void TellFunnyAnecdote()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "Tell a funny anecdote" },
            Guidance = "Tell the assistant a funny anecdote",
            LookingForALaugh = true
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void OffendedAtLackOfLaughter()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You just said something funny" },
            Guidance = "Express your disappointment at the lack of laughter at your wonderful humor",
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void ExpressRapport()
    {
        var rapport = GameManager.Instance.RapportPercent;
        var context = rapport > 0.7f
            ? "you are enjoying the assistant's company"
            : rapport > 0.3f
                ? "you are tolerating the assistant."
                : "you don't like the assistant.";
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { context },
            Guidance = Guidance.ExpressYourself(),
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }
}
