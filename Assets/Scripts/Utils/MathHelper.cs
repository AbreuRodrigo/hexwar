using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper : MonoBehaviour
{

    public static bool HasNeighborInDirection(Hexagon origin, Vector3 direction)
    {
        Vector3 p = Camera.main.WorldToScreenPoint(direction);
        Ray ray = Camera.main.ScreenPointToRay(p);
        
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10);

        return hit.collider != null;
    }

    public static Hexagon DetectNeighbor(Hexagon origin, Vector3 direction)
    {
        Vector3 p = Camera.main.WorldToScreenPoint(direction);
        Ray ray = Camera.main.ScreenPointToRay(p);
        
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        Collider2D collider = hit.collider;

        Hexagon hexRef = null;

        if (collider != null)
        {
            hexRef = collider.GetComponent<Hexagon>();
        }

        return hexRef;
    }
}