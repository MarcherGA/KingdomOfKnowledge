using System.Security.Cryptography;
using UnityEngine;

public class ResizeToScreen : MonoBehaviour
{
    private Resolution _res;

    private void Start()
    {
        Resize();
    }
    void Resize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width + 0.001f;
        transform.localScale = xWidth;
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height + 0.001f;
        transform.localScale = yHeight;
        //transform.localScale.y = worldScreenHeight / height; 
    }
    void Update()
    {

        if (_res.height != Screen.currentResolution.height || _res.width != Screen.currentResolution.width)
        {

            //Do stuff

            _res = Screen.currentResolution;

        }

    }
}
