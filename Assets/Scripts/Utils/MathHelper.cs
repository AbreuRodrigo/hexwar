using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper : MonoBehaviour
{
    public static bool HasNeighborInDirection(Vector3 direction)
    {
        Vector3 p = Camera.main.WorldToScreenPoint(direction);
        Ray ray = Camera.main.ScreenPointToRay(p);
        
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10);

        return hit.collider != null;
    }

    public static Hexagon DetectNeighbor(Vector3 direction)
    {
        Vector3 p = Camera.main.WorldToScreenPoint(direction);
        Ray ray = Camera.main.ScreenPointToRay(p);

        Vector2 or = ray.origin;

        RaycastHit2D hit = Physics2D.Raycast(or, ray.direction);

        Collider2D collider = hit.collider;

        Hexagon hexRef = null;

        if (collider != null)
        {
            hexRef = collider.GetComponent<Hexagon>();
        }

        return hexRef;
    }
}