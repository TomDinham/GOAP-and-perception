using UnityEngine;
using System.Collections;

public class Senses : MonoBehaviour {
    public GameObject target;
    private CharacterController characterController;
    public float viewingAngle = 200.0f;
    public float sightRange = 200.0f;
    public float engageAngle = 20f;
    [HideInInspector]
    public float forwardWeaponRotationCorrectionFactor = 0f;

	void Start(){
        characterController = GetComponent<CharacterController>();
        // Initialise here as initialising when defined saved value in properties even though hide had been enabled and name changed!!
        forwardWeaponRotationCorrectionFactor = 50f;
    }


    // Is the target within the AI's line of sight
    public bool CanSeeTarget(){
        Debug.Log("Search");
        // Check player is alive
        if (target != null){
            CharacterController targetCharacterController = target.GetComponent<CharacterController>();
            // Direction of target from AI
            Vector3 targetDirection = target.transform.position - transform.position;
            // Angle between AI and Player
            float angle = Vector3.Angle(targetDirection, transform.forward);

            angle += forwardWeaponRotationCorrectionFactor;
          //  float angle = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles.y;
           // Debug.Log("Can See Y " + angle);
          //  float angle = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles.y;
            // Convert to positive value
            angle = System.Math.Abs(angle);
            // Is the target within the viewing angle. Ignores obstacles 
            if (angle < (viewingAngle / 2)){
                // Get distance to player
                float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
                // Check in visible range
                if (sightRange > distanceToTarget)
                {
                    Debug.Log("sight");
                    RaycastHit hitData;
                    // Create a layer mask for the ray. Look for players only (player should be configured to layer 8).
                    LayerMask playerMask = 1 << 8;
                    LayerMask aiMask = 1 << 10;
                    // Player may be obscurred by cover so ensure ray picks up cover too
                    LayerMask coverMask = 1 << 9;
                    LayerMask mask = coverMask | playerMask | aiMask;
                    float targetHeight = targetCharacterController.height;
                    float height = characterController.height;
                    Vector3 eyePosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
                    // A position in the middle of the target
                    Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y - (targetHeight / 2.0f), target.transform.position.z);
                    // vector from AI to middle of target
                    Vector3 direction = (targetPos - transform.position).normalized;
                    // Cast a ray to ensure target is not hidden by obstacles
                    bool hit = Physics.Raycast(eyePosition, direction, out hitData, sightRange, mask.value);
                    Debug.DrawRay(eyePosition, direction * sightRange, Color.red);
                    // Ray hit target/cover 
                    if (hit){
                        Debug.Log("Hitting");
                        if (hitData.collider.tag == target.gameObject.tag) {
                            Debug.Log("Hit");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        else return false;
    }


    public bool IsBehindLowCover(GameObject agent, GameObject target) {
        float[] checkHeights = { 0.2f, 0.5f, 2.0f };
        bool[] covered = { false, false, false };
        float range = 3f;
        RaycastHit hitData;
        LayerMask coverMask = 1 << 9;
        Vector3 checkPosition;
        Vector3 direction = (target.transform.position - agent.transform.position).normalized;
        for(int n = 0; n < checkHeights.Length; n++){
            checkPosition = new Vector3(agent.transform.position.x, agent.transform.position.y + checkHeights[n], agent.transform.position.z);
            covered[n] = Physics.Raycast(checkPosition, direction, out hitData, range, coverMask.value);
            //Debug.DrawRay(checkPosition, direction * range, Color.yellow);
            //Debug.Log("Ray hit : " + covered[n] );
        }
        // Partial cover considered OK
        return ((covered[0] || covered[1]) && !covered[2]);
    }

 

}
