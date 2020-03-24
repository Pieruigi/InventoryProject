using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OMTB.Utility
{
    public class SpriteUtil
    {
        public static List<Sprite> GetSprites(Texture2D source, int cols, int rows)
        {
            List<Sprite> sprites = new List<Sprite>();
           
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < cols; j++)
                {
                    //Sprite newSprite = Sprite.Create(source, new Rect(j * sW, i * sH, sW, sH), Vector2.one / 2f);
                    sprites.Add(GetSprite(source, cols, rows, j, i));
                }
            }

            return sprites;
        }

        public static Sprite GetSprite(Texture2D source, int cols, int rows, int x, int y)
        {
            if (source == null || cols <= 0 || rows <= 0 || x < 0 || y < 0 || x >= cols || y >= rows)
                throw new Exception(string.Format("Wrong parameters: source:{0}, cols:{1}, rows:{2}, x:{3}, y:{4}.", source, cols, rows, x, y));

            float sW = source.width / cols;
            float sH = source.height / rows;


            y = rows - 1 - y;

            return Sprite.Create(source, new Rect(x * sW, y * sH, sW, sH), Vector2.one / 2f);
        }

    }

}
