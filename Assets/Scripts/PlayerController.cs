using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxX;

    [Header(" Control Settings ")]
    private float clickedScreenX;
    private float clickedPlayerX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          ManageControl();
    }
    private void ManageControl(){
        float xDifference, newXposition;

        if(Input.GetMouseButtonDown(0)){
            clickedScreenX=Input.mousePosition.x;
            clickedPlayerX=transform.position.x;
        }else if(Input.GetMouseButton(0)){
            xDifference = Input.mousePosition.x - clickedScreenX;
            xDifference /= Screen.width;
            xDifference *= moveSpeed;
            newXposition = Mathf.Clamp(clickedPlayerX+xDifference,-maxX, maxX);
            transform.position = new Vector2(newXposition,transform.position.y);
        }
    }
}
