using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSC : MonoBehaviour
{
 
    public GameObject playerPrefab;
    public GameObject brickPrefab;
    public GameObject upperBound;
    public GameObject lowerBound;
    public GameObject rightBound;
    public GameObject leftBound;
    private Camera cam;
    private List<BrickTypeSC> _listOfBricks;

    private void BoundSetup()
    {   
        Renderer rend = upperBound.GetComponent<Renderer>();
        Vector3 boundSize = rend.bounds.size;
        Vector3 boundScale;

        //upperBound.GetComponent<MeshRenderer>().enabled = false;
        //lowerBound.GetComponent<MeshRenderer>().enabled = false;
        //rightBound.GetComponent<MeshRenderer>().enabled = false;
        //leftBound.GetComponent<MeshRenderer>().enabled = false;

        if (cam)
        {
            float zDist = Mathf.Abs(cam.transform.position.z);
            float xScale = cam.ViewportToWorldPoint(new Vector3(1, 0, zDist)).x - cam.ViewportToWorldPoint(new Vector3(0, 0, zDist)).x;
            float yScale = cam.ViewportToWorldPoint(new Vector3(0, 1, zDist)).y - cam.ViewportToWorldPoint(new Vector3(0, 0, zDist)).y;

            float worldUp = cam.ViewportToWorldPoint(new Vector3(0, 1, zDist)).y;
            float worldDown = cam.ViewportToWorldPoint(new Vector3(0, 0, zDist)).y;
            float worldLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zDist)).x;
            float worldRight = cam.ViewportToWorldPoint(new Vector3(1, 0, zDist)).x;

            boundScale = upperBound.transform.localScale;
            boundScale.x = xScale;
            upperBound.transform.localScale = boundScale;
            upperBound.transform.position = new Vector3(0, worldUp + boundSize.y / 2, 0);

            boundScale = lowerBound.transform.localScale;
            boundScale.x = xScale;
            lowerBound.transform.localScale = boundScale;
            lowerBound.transform.position = new Vector3(0, worldDown - boundSize.y / 2, 0);

            boundScale = leftBound.transform.localScale;
            boundScale.y = yScale;
            leftBound.transform.localScale = boundScale;
            leftBound.transform.position = new Vector3(worldLeft - boundSize.x / 2, 0 , 0);

            boundScale = rightBound.transform.localScale;
            boundScale.y = yScale;
            rightBound.transform.localScale = boundScale;
            rightBound.transform.position = new Vector3(worldRight + boundSize.x / 2, 0 , 0);


        }
    }

    void RandomCubeBoard(float prob )
    {
        float stdBrickSize = 2f * brickPrefab.transform.localScale.x;
        BrickTypeSC brickSc;
        _listOfBricks = new List<BrickTypeSC>();

        for (int i = 0; i < 5; i ++)
        {
            if (Random.Range(0, 1f) < prob)
            {
                brickSc = Instantiate(brickPrefab, new Vector3(0 + i * stdBrickSize, 0, 0 ), Quaternion.identity).GetComponent<BrickTypeSC>();
                _listOfBricks.Add(brickSc);              
            }

            if (Random.Range(0, 1f) < prob)
            {
                brickSc = Instantiate(brickPrefab, new Vector3(0 + i * stdBrickSize,  stdBrickSize, 0 ), Quaternion.identity).GetComponent<BrickTypeSC>();
                _listOfBricks.Add(brickSc);
            }

            if (Random.Range(0, 1f) < prob)
            {
                brickSc = Instantiate(brickPrefab, new Vector3(0 + i * stdBrickSize,  -stdBrickSize, 0 ), Quaternion.identity).GetComponent<BrickTypeSC>();
                _listOfBricks.Add(brickSc);
            }
        }
    }

    private void Awake() 
    {
        cam = Camera.main;
        BoundSetup();
        GameObject PlayerInst = Instantiate(playerPrefab, - 3 * Vector3.right, Quaternion.identity);
        PlayerSC Player = PlayerInst.GetComponent<PlayerSC>();
        Player.ScaleTo(.5f);
        RandomCubeBoard(.7f);
    }
    void Start()
    {

    }


    void Update()
    {
        
    }
}
