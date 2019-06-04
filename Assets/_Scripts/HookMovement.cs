using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMovement : MonoBehaviour {

    public enum GameState { Aiming, GrabbingDown, GrabWait, GrabbingUp, Dropping, Reset }
    public GameState gameState;

    public GameObject frameX;
    public GameObject frameZ;
    public GameObject hook;
    public GameObject[] claws;

    public float speed;

    private SpringJoint springJoint;
    private HingeJoint[] clawHinges = new HingeJoint[3];
    private JointMotor[] motor = new JointMotor[3];

    private float x;
    private float z;

	void Start () {
        gameState = GameState.Aiming;

        springJoint = hook.GetComponent<SpringJoint>();
        clawHinges[0] = claws[0].GetComponent<HingeJoint>();
        clawHinges[1] = claws[1].GetComponent<HingeJoint>();
        clawHinges[2] = claws[2].GetComponent<HingeJoint>();

        motor[0] = clawHinges[0].motor;
        motor[1] = clawHinges[1].motor;
        motor[2] = clawHinges[2].motor;
        clawHinges[0].motor = motor[0];
        clawHinges[1].motor = motor[1];
        clawHinges[2].motor = motor[2];
    }

    void Update () {
        //Debug.Log(gameState);
        x = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        z = Input.GetAxis("Horizontal") * Time.deltaTime * -speed;

        switch (gameState)
        {
            case GameState.Aiming:

                if (x < 0.006f && x > -0.006f)
                {
                    frameZ.transform.Translate(0, 0, z);
                }
                if (z < 0.006f && z > -0.006f)
                {
                    frameX.transform.Translate(x, 0, 0);
                }

                frameX.transform.position = new Vector3(Mathf.Clamp(frameX.transform.position.x, -0.8f, 0.8f), frameX.transform.position.y, frameX.transform.position.z);
                frameZ.transform.position = new Vector3(frameZ.transform.position.x, frameZ.transform.position.y, Mathf.Clamp(frameZ.transform.position.z, -0.75f, 0.75f));

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameState = GameState.GrabbingDown;
                }

                break;

            case GameState.GrabbingDown:

                springJoint.damper++;
                if (springJoint.damper > 220 || Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(WaitForLiftOff());
                }

                break;

            case GameState.GrabWait:
                //this is where the hook should close
                StartCoroutine(ClambDown());

                break;

            case GameState.GrabbingUp:
                //this is where the hook goes up
                springJoint.damper--;
                if (springJoint.damper < 0.2f)
                {
                    gameState = GameState.Dropping;
                }
                break;

            case GameState.Dropping:
                //this is where the hook moves over to the drop point and drops it
                if (frameX.transform.position.x > -0.5f)
                {
                    frameX.transform.position = new Vector3(frameX.transform.position.x - speed * Time.deltaTime, frameX.transform.position.y, frameX.transform.position.z);
                }
                else if (frameZ.transform.position.z < 0.5f)
                {
                    frameZ.transform.position = new Vector3(frameZ.transform.position.x, frameZ.transform.position.y, frameZ.transform.position.z + speed * Time.deltaTime);
                }
                else
                {
                    motor[0].targetVelocity = 40f;
                    motor[1].targetVelocity = 40f;
                    motor[2].targetVelocity = 40f;

                    clawHinges[0].motor = motor[0];
                    clawHinges[1].motor = motor[1];
                    clawHinges[2].motor = motor[2];
                    StartCoroutine(WaitForReset());
                }
                break;

            case GameState.Reset:
                
                if (frameX.transform.position.x < 0f)
                {
                    frameX.transform.position = new Vector3(frameX.transform.position.x + speed * Time.deltaTime, frameX.transform.position.y, frameX.transform.position.z);
                }
                else if (frameZ.transform.position.z > 0f)
                {
                    frameZ.transform.position = new Vector3(frameZ.transform.position.x, frameZ.transform.position.y, frameZ.transform.position.z - speed * Time.deltaTime);
                }
                else
                {
                    gameState = GameState.Aiming;
                }
                
                break;

            default:
                Debug.Log("There is a bugg :(");
                break;
        }
    }
    
    IEnumerator WaitForLiftOff()
    {
        gameState = GameState.GrabWait;
        yield return new WaitForSeconds(7f);
        gameState = GameState.GrabbingUp;
    }

    IEnumerator ClambDown()
    {
        yield return new WaitForSeconds(5f);
        motor[0].targetVelocity = -1f;
        motor[1].targetVelocity = -1f;
        motor[2].targetVelocity = -1f;

        clawHinges[0].motor = motor[0];
        clawHinges[1].motor = motor[1];
        clawHinges[2].motor = motor[2];
    }

    IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(3.5f);
        gameState = GameState.Reset;
    }
}
