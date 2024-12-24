using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;
    public LayerMask layerCropMask;

    public GameObject tinto;
    public GameObject libro;
    public GameObject semillas;
    public GameObject cultivo;

    public bool isFree;
    public bool isCultivating;

    public int currentItemID; 

    void Start()
    {
       isFree = true;
       isCultivating = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position;

        if (Input.GetKey(KeyCode.X) && isFree)
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                Item item = hit.collider.GetComponent<Item>();
                currentItemID = item.itemID;

                isFree = false;

                string objectTag = hit.collider.tag;
                
                
                switch (objectTag)
                {
                    case "Tetera":
                        Debug.Log("Impacto con una Tetera: " + hit.collider.name);
                        tinto.SetActive(true);
                        break;

                    case "Librero":
                        Debug.Log("Impacto con un Cubo: " + hit.collider.name);
                        libro.SetActive(true);
                        break;

                    case "Semillas":
                        Debug.Log("Impacto con una Esfera: " + hit.collider.name);
                        isCultivating = true;
                        semillas.SetActive(true);
                        break;

                    case "Cultivo":
                        Debug.Log("Impacto con una Esfera: " + hit.collider.name);
                        cultivo.SetActive(true);
                        break;

                    default:
                        break;
                }

                if (Physics.Raycast(origin, direction, out RaycastHit shoot, maxDistance, layerCropMask))
                {
                    if (objectTag == "Cultivo")
                    {
                        Crop crop = shoot.collider.GetComponent<Crop>();
                        crop.isCropFree = true;

                        GameObject collidedObject = shoot.collider.gameObject;
                        Transform childTransform = collidedObject.transform.GetChild(1);
                        childTransform.gameObject.SetActive(false);
                    }
                    
                }

                    Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
            }
        }

        
    }
}