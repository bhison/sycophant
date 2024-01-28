using System;
using System.IO;
using UnityEngine;


/**
 * Context:
 * Guidance:
 * LookingForALaugh:
 */

public static class PromptUtilities
{
    public static string ClothesTypeToString (ClothesType clothesType) {
        var toString = clothesType switch
        {
            ClothesType.Hat => "a hat",
            ClothesType.Shirt => "a shirt",
            ClothesType.Shoes => "some shoes",
            _ => throw new ArgumentOutOfRangeException(nameof(clothesType), clothesType, null)
        };
        return toString;
    }
}

public static class ConfigStatements
{
    public static string Setup = "You are a character in a fun video game. You are an elephant who is currently in a shop doing some clothes shopping. All responses you give are in the register and format of an elephant saying a thing to a person. The response should just be what you say. The length of responses should never be more than thirty words. Do not include anything in your response apart from words and punctuation that should be said. You are posh, pompous and entitled but kind of lovable. It's good if you acknowledge that you are an elephant you will be told what has happened in the game and be given guidance on what should be said, and whether you are looking for a laugh. the format will be - Context: what has happened. Guidance: what is needed to be stated for narrative purposes. LookingForALaugh: true or false - if you are LookingForALaugh you should end on a pun or even laughing out loud so it is explicitly clear you are looking to make the person laugh";

    public static string ExampleSituationPrompt = "An example prompt and a good response could be ..." +
                                                 "/Context: you have bought a shirt and want to spend more" +
                                                 "/Guidance: you would like the assistant to give you a hat to try on to keep you warm" +
                                                 "/LookingForALaugh: false" +
                                                 " ...and your response could be 'I loved that shirt, now I need a hat to match but it must be warm. I'm almost perfect but I do get a chilly head! Haha!'";
}

public static class ContextPrompts
{
    public static string LongTime = "you have been shopping for a long time";
    public static string ShortTime = "you have been shopping for a short time";
    public static string Hungry = "you are hungry";
    
    

    public static string AskedFor(ClothesType clothesType, Describer describer)
    {
        var ender = PromptUtilities.ClothesTypeToString(clothesType);

        return "you have asked the assistant to bring you " + 
               clothesType + 
               ",  which you have specified must be " +
               describer;
    }
    
    public static string BeenGiven(bool correctly, ClothesType clothesType, Describer describer)
    {
        if (correctly) return "and thankfully they have given you just that";

        var ender = PromptUtilities.ClothesTypeToString(clothesType);
        return "but the assistant has incorrectly brought you " + clothesType + "which looks to be " + describer + ", which you are annoyed by";
    }

    public static string TempChanged(Temperature newTemp, Temperature reqTemp)
    {
        var returnString = "you asked for the temperature to be turned " +
                           (reqTemp == Temperature.Cold ? "down" : "up ");
        returnString += (reqTemp == newTemp)
            ? " and the assistant has done this which you are thankful for"
            : " but the assistant has made it worse, what a fool...";
        return returnString;
    }

    public static string MusicChanged(MusicType newType, MusicType reqType)
    {
        var returnString = "you asked for the music to be changed to " + (reqType);
        returnString += newType == reqType
            ? " and the assistant has done that perfectly, what a good person!"
            : " but the philistine has switched it to " + newType + " - which you hate and should explain why.";
        return returnString;
    }
    
    public static string LaughingAt(bool isLaughing, bool believably, bool youWantThemToLaugh)
    {
        if (!isLaughing)
        {
            return youWantThemToLaugh
                ? "you made a joke but the assistant is not laughing which is very rude"
                : "the assistant is taking you seriously, which is good as you're being very serious.";
        }

        return youWantThemToLaugh
            ? "the assistant is laughing at your good humour " + (believably
                ? "in a pleasant appreciative way"
                : "but in a clearly fake and condescending way")
            : "the assistant is laughing at you! how offensive!";
    }

    public static string AssistantInfo(string name)
    {
        return "The assistant is called " + name;
    }
}

public static class Guidance
{
    public static string YouWantClothes(ClothesType clothesType, Describer describer)
    {
        return "You should ask the assistant to provide you with " + PromptUtilities.ClothesTypeToString(clothesType) + " that is " + describer;
    }

    public static string YouWantTemp(Temperature temperature)
    {
        return temperature == Temperature.Cold 
            ? "you're feeling a bit stuffy, ask the assistant to cool the place down" 
            : "you're feeling a bit chilly, ask the assistant to turn up the heating";
    }

    public static string YouWantMusic(MusicType musicType)
    {
        return "you want to listen to some " + musicType + " music, ask the assistant to change the radio to your preferred genre";
    }

    public static string ExpressYourself()
    {
        return "Let the assistant know your very important feelings!";
    }
}
