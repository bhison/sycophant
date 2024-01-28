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

        private float tickLength = 3f;
        private float timeTilNextTick = 0;

        private void Awake()
        {
            _elephantActions = GetComponent<ElephantActions>();
        }

        public void DoGameStuff()
        {
            var gm = GameManager.Instance;
            var elephant = ElephantController.Instance;

            if (gm.patienceLeftForTask > 0)
            {
                gm.patienceLeftForTask -= Time.deltaTime;
                if (gm.patienceLeftForTask < 0)
                {
                    if (elephant.wantedClothesType != null)
                    {
                        _elephantActions.CancelClothesRequest();
                    }
                    else
                    {
                        _elephantActions.CancelOtherReq();
                    }

                    return;
                }
            }
            
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
                timeTilNextTick -= Time.deltaTime;
                if (timeTilNextTick > 0) return;
            }

            timeTilNextTick = tickLength;

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

            if (interactions.Count is 2 or 5)
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

                _elephantActions.RequestMusicChange(Random.value > 0.5f ? MusicType.Trance : MusicType.PopBanger);
                interactions.Add(Interactions.changeMusic);
                return;
            }

            if (interactions.Count == 4)
            {
                var clothes = Random.value > 0.5f ? ClothesType.Shoes : ClothesType.Hat;
                var describer = Random.value > 0.5f ? Describer.Stylish : Describer.Warm;
                _elephantActions.AskForClothes(clothes, describer );
                elephant.wantedClothesType = clothes;
                elephant.wantedDescriber = describer;
                interactions.Add(clothes == ClothesType.Shoes ? Interactions.getShoes : Interactions.getHat);
                return;
            }
            
            if (interactions.Count == 6)
            {
                _elephantActions.Goodbye();
                StartCoroutine(EndGameAfterSeconds(35));
            }
        }
        IEnumerator EndGameAfterSeconds(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameManager.Instance.EndGame();
        }
    }

    
}