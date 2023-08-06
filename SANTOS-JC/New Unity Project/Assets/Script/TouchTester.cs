using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTester : MonoBehaviour
{
    public Sprite Idle;
    public Sprite Stationary;
    public Sprite Ended;
    public Sprite Pressed;

    private SpriteRenderer _spriteRender;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int touches = Input.touchCount;
        if(touches > 0)
        {
            Touch t = Input.GetTouch(0);
            Debug.Log($"Touch: {t.phase}");


            if(t.phase != TouchPhase.Ended)
            {
                Ray r = Camera.main.ScreenPointToRay(t.position);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(r, out hit, Mathf.Infinity))
                {
                    _spriteRender.sprite = Pressed;
                }
                else
                {
                    _spriteRender.sprite = Idle;
                }

            }



            else
            {
                _spriteRender.sprite = Idle;
            }

            /*switch (t.phase)
            {
                case TouchPhase.Began:
                    _spriteRender.sprite = Idle; break;
                case TouchPhase.Stationary:
                    _spriteRender.sprite = Stationary; break;
                case TouchPhase.Moved:
                    _spriteRender.sprite = Pressed; break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    _spriteRender.sprite = Ended; break;

            }*/

        }
        
    }

    private void OnDrawGizmos()
    {
        int touches = Input.touchCount;
        if(touches > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                Ray r = Camera.main.ScreenPointToRay(t.position);

                

                switch (t.fingerId)
                {
                    case 0:
                        Gizmos.DrawIcon(r.GetPoint(10), "rushia");break;
                    case 1:
                        Gizmos.DrawIcon(r.GetPoint(10), "ayame"); break;
                    case 2:
                        Gizmos.DrawIcon(r.GetPoint(10), "fubuki"); break;
                    case 3:
                        Gizmos.DrawIcon(r.GetPoint(10), "cali"); break;
                    case 4:
                        Gizmos.DrawIcon(r.GetPoint(10), "marine"); break;
                    default:
                        Gizmos.DrawIcon(r.GetPoint(10), "ina"); break;

                }
            }
            
        }
    }
}
