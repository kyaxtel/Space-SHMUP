using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(BoundsCheck) )]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }
    void Update()
    {
        if ( bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp) ) {
            Destroy( gameObject );
        }
    }
}
