using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class SpinBonusItem : MonoBehaviour
    {
        public List<Sprite> sprites;
        [SerializeField] private Image img;

        public Sprite ItemSprite
        {
            get
            {
                return img.sprite;
            }
        }
        public void Init(int value)
        {
            switch (value)
            {
                case 2:
                    img.sprite = sprites[0];
                    break;
                case 3: 
                    img.sprite = sprites[1];
                    break;
                case 4: 
                    img.sprite = sprites[2];
                    break;
                case 5: 
                    img.sprite = sprites[3];
                    break;
            }
            img.SetNativeSize();
        }
        
    }
}