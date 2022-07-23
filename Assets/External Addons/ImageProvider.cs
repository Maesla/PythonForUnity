using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MicrokernelSystem.Addons.External
{
    public class ImageProvider : BasePythonAPIProvider
    {
        private Image sourceImage;
        public ImageProvider([Inject(Id = "sourceImage")] Image img)
        {
            sourceImage = img;
        }

        public void SetImage(string base64img)
        {
            sourceImage.sprite = ConvertImage(base64img);
        }

        private Sprite ConvertImage(string base64img)
        {
            byte[] imageBytes = Convert.FromBase64String(base64img);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            return sprite;
        }

    } 
}
