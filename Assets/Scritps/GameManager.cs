using TMPro;
using UnityEngine;
using cakeslice;

public class GameManager : MonoBehaviour
{
    public TorchLightManager torch;
    public PlayerController player;
    public static GameManager instance;
    public bool isGameOver;

    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI interactionText;

    [SerializeField] private LayerMask doorLayerMask;
    [SerializeField] private LayerMask keyLayerMask;
    [SerializeField] private LayerMask timeIncreaserLayerMask;

    private Outline currentHighlightedOutline;
    private GameObject currentInteractableObject;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        gameOver.SetActive(false);
        isGameOver = false;
        interactionText.enabled = false;
    }

    private void Update()
    {
        if (isGameOver) return;

        InteractionWithRaycast();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractableObject != null)
        {
            HandleInteraction();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
        torch.HideSlider();
        player.HideSlider();
    }

    void InteractionWithRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, doorLayerMask | keyLayerMask | timeIncreaserLayerMask))
        {
            GameObject target = hit.collider.gameObject;
            int layer = target.layer;

            Outline outline = target.GetComponent<Outline>();

            if (currentHighlightedOutline != null && currentHighlightedOutline != outline)
                currentHighlightedOutline.enabled = false;

            if (outline != null)
            {
                outline.enabled = true;
                currentHighlightedOutline = outline;
            }

            if (((1 << layer) & doorLayerMask) != 0)
            {
                if (InventoryManager.instance.HasItem(InventoryItems.key))
                    interactionText.text = "Press E to Open Door";
                else
                    interactionText.text = "Need a key to open this door";
            }
            else if (((1 << layer) & keyLayerMask) != 0)
            {
                interactionText.text = "Press E to Collect Key";
            }
            else if (((1 << layer) & timeIncreaserLayerMask) != 0)
            {
                interactionText.text = "Press E to Collect Time Increaser";
            }

            interactionText.enabled = true;
            currentInteractableObject = target;
        }
        else
        {
            if (currentHighlightedOutline != null)
                currentHighlightedOutline.enabled = false;

            interactionText.enabled = false;
            currentInteractableObject = null;
            currentHighlightedOutline = null;
        }
    }

    void HandleInteraction()
    {
        if (currentInteractableObject == null) return;

        int layer = currentInteractableObject.layer;

        if (((1 << layer) & doorLayerMask) != 0)
        {
            if (InventoryManager.instance.HasItem(InventoryItems.key))
            {
                OpenDoor(currentInteractableObject);
                InventoryManager.instance.UseItem(InventoryItems.key);
                Debug.Log("Door opened using a key.");
            }
            else
            {
                Debug.Log("You need a key to open this door.");
            }
        }
        else if (((1 << layer) & keyLayerMask) != 0)
        {
            InventoryManager.instance.AddItems(InventoryItems.key);
            Destroy(currentInteractableObject);
            Debug.Log("Key collected!");
        }
        else if (((1 << layer) & timeIncreaserLayerMask) != 0)
        {
            InventoryManager.instance.AddItems(InventoryItems.timeIncreaser);
            Destroy(currentInteractableObject);
            Debug.Log("Time Increaser collected!");
        }

        interactionText.enabled = false;

        if (currentHighlightedOutline != null)
            currentHighlightedOutline.enabled = false;

        currentInteractableObject = null;
        currentHighlightedOutline = null;
    }

    void OpenDoor(GameObject door)
    {
        Debug.Log("Opening door...");
        Destroy(door);
    }
}
