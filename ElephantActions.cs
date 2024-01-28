using System;
using System.Collections.Generic;
using GenerativeAudio;
using UnityEngine;
using UnityEngine.Serialization;

public class ElephantActions : MonoBehaviour
{

    [SerializeField] private float rapportToPatienceDivider = 50;
    [SerializeField] private float rapportToMoneyMultiplier = 10;
    [SerializeField] private float badClothesPenalty = 40;
    [SerializeField] private float badOtherPenalty = 30;

    [SerializeField] private float LaughCorrectBonus = 25;
    [SerializeField] private float LaughWrongPenalty = 40;
    
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
        ElephantController.Instance.isBusy = true;
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
            var correct = wantedClothes == recClothesType && wantedDesc == recDescriber;
            contexts.Add(ContextPrompts.AskedFor(wantedClothes, wantedDesc));
            contexts.Add(ContextPrompts.BeenGiven(
                    correct,
                    recClothesType, recDescriber
                ));
            if (correct)
            {
                GameManager.Instance.ChangeRapport(
                    (GameManager.Instance.patience - GameManager.Instance.patienceLeftForTask)/rapportToPatienceDivider);
               var amount =  GameManager.Instance.RapportPercent * rapportToMoneyMultiplier;
                contexts.Add("Thank the assistant and give them a tip of £" + (amount));
                GameManager.Instance.AddTip(amount);
            }
            else
            {
                GameManager.Instance.ChangeRapport(-badClothesPenalty);
            }
        }
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = contexts.ToArray(), Guidance = "React to receiving the item", LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);

        ElephantController.Instance.ResetWants();
        ElephantController.Instance.isBusy = false;


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
        ElephantController.Instance.wantedResponseExpectation = ResponseExpectation.Laugh;
        ElephantController.Instance.isBusy = true;
    }

    public void OffendedAtLackOfLaughter()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You just said something funny" },
            Guidance = "Express your disappointment at the lack of laughter at your wonderful humor",
            LookingForALaugh = false
        };
        GameManager.Instance.ChangeRapport(-LaughWrongPenalty);
        SaySomething.Instance.Speak(dialogueParameters);
        ElephantController.Instance.wantedResponseExpectation = ResponseExpectation.Neutral;
        ElephantController.Instance.isBusy = false;
    }

    public void AppreciateLaughter()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You appreciate the assistant's laughter" },
            Guidance = "Express your appreciation for the assistant's laughter",
            LookingForALaugh = false
        };
        GameManager.Instance.ChangeRapport(LaughCorrectBonus);

        SaySomething.Instance.Speak(dialogueParameters);
        ElephantController.Instance.isBusy = false;
    }

    public void ExpressRapportLevel()
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

    public void RequestMusicChange(MusicType changeTo)
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You want to change the music" },
            Guidance = Guidance.YouWantMusic(changeTo),
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
        ElephantController.Instance.wantedMusicType = changeTo;
        ElephantController.Instance.isBusy = true;
    }

    public void RequestTemperatureChange(Temperature temp)
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You want to change the temperature" },
            Guidance = Guidance.YouWantTemp(temp),
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
        ElephantController.Instance.wantedTemperature = temp;
        ElephantController.Instance.isBusy = true;
    }

    public void MusicHasChanged(MusicType changedTo)
    {
        var positive = changedTo == ElephantController.Instance.wantedMusicType;
        if (positive)
        {
            GameManager.Instance.ChangeRapport((GameManager.Instance.patience - GameManager.Instance.patience) / rapportToPatienceDivider);
            DialogueParameters dialogueParameters = new DialogueParameters
            {
                Context = new string[] { "The music has changed to " + changedTo.ToString() },
                Guidance = "you are thankful the music is now " + changedTo.ToString(),
                LookingForALaugh = false
            };
            SaySomething.Instance.Speak(dialogueParameters);
        }
        else
        {
            GameManager.Instance.ChangeRapport(-badOtherPenalty);
            DialogueParameters dialogueParameters = new DialogueParameters
            {
                Context = new string[] { "The music has changed to or remained as " + changedTo.ToString() },
                Guidance = "you are upset the music is " + changedTo.ToString() + " as you wanted " + ElephantController.Instance.wantedMusicType.ToString() + ", which is much better",
                LookingForALaugh = false
            };
            SaySomething.Instance.Speak(dialogueParameters);
        }
        ElephantController.Instance.ResetWants();
        ElephantController.Instance.isBusy = false;
    }

    public void TemperatureHasChanged(Temperature temperature)
    {
        if (ElephantController.Instance.wantedTemperature == temperature)
        {
            DialogueParameters dialogueParameters = new DialogueParameters
            {
                Context = new string[] { "The temperature has changed to " + temperature.ToString() },
                Guidance = "you are thankful the temperature is now " + temperature.ToString(),
                LookingForALaugh = false
            };
            SaySomething.Instance.Speak(dialogueParameters);
        }
        else
        {
            DialogueParameters dialogueParameters = new DialogueParameters
            {
                Context = new string[] { "The temperature has changed to or remained as " + temperature.ToString() },
                Guidance = "you are upset the temperature is " + temperature.ToString() + " as you wanted " +
                           ElephantController.Instance.wantedTemperature.ToString(),
                LookingForALaugh = false
            };
            SaySomething.Instance.Speak(dialogueParameters);
        }
        ElephantController.Instance.ResetWants();
        ElephantController.Instance.isBusy = false;
    }

    public void Goodbye()
    {
        var rapport = GameManager.Instance.RapportPercent;
        var goodbyeMessage = rapport > 0.7f ? "You say goodbye to the assistant and tell them how much you love them." :
            rapport > 0.3f ? "You say goodbye to the assistant and are just a bit meh about it" :
            "You say goodbye to the assistant and tell them off for being not very good customer service";
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { goodbyeMessage },
            Guidance = "Say goodbye to the assistant",
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
        
    }

    public void CancelClothesRequest()
    {
        var wanted = ElephantController.Instance.wantedDescriber + " " + ElephantController.Instance.wantedMusicType;
        if (ElephantController.Instance.wantedDescriber != null && ElephantController.Instance.wantedMusicType != null)
        {
            DialogueParameters dialogueParameters = new DialogueParameters
            {
                Context = new string[] { "You cancel your request for " + wanted },
                Guidance = "Cancel your request for " + wanted + " as you've lost interest",
                LookingForALaugh = false
            };
            SaySomething.Instance.Speak(dialogueParameters);
        }
        GameManager.Instance.ChangeRapport(-badClothesPenalty);
        ElephantController.Instance.ResetWants();
        ElephantController.Instance.isBusy = false;
    }

    public void CancelOtherReq()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You cancel your request" },
            Guidance = "tell the assistant to not bother and you don't care anymore. you are annoyed.",
            LookingForALaugh = false
        };
        ElephantController.Instance.ResetWants();
        ElephantController.Instance.isBusy = false;
        GameManager.Instance.ChangeRapport(-badOtherPenalty);
        SaySomething.Instance.Speak(dialogueParameters);
    }
    
    public void MinorAmusement()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "the assistant laughed at something you said which you like" },
            Guidance = "React with minor amusement, only a couple of words",
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }

    public void MinorGrump()
    {
        DialogueParameters dialogueParameters = new DialogueParameters
        {
            Context = new string[] { "You express a minor grump" },
            Guidance = "React with a minor grump, , only a couple of words",
            LookingForALaugh = false
        };
        SaySomething.Instance.Speak(dialogueParameters);
    }
}
