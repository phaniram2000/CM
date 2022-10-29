using UnityEngine;

public class TBRGameController : MonoBehaviour
{

    public static TBRGameController Get { get; set; }
    
    [SerializeField] private int itemsToPick = 2;
    
    private int gameItemsPickedCount;


    public int GameItemsPickedCount => gameItemsPickedCount;

    public int ItemsToPick => itemsToPick;

    private void OnEnable()
    {
        TBREvents.ItemPickedUpByGirl += OnItemPickedUpByGirl;
    }

    private void OnDisable()
    {
        TBREvents.ItemPickedUpByGirl -= OnItemPickedUpByGirl;
    }

    private void Awake()
    {
        if (!Get) Get = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameItemsPickedCount = 0;
    }

    private void OnItemPickedUpByGirl()
    {
        gameItemsPickedCount++;

        if (gameItemsPickedCount < itemsToPick) return;
        
        TBREvents.InvokeOnGirlPrankingDoneNowEscape();
    }
    
    
    
}
