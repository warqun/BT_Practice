using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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

    private NavMeshSurface navMeshSurface;

    bool navMeshNeedsUpdate = true;

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
    public class MetaPatternChunk
    {
        public Vector2Int localCoord; // 메타 패턴 내 위치 (예: (1,3))
        public GameObject prefab;     // 해당 위치에 생성할 프리팹
    }

    [System.Serializable]
    public class MetaPattern
    {
        public Vector2Int size = new Vector2Int(4, 4); // 메타 패턴의 가로, 세로 크기
        public List<MetaPatternChunk> chunks = new List<MetaPatternChunk>();
    }

    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();
    public MetaPattern metaPattern;

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
                navMeshNeedsUpdate = true; // 네비메시 업데이트 필요
            }
        }

        // Ȱ��ȭ�� ûũ�� �ƴ� ûũ�� ��Ȱ��ȭ
        foreach (var chunk in chunksToRemove)
        {
            UnloadChunk(chunk);
        }

        // 청크 로드 후 네비메시 빌드
        if (navMeshNeedsUpdate)
        {
            navMeshNeedsUpdate = false;
            BakeGlobalNavMesh();
        }
    }

    //벡터int로 받은 좌표로 불러올 패턴 청크의 인덱스에 맞게 변환하는 메서드
    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    private void BakeGlobalNavMesh()
    {
        if (navMeshSurface == null)
            navMeshSurface = GetComponent<NavMeshSurface>();

        navMeshSurface.BuildNavMesh();
    }



    // ������ �������� ���� ûũ Ȱ��ȭ
    void LoadChunk(Vector2Int coord)
    {
        // 1. 이벤트 청크 우선
        EventChunkData matchingEventChunk = eventChunkPresets.Find(e => e.chunkPosition == coord);
        if (matchingEventChunk != null)
        {
            GameObject chunk = new GameObject($"EventChunk_{coord.x}_{coord.y}");
            chunk.transform.position = new Vector3(coord.x * patternChunkWidth.x, 0, coord.y * patternChunkWidth.y);
            chunk.transform.parent = transform;
            Instantiate(matchingEventChunk.chunkPrefab, chunk.transform.position, Quaternion.identity, chunk.transform);
            loadedChunks[coord] = chunk;
            return;
        }

        // 2. 메타 패턴 청크
        if (metaPattern != null && metaPattern.size.x > 0 && metaPattern.size.y > 0)
        {
            // 메타 패턴 내 위치 계산 (반복)
            int localX = Mod(Mod(coord.x, metaPattern.size.x), metaPattern.size.x);
            int localY = Mod(Mod(coord.y, metaPattern.size.y), metaPattern.size.y);
            Vector2Int localCoord = new Vector2Int(localX, localY);

            MetaPatternChunk patternChunk = metaPattern.chunks.Find(c => c.localCoord == localCoord);
            if (patternChunk != null && patternChunk.prefab != null)
            {
                GameObject chunk = new GameObject($"MetaPatternChunk_{coord.x}_{coord.y}");
                chunk.transform.position = new Vector3(coord.x * patternChunkWidth.x, 0, coord.y * patternChunkWidth.y);
                chunk.transform.parent = transform;
                Instantiate(patternChunk.prefab, chunk.transform.position, Quaternion.identity, chunk.transform);
                loadedChunks[coord] = chunk;
                return;
            }
        }

        // 3. 기본 타일
        GameObject defaultChunk = new GameObject($"DefaultChunk_{coord.x}_{coord.y}");
        defaultChunk.transform.position = new Vector3(coord.x * patternChunkWidth.x, 0, coord.y * patternChunkWidth.y);
        defaultChunk.transform.parent = transform;
        Instantiate(defaultTilePrefab, defaultChunk.transform.position, Quaternion.identity, defaultChunk.transform);
        loadedChunks[coord] = defaultChunk;
    }

    //청크제거
    void UnloadChunk(Vector2Int coord)
    {
        if (loadedChunks.ContainsKey(coord))
        {
            Destroy(loadedChunks[coord]);
            loadedChunks.Remove(coord);
        }
    }
}
