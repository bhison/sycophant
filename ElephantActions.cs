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
}
