using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// TODO
/// tilePrefab�� ���� ����, ������ ���� �Ǵ� Ư�� ���� ���� �ݺ� ��� �߰�.
/// => Ÿ�� �� ����.
/// <summary>
/// �ϳ��� ū ���� ������ �ش� �ʿ��� �ִ� ��ġ�� ���� Ÿ�� ��ġ.
/// </summary>
public class MapManager : ManagerBase
{
    public static MapManager instance;

    public Vector2Int patternChunkWidth = new Vector2Int(40, 40); // 패턴 청크의 가로 세로 길이
    public int renderDistance = 5; // 랜더링 거리(플레이어 청크를 기준으로 전후좌우 몇개의 청크를 로드할지)
    public GameObject defaultTilePrefab; // 디폴트 타입 프리팹
    

    public enum EventChunkType
    {
        EventBossA,
        EventBossB,
        EventShopA,
        EventShopB,
    }

    public enum PatternChunkType
    {
        LittleGarden,
        PatternB,
        PatternC,
        PatternD,
    }

    [System.Serializable]
    public class EventChunkData
    {
        public EventChunkType type;
        public GameObject chunkPrefab; //불러올 청크 프리팹
        public Vector2Int chunkPosition; //지정할 청크 위치
    }

    public List<EventChunkData> eventChunkPresets;

    [System.Serializable]
    public class PatternChunkData
    {
        public PatternChunkType type;
        public GameObject chunkPrefab; //불러올 청크 프리팹
        public Vector2Int chunkPosition; //패턴 내 지정할 청크 위치
    }

    public List<PatternChunkData> patternChunkPresets;


    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();
    private void Awake()
    {
        instance = this;
    }
    public override void FrameUpdate()
    {
        if (GameBase.gameBase == null)
            return;
        if (GameBase.gameBase.player == null)
            return;
        if (defaultTilePrefab == null)
            return;
        Transform player = GameBase.gameBase.player.transform;
        Vector2Int playerChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / patternChunkWidth.x),
            Mathf.FloorToInt(player.position.z / patternChunkWidth.y)
        );

        HashSet<Vector2Int> activeChunks = new HashSet<Vector2Int>();

        //플레이어청크를 기준으로 랜더링 거리만큼 전후좌우의 모든 청크를 로드
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunk.x + x, playerChunk.y + z);
                activeChunks.Add(chunkCoord);
                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    //청크로드
                    LoadChunk(chunkCoord);
                }
            }
        }

        //������ ûũ���� ��ǥ�� ���� ����Ʈ�� ����
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        // ���� �ε�� ûũ �߿��� Ȱ��ȭ�� ûũ�� �ƴ� ûũ�� ã��
        foreach (var chunk in loadedChunks.Keys)
        {
            // Ȱ��ȭ�� ûũ�� �ƴ� ûũ�� ã��
            if (!activeChunks.Contains(chunk))
            {
                chunksToRemove.Add(chunk);
            }
        }

        // Ȱ��ȭ�� ûũ�� �ƴ� ûũ�� ��Ȱ��ȭ
        foreach (var chunk in chunksToRemove)
        {
            UnloadChunk(chunk);
        }
    }

    //벡터int로 받은 좌표로 불러올 패턴 청크의 인덱스에 맞게 변환하는 메서드
    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }


    // ������ �������� ���� ûũ Ȱ��ȭ
    void LoadChunk(Vector2Int coord)
    {

        GameObject chunk = new GameObject($"Chunk_{coord.x}_{coord.y}");
        // ûũ�� ��ġ�� ����
        chunk.transform.position = new Vector3(coord.x * patternChunkWidth.x, 0, coord.y * patternChunkWidth.y);
        // ûũ�� �θ� ����
        chunk.transform.parent = transform;

        //찾은 이벤트 청크 저장받기
        EventChunkData matchingEventChunk = null;
        PatternChunkType patternType = PatternChunkType.PatternA; // 기본값

        bool isEventChunk = false;
        bool isPatternChunk = false;

        //일치하는 이벤트 청크를 찾기 위한 루프
        foreach (var chunkData in eventChunkPresets)
        {
            if (chunkData.chunkPosition == coord)
            {
                isEventChunk = true;
                matchingEventChunk = chunkData;
            }
        }

        //if (isEventChunk == false)
        //{
        //    Vector2Int localIndex = new Vector2Int(
        //        Mod(coord.x, patternChunkWidth.x),
        //        Mod(coord.y, patternChunkWidth.y)
        //    );
        //}

        Vector3 tilePosition = new Vector3(coord.x * patternChunkWidth.x + patternChunkWidth.x, 0, coord.y * patternChunkWidth.y + patternChunkWidth.y);

        // ûũ�� ���������� ��ġ�� ûũ�� �ִ��� Ȯ��
        if (matchingEventChunk != null)
        {
            // 이벤트 청크 프리팹을 사용하여 청크를 생성
            Instantiate(matchingEventChunk.chunkPrefab, tilePosition, Quaternion.identity, chunk.transform);
        }
        else
        {
            // 기초 타일 프리팹을 사용하여 타일을 생성
            Instantiate(defaultTilePrefab, tilePosition, Quaternion.identity, chunk.transform);
            Debug.LogError($"[MapManager] Error: No matching event or pattern chunk preset for coord {coord}");
        }

        loadedChunks[coord] = chunk;
    }

    // �������� ������ �������� ��� ûũ�� ��Ȱ��ȭ
    void UnloadChunk(Vector2Int coord)
    {
        if (loadedChunks.ContainsKey(coord))
        {
            Destroy(loadedChunks[coord]);
            loadedChunks.Remove(coord);
        }
    }
}
