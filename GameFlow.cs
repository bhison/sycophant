using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    enum Interactions{
        hello,
        getHat,
        getShirt,
        getShoes,
        makeJoke,
        changeTemp,
        changeMusic
    }
    public class GameFlow : MonoBehaviour
    {
        private ElephantActions _elephantActions;
        private List<Interactions> interactions;

        private void Awake()
        {
            _elephantActions = GetComponent<ElephantActions>();
        }

        public void DoGameStuff()
        {
            var gm = GameManager.Instance;
            var elephant = ElephantController.Instance;
            
            if (interactions.Count == 0)
            {
                _elephantActions.ElephantStart();
                interactions.Add(Interactions.hello);
                return;
            }

            else
            {
                if (elephant.isSpeaking) return;
                if (elephant.isBusy) return;
            }

            var numClothesDone = 0;
            var haveReqdHat = interactions.Contains(Interactions.getHat);
            if (haveReqdHat) numClothesDone++;
            var haveReqdShirt = interactions.Contains(Interactions.getShirt);
            if (haveReqdShirt) numClothesDone++;
            var haveReqdShoes = interactions.Contains(Interactions.getShoes);
            if (haveReqdShoes) numClothesDone++;
            var haveDoneTemp = interactions.Contains(Interactions.changeTemp);
            var haveDoneMusic = interactions.Contains(Interactions.changeMusic);

            if (interactions.Count == 1)
            {
                var clothes = ClothesType.Hat;
                var describer = Random.value > 0.5f ? Describer.Comfy : Describer.Stylish;
                _elephantActions.AskForClothes(clothes, describer );
                elephant.wantedClothesType = clothes;
                elephant.wantedDescriber = describer;
                interactions.Add(Interactions.getHat);
                return;
            }

            if (interactions.Count == 2)
            {
                _elephantActions.TellFunnyAnecdote();
                interactions.Add(Interactions.makeJoke);
            }

            if (interactions.Count == 3)
            {
                if (Random.value > 0.6f)
                {
                    _elephantActions.RequestTemperatureChange(Random.value >0.5f ? Temperature.Cold : Temperature.Hot); 
                    interactions.Add(Interactions.changeTemp);
                    return;
                }
                else
                {
                    _elephantActions.RequestMusicChange(Random.value > 0.5f ? MusicType.Trance : MusicType.PopBanger);
                    interactions.Add(Interactions.changeMusic);
                    return;
                }
            }

            if (interactions.Count == 4)
            {
                var clothes = ClothesType.Shoes;
                var describer = Random.value > 0.5f ? Describer.Stylish : Describer.Warm;
                _elephantActions.AskForClothes(clothes, describer );
                elephant.wantedClothesType = clothes;
                elephant.wantedDescriber = describer;
                interactions.Add(Interactions.getShoes);
                return;
            }

            if (interactions.Count == 5)
            {
                _elephantActions.Goodbye();
                
            }
        }
        IEnumerator EndGameAfterSeconds(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameManager.Instance.GameRunTime = 999;
        }
    }

    
}