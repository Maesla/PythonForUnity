using System.IO;
using UnityEngine;

namespace MicrokernelSystem
{
    public class ICon : IICon
    {
        public string Path { get; private set; }

        public Texture2D Texture { get; private set; }

        public Sprite Sprite { get; private set; }

        public ICon(string iconPath)
        {
            Path = iconPath;
            Load();
        }

        private void Load()
        {
            if (File.Exists(Path))
            {
                byte[] fileData;
                fileData = File.ReadAllBytes(Path);
                Texture = new Texture2D(2, 2);
                Texture.LoadImage(fileData);

                Sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
