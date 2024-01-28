using UnityEngine;

namespace DefaultNamespace
{
    public class DebugDoTasks:MonoBehaviour
    {
        private ElephantActions _elephantActions;

        public void GiveComfyShoes()
        {
            _elephantActions.ReceiveItem(ClothesType.Shoes, Describer.Comfy);
        }
        public void GiveStylishShoes()
        {
            _elephantActions.ReceiveItem(ClothesType.Shoes, Describer.Stylish);
        }
        public void GiveWarmShoes()
        {
            _elephantActions.ReceiveItem(ClothesType.Shoes, Describer.Warm);
        }
        public void GiveComfyShirt()
        {
            _elephantActions.ReceiveItem(ClothesType.Shirt, Describer.Comfy);
        }
        public void GiveStylishShirt()
        {
            _elephantActions.ReceiveItem(ClothesType.Shirt, Describer.Stylish);
        }
        public void GiveWarmShirt()
        {
            _elephantActions.ReceiveItem(ClothesType.Shirt, Describer.Warm);
        }
        public void GiveComfyHat()
        {
            _elephantActions.ReceiveItem(ClothesType.Hat, Describer.Comfy);
        }
        public void GiveStylishHat()
        {
            _elephantActions.ReceiveItem(ClothesType.Hat, Describer.Stylish);
        }
        public void GiveWarmHat()
        {
            _elephantActions.ReceiveItem(ClothesType.Hat, Describer.Warm);
        }

        public void MusicChangedToEasy()
        {
            _elephantActions.MusicHasChanged(MusicType.EasyListening);
        }

        public void MusicChangedToTrance()
        {
            _elephantActions.MusicHasChanged(MusicType.Trance);
        }

        public void MusicChangedToPop()
        {
            _elephantActions.MusicHasChanged(MusicType.PopBanger);
        }

        public void TemperatureHasGoneUp()
        {
            _elephantActions.TemperatureHasChanged(Temperature.Hot);
        }
        public void TemperatureHasGoneDown()
        {
            _elephantActions.TemperatureHasChanged(Temperature.Cold);
        }

        public void RotateToNextMusicType()
        {
            var currentMusicType = GameManager.Instance.currentMusicType;
            if (currentMusicType == MusicType.EasyListening)
            {
                GameManager.Instance.currentMusicType = MusicType.Trance;
                AudioController.Instance.PlayMusic(MusicType.Trance);
            }
            if (currentMusicType == MusicType.Trance)
            {
                GameManager.Instance.currentMusicType = MusicType.PopBanger;
                AudioController.Instance.PlayMusic(MusicType.PopBanger);
            }
            if (currentMusicType == MusicType.PopBanger)
            {
                GameManager.Instance.currentMusicType = MusicType.EasyListening;
                AudioController.Instance.PlayMusic(MusicType.EasyListening);
            }
        }
    }
}