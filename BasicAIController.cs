using UnityEngine;
using System.Collections;

public class BasicAIController : MonoBehaviour {
    [HideInInspector]
    public Animator animator;
    
    [HideInInspector]
    public CharacterController characterController;
    private Vector3 moveDirection;
    [HideInInspector]
    public Senses senses;
    public float turnSpeed = 2.0f;

    public Vector3 MoveDirection { get { return moveDirection; } }
    public float forwardSpeed = 1.25f;



    public void Awake() {
        animator = GetComponent<Animator>();
        senses = GetComponent<Senses>();
    }


	// Use this for initialization
	public void Start () {
        // animator = GetComponent<Animator>();
     
        characterController = GetComponent<CharacterController>();
        
        Rigidbody[] rbs = transform.root.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

	}
	
	// Update is called once per frame
	public void Update () {
        
        //if (senses.CanSeeTarget()) {
        //    // Exercise 3.6
        //    animator.SetBool("Surrender", true);
        //}
	}

    void Die() {
        characterController.enabled = false;
        animator.enabled = false;
        Rigidbody[] rbs = transform.root.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        Collider[] cols = transform.root.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols) {
            col.isTrigger = false;
        }
        this.enabled = false;
    }

}
