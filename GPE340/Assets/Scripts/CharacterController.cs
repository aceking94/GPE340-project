using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class CharacterController : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float turnSpeed = 7f;
    public float jumpForce = 10f;

    bool turning = true;
    bool running = false;

    private Rigidbody body; // Rigidbody for the charecter
    private Animator animator; //The animator for the character

    //Intiger values for animation parameters
    private int horizontal;
    private int vertical;
    private int inTheAir;
    private int onTheGround;
    private int velocity;

    //Weapon
    private Weapon equippedWeapon;
    private Weapon tempWeapon;
    public Weapon pistol;
    public Weapon rifle;
    public Transform holdPoints;

    public PlayerData health { get; private set; } // set and get health

    void Awake()
    {
        //Get the character's animator component
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        health = GetComponent<PlayerData>();

        //Get the ID for setting the all animator parameters
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        inTheAir = Animator.StringToHash("Jumping");
        onTheGround = Animator.StringToHash("OnGround");
        velocity = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        //Set the animator bools of Forward and Right to the V/H axis
        animator.SetFloat(horizontal, Input.GetAxis("Vertical"));
        animator.SetFloat(vertical, Input.GetAxis("Horizontal"));
        animator.SetFloat(velocity, body.velocity.y);
        animator.SetBool("Running", running);

        //Movement  
        if (OnGround)
        {
            //The get input for x and z
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            //Clamp the movement
            input = Vector3.ClampMagnitude(input, 1f);

            //Multiply by speed
            input *= movementSpeed;

            //Convert it from local-space to world-space
            input = transform.InverseTransformDirection(input);

            //Aply movement
            animator.SetFloat(horizontal, input.x);
            animator.SetFloat(vertical, input.z);
        }

        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Up the speed of movement when sprinting
            running = true;
        }
        else
        {
            //When not h olding lower the speed and set sprinting to false
            running = false;            
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            //Jump
            Jumping = true;
            //The character is no longer on the ground
            OnGround = false;
            //Add a jump force
            body.AddRelativeForce(0f, jumpForce, 0f, ForceMode.Impulse);
        }
        if (Jumping && body.velocity.y <= 0f)
        {
            //In the air and falling
            Jumping = false;
        }

        //Turn
        if (turning)
        {
            //Look at the mouse pointer
            Follow();
        }

        //Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0)) //if held down shoot
        {
            // Shoot Only if weapon
            if (equippedWeapon)
            {
                equippedWeapon.AttackStart();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) //stop shooting when release
        {
            // Only if weapon
            if (equippedWeapon)
            {
                equippedWeapon.AttackEnd();
            }
        }

        //Weapons Switch
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Equip pistol           
            tempWeapon = pistol;
            tempWeapon.weaponType = WeaponType.Handgun;
            animator.SetInteger("WeaponType", (int)tempWeapon.weaponType);
            EquipWeapon(tempWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Equip rifle            
            tempWeapon = rifle;
            tempWeapon.weaponType = WeaponType.Rifle;
            animator.SetInteger("WeaponType", (int)tempWeapon.weaponType);
            EquipWeapon(tempWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Unequip weapon
            if (equippedWeapon)
            {
                equippedWeapon.weaponType = WeaponType.None;
                tempWeapon.weaponType = WeaponType.None;

                UnequipWeapon();
            }
        }
    }

    void FixedUpdate()
    {
        //Move in the air
        if (!OnGround)
        {
            //Set the animator floats to zero
            animator.SetFloat(horizontal, 0f);
            animator.SetFloat(vertical, 0f);
            
            //Set the velocity for moving less in the air
            body.velocity = new Vector3(Input.GetAxis("Horizontal") * movementSpeed/2, body.velocity.y, Input.GetAxis("Vertical") * movementSpeed/2);
        }
    }

    void Follow()
    {
        //Area to look at
        Plane playArea = new Plane(Vector3.up, transform.position);

        //Create ray too mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //If in play are then get distance
        if (playArea.Raycast(ray,out var distance))
        {
            //Store hit point
            var hitPoint = ray.GetPoint(distance);

            //Store the previous rotatio
            var previousRotation = transform.rotation;

            //Turn to hit point
            transform.LookAt(hitPoint);

            //Get the new rotation
            var newRotation = transform.rotation;

            //Put it all together
            transform.rotation = Quaternion.Lerp(previousRotation, newRotation, turnSpeed * Time.deltaTime);
        }
    }

    void EquipWeapon(Weapon weaponToEquip)
    {
        // Check to see if equiped
        if (equippedWeapon != null)
        {
            //Clear hands
            UnequipWeapon();
        }
        
        equippedWeapon = weaponToEquip;// Equip the desired weapon
        
        equippedWeapon = Instantiate(weaponToEquip) as Weapon;
        
        // Attach to specific point on player
        equippedWeapon.transform.SetParent(holdPoints);
        // Set weapon location and root
        equippedWeapon.transform.localPosition = tempWeapon.transform.localPosition;
        equippedWeapon.transform.localRotation = tempWeapon.transform.localRotation;
    }

    void UnequipWeapon()
    {
        if (equippedWeapon)
        {
            Destroy(equippedWeapon.gameObject);
            equippedWeapon = null;
        }
    }

    public void OnAnimatorIK()
    {
        // If no weapon return zero
        if (equippedWeapon == null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);

            return;
        }

        // Right Hand
        if (equippedWeapon.rightHand != null)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, equippedWeapon.rightHand.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKRotation(AvatarIKGoal.RightHand, equippedWeapon.rightHand.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);
        }

        // Left Hand
        if (equippedWeapon.leftHand != null)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, equippedWeapon.leftHand.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, equippedWeapon.leftHand.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);
        }
    }

    //If in the air
    public bool Jumping
    {
        get => animator.GetBool(inTheAir);
        private set => animator.SetBool(inTheAir, value);
    }

    //If on the ground
    public bool OnGround
    {
        get => animator.GetBool(onTheGround);
        private set => animator.SetBool(onTheGround, value);
    }

    //If the player collides 
    private void OnCollisionEnter(Collision collision)
    {
        OnGround = true;
    }
}
