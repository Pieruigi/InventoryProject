using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _testMultiSprite : MonoBehaviour
{
    [SerializeField]
    Texture2D source;

    Image image;

    List<Sprite> sprites = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        Vector2 size = new Vector2(source.width, source.height);
        Debug.Log("TexSize:" + size);




        image.sprite = OMTB.Utility.SpriteUtil.GetSprite(source, 2, 3, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
