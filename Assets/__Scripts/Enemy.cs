using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;
using Unity.VisualScripting;

[RequireComponent( typeof(BoundsCheck) )]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f; // The movement speed is 10m/s
    public float fireRate = 0.3f; // Seconds/shot (Unused)
    public float health = 10; // Damage needed to destroy this enemy
    public int score = 100; // Points earned for destroying this
    public float powerUpDropChance = 1f; // Chance to drop a PowerUp
    
    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck; // Change bndCheck from private to protected

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // This is a property: A method that acts like a field
    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();
        // Check wether this Enemy has gone off the bottom of the screen
        if ( bndCheck.LocIs( BoundsCheck.eScreenLocs.offDown )) {
            Destroy( gameObject );
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        // Check for collisions with ProjectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if ( p != null ) {
            // Only damage this Enemy if it's on screen
            if ( bndCheck.isOnScreen ) {
                // Get the damage amount from the Main WEAP_DICT.
                health -= Main.GET_WEAPON_DEFINITION( p.type ).damageOnHit;
                if ( health <= 0 ) {
                    // Tell main that this ship was destroyed
                    if ( !calledShipDestroyed) {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    Destroy( this.gameObject );
                }
            }
            // Destroy the ProjectileHero regardles
            Destroy( otherGO );
        }
        else {
            print( "Enemy hit by non-ProjectileHero: " + otherGO.name );
        }
    }
}
