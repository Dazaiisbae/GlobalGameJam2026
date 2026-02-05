using System.Collections;
using UnityEngine;

public class ElectricShot : Ability
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    /// <summary>
    /// public Camera cam;
    /// </summary>
    Animator animator;

    public Camera aimCamera;         // drag the active cam here OR leave null to use Camera.main
    public float aimRange = 500f;
    public LayerMask aimMask = ~0;   // what the raycast can hit

    public void Start()
    {
        animator = GetComponent<Animator>();

        ///if (cam == null)
            ///cam = GetComponent<Camera>().main;
    }

    public override void Activate()
    {
        var stealth = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerStealth>();

        if (stealth != null)
            stealth.RevealTemporarily();

        Camera cam = aimCamera != null ? aimCamera : Camera.main;

        // Ray from screen center (crosshair)
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, aimRange, aimMask))
            aimPoint = hit.point;
        else
            aimPoint = ray.GetPoint(aimRange);

        // Shoot from muzzle toward aim point
        Vector3 dir = (aimPoint - firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        Instantiate(projectilePrefab, firePoint.position, rot);

        animator.SetBool("IsAttacking", true);
        StartCoroutine(WaitForSeconds());
    }
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(.1f);

        animator.SetBool("IsAttacking", false);
    }

}
