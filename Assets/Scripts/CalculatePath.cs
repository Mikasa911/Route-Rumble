using System;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePath : MonoBehaviour
{
    public LayerMask obstacles;
    public LayerMask rockLayer;
    public LayerMask interactables;
    public LayerMask blankSpaceLayer;

    public ButtonHoldDetector buttonHoldDetector;

    Vector2 mousePos;
    Vector2 playerpos;

    GamePlayCanvas gameplayCanvas;
    [SerializeField] TreasureChest chest;
    [SerializeField] PlayerMovement player;

    public int totalsteps=0,lastrock;
    List<Vector2> lastpath = null;
    List<Vector2> path = null;
    List<List<Vector2>> pathHistory = new List<List<Vector2>>();
    List<Mushroom> RocksHistory = new List<Mushroom>();
     List<int> RockIndexes = new List<int>();
    public Vector2 previousPos;
    Vector2 targetGridPos;

    bool PathExists;
    bool canCalculatePath;
    bool isRetracing=false;
    bool TracedPath;
    bool Tapped;
    bool isSwiping;
    bool iswalking;

    Vector2 initMousePos;
    Vector2 finalMousePos;

    private void Awake()
    {
        gameplayCanvas = FindObjectOfType<GamePlayCanvas>();
    }
    private void Update()
    {

        if(Tapped==true)
        {  
            Tapped = false;
            Debug.Log("Tap");
            if (player.transform.position == player.target && path == null && player.myRigidBody.velocity == Vector2.zero && !gameplayCanvas.GamePaused)
            {               
                if (!canCalculatePath)
                {
                    playerpos = new Vector2(player.transform.position.x, player.transform.position.y);
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetGridPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));
                    rockcheck();
                    
                    if (PathExists)
                    {
                        PathExists = false;
                        path = PathFinder.FindPath(playerpos, targetGridPos, obstacles, rockLayer, interactables, blankSpaceLayer, chest.Key);
                        previousPos = playerpos;
                        if (path != null)
                        {
                            Debug.Log("one");
                            path.RemoveAt(0);
                            pathHistory.Add(PathFinder.FindPath(playerpos,
                                targetGridPos, obstacles, rockLayer, interactables,
                                blankSpaceLayer, chest.Key));
                        }

                    }
                    else
                    {
                        Debug.Log("No path found.");
                        return;
                    }
                }
                else if (totalsteps != 0)
                {
                    lastpath = pathHistory[pathHistory.Count - 1];
                    if (lastpath != null)
                    {
                        lastpath.Reverse();
                        {
                            isRetracing = true;
                            if (lastpath.Count != 1)
                            {
                                lastpath.RemoveAt(0);
                            }
                            path = lastpath;
                            pathHistory.RemoveAt(pathHistory.Count - 1);

                        }
                    }
                }
                TracedPath = false;
            }
        } 
        if(iswalking==false)
        {
            if (Input.GetMouseButtonUp(0))
            {
                finalMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 distance = new Vector2(finalMousePos.x - initMousePos.x,
                    finalMousePos.y - initMousePos.y);

                Vector2 movedirection;
                if (Mathf.Abs(distance.x) < MathF.Abs(distance.y))
                {
                    movedirection = new Vector2(0, distance.y).normalized;
                }
                else
                {
                    movedirection = new Vector2(distance.x, 0).normalized;
                }
                Debug.Log(movedirection);
                Debug.Log(distance);
                if ((Mathf.Abs(distance.x) < .2 && Mathf.Abs(distance.y) < .2))
                {
                    Debug.Log("tap");
                    Tapped = true;
                }
                else
                {
                    isSwiping = true;
                    playerpos = player.transform.position;
                    targetGridPos = playerpos + movedirection;
                    rockcheck();
                    isSwiping = false;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                canCalculatePath = buttonHoldDetector.IsButtonHeld;
            }
        }

        if (!TracedPath)
        {
            TracePath(ref path);
        }
        if(player.myRigidBody.velocity==Vector2.zero && path==null)
        {
            iswalking = false;
        }
    }
    void rockcheck()
    {
        if ((Physics2D.OverlapCircle(targetGridPos, 0.1f,
                         rockLayer)))
        {
            Debug.Log("Clicked on rock");
            if (((Mathf.Abs(playerpos.x - targetGridPos.x) < 1.2f) && (Mathf.Abs(playerpos.y - targetGridPos.y) < 0.2f))
                || ((Mathf.Abs(playerpos.y - targetGridPos.y) < 1.2f) && (Mathf.Abs(playerpos.x - targetGridPos.x) < 0.2f)))
            {
                Debug.Log("More than one tile");
                player.target = targetGridPos;
                Collider2D lastrock = Physics2D.OverlapCircle(targetGridPos, 0.1f, rockLayer);
                if (lastrock.isTrigger == false)
                {
                    totalsteps++;
                    Debug.Log(totalsteps);
                    RockIndexes.Add(totalsteps - 1);
                    RocksHistory.Add(lastrock.gameObject.GetComponent<Mushroom>());

                }
                else
                {
                    totalsteps++;
                }

                pathHistory.Add(new List<Vector2> { playerpos });
                return;
            }
            else if (Physics2D.OverlapCircle(targetGridPos, 0.3f, blankSpaceLayer))
            {
                Debug.Log("Clicked on void");
                PathExists = true;
            }
        }
        else if (Physics2D.OverlapCircle(targetGridPos, 0.3f, obstacles) ||
                      Physics2D.OverlapCircle(targetGridPos, 0.3f, blankSpaceLayer) || (Physics2D.OverlapCircle(targetGridPos, 0.3f,
                      interactables) && chest.Key == false) || playerpos == targetGridPos)
        {
            PathExists = false;
            Debug.Log("Clicked on an obstacle.");
        }
        else
        {
            PathExists = true;
            if(isSwiping)
            {
                player.target = targetGridPos;
                totalsteps++;
                pathHistory.Add(new List<Vector2> { playerpos });
            }
        }
    }
    public void Obtructed()
    {
        totalsteps--;
        RockIndexes.RemoveAt(RockIndexes.Count-1);
        RocksHistory.RemoveAt(RocksHistory.Count-1);
        pathHistory.RemoveAt(pathHistory.Count-1);
    }
    void TracePath(ref List<Vector2> currentpath)
    {
        if (player.playerCanMove && currentpath != null)
        {
            if (player.transform.position == player.target && currentpath.Count == 0)
            {
                TracedPath = true;
                currentpath = null;
                isRetracing = false;             
            }
            else
            {
                iswalking = true;               
                if (currentpath.Count!=0)
                {
                    if (isRetracing)
                    {
                        playerpos = player.transform.position;
                        if (RockIndexes.Count != 0)
                        {
                            
                            if (RockIndexes[RockIndexes.Count - 1] == totalsteps-1)
                            {
                                RocksHistory[RocksHistory.Count - 1].RetraceRock(playerpos);
                                RockIndexes.RemoveAt(RockIndexes.Count - 1);
                                RocksHistory.RemoveAt(RocksHistory.Count - 1);
                            }
                        }
                        totalsteps--; 
                        Debug.Log(totalsteps);
                    }
                    else
                    {
                        
                        totalsteps++;
                        Debug.Log(totalsteps);
                    }
                    
                    player.target = currentpath[0];
                    currentpath.RemoveAt(0);
                }
                else
                {
                    isRetracing = false;
                }
                                                          
            }
        }
    }
}
