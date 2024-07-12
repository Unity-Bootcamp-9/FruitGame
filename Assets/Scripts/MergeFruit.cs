using UnityEngine;

public class MergeFruit : MonoBehaviour
{
    public enum Type { Blueberry, Strawberry, Durian }
    public Type fruitType;

    private FruitGameManager gameManager;
    private bool isMerged = false; // 과일이 이미 합쳐졌는지 여부

    void Start()
    {
        gameManager = FindObjectOfType<FruitGameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isMerged && collision.gameObject.CompareTag(tag)) // 플래그 확인
        {
            MergeFruit otherFruit = collision.gameObject.GetComponent<MergeFruit>();

            if (otherFruit != null && otherFruit.fruitType == fruitType && !otherFruit.isMerged)
            {
                // 과일 합치기 로직
                MergeFruits(collision.gameObject);
                gameManager.OnFruitMerged(fruitType);
            }
        }
    }

    private void MergeFruits(GameObject otherFruit)
    {
        isMerged = true; // 현재 과일이 합쳐졌음을 표시
        MergeFruit otherFruitScript = otherFruit.GetComponent<MergeFruit>();
        if (otherFruitScript != null)
        {
            otherFruitScript.isMerged = true; // 다른 과일도 합쳐졌음을 표시
        }

        Debug.Log($"과일 합쳐짐: {fruitType}");

        Vector3 mergePosition = transform.position; // 합쳐질 위치
        Destroy(otherFruit);
        Destroy(gameObject);

        Type newFruitType = Type.Blueberry; // 기본값

        switch (fruitType)
        {
            case Type.Blueberry:
                newFruitType = Type.Strawberry;
                break;
            case Type.Strawberry:
                newFruitType = Type.Durian;
                break;
        }

        InstantiateFruit(newFruitType, mergePosition);
    }

    private GameObject InstantiateFruit(Type newFruitType, Vector3 position)
    {
        GameObject fruitPrefab = null;
        switch (newFruitType)
        {
            case Type.Blueberry:
                fruitPrefab = Resources.Load<GameObject>("Prefabs/Blueberry");
                break;
            case Type.Strawberry:
                fruitPrefab = Resources.Load<GameObject>("Prefabs/Strawberry");
                break;
            case Type.Durian:
                fruitPrefab = Resources.Load<GameObject>("Prefabs/Durian");
                break;
        }
        return Instantiate(fruitPrefab, position, Quaternion.identity);
    }
}