using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BlockType { Air, Stone, Grass, Dirt, Water }

public class WorldGenerator : MonoBehaviour
{
    [Header("World formation")]
    [Tooltip("Number of blocks in X and Z directions")]
    public int n = 10; // Test with small values
    [Tooltip("Max world height")]
    public int m = 384; // Max height
    [Tooltip("Terrain height")]
    public float T_H = 20;
    public float f = 0.1f; // Controls wave density
    public float amp = 10f; // Controls wave height

    [Header("Blocks")]
    public GameObject stonePrefab; // Assign stone block prefab
    public GameObject grassPrefab; // Assign grass block prefab
    public GameObject dirtPrefab; // Assign dirt block prefab

    private BlockType[,,] worldGrid; // Tracks block types
    private GameObject[,,] blocksGrid; // Tracks instantiated prefabs
    private Transform stoneParent; // Parent for stone blocks
    private Transform surfaceParent; // Parent for grass blocks
    private Camera _mainCamera;
    //private Renderer _renderer;

    void Start()
    {
        CreateParentContainers(); // Create "StoneBlocks" and "GrassBlocks" parents
        InitializeWorldGrid();
        GenerateWorldFromGrid();
        ReplaceSurfaceWithGrass();
        IsBlockSurrounded();
        //CombineMeshes();

    }

    /*void Update()
    {
        MeshRenderer _renderer1 = stonePrefab.GetComponent<MeshRenderer>();
        MeshRenderer _renderer2 = grassPrefab.GetComponent<MeshRenderer>();
        MeshRenderer _renderer3 = dirtPrefab.GetComponent<MeshRenderer>();
        // Check if the block is inside the camera's view frustum
        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_mainCamera), _renderer1.bounds))
        {
            _renderer1.enabled = true;
        }
        else
        {
            _renderer1.enabled = false;
        }
        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_mainCamera), _renderer2.bounds))
        {
            _renderer2.enabled = true;
        }
        else
        {
            _renderer2.enabled = false;
        }
        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_mainCamera), _renderer3.bounds))
        {
            _renderer3.enabled = true;
        }
        else
        {
            _renderer3.enabled = false;
        }
    }
    */
    void CreateParentContainers()
    {
        surfaceParent = new GameObject("GrassBlocks").transform;
        surfaceParent.SetParent(transform);
        stoneParent = new GameObject("StoneBlocks").transform;
        stoneParent.SetParent(transform); // Make child of this World object
    }

    // Initialize grid with stone below y=100
    void InitializeWorldGrid()
    {
        int worldSizeX = n;
        int worldSizeZ = n;
        int worldSizeY = m;


        worldGrid = new BlockType[worldSizeX, worldSizeY, worldSizeZ];
        blocksGrid = new GameObject[worldSizeX, worldSizeY, worldSizeZ];

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                float terrainHeight = T_H + amp * Mathf.Sin(f * x) * Mathf.Sin(f * z);

                //float terrainHeight = (T_H/Random.Range(T_H - 3, T_H + 4)) + amp * Mathf.PerlinNoise(f * x, f * z);

                float noise1 = 2 * amp * Mathf.Sin(f * 0.1f * x) * Mathf.Sin(f * 0.05f * z);

                float noise2 = amp * Mathf.Tan(f * x) * Mathf.Tan(f * z);

                

                for (int y = 0; y < worldSizeY; y++)
                {
                    worldGrid[x, y, z] = (y < terrainHeight) ? BlockType.Stone : BlockType.Air;
                }
            }
        }
    }

    // Instantiate blocks based on worldGrid
    void GenerateWorldFromGrid()
    {
        for (int x = 0; x < worldGrid.GetLength(0); x++)
        {
            for (int y = 0; y < worldGrid.GetLength(1); y++)
            {
                for (int z = 0; z < worldGrid.GetLength(2); z++)
                {
                    BlockType type = worldGrid[x, y, z];
                    if (type != BlockType.Air)
                    {
                        GameObject prefab = GetPrefabForType(type);
                        Vector3 position = new Vector3(x, y, z);
                        Transform parent = (type == BlockType.Stone) ? stoneParent : surfaceParent;
                        GameObject block = Instantiate(prefab, position, Quaternion.identity, parent);
                        blocksGrid[x, y, z] = block;
                    }
                }
            }
        }
    }

    // Replace top stone blocks with grass
    void ReplaceSurfaceWithGrass()
    {
        for (int x = 0; x < worldGrid.GetLength(0); x++)
        {
            for (int z = 0; z < worldGrid.GetLength(2); z++)
            {
                for (int y = worldGrid.GetLength(1) - 1; y >= 0; y--)
                {
                    if (worldGrid[x, y, z] == BlockType.Stone)
                    {
                        SetBlock(x, y, z, BlockType.Grass);
                        for (int i = y-1; i>= y-5; i--)
                        {
                            SetBlock(x, i , z, BlockType.Dirt);
                        }
                        break;
                    }
                }
            }
        }
    }

    // Public method to modify blocks (e.g., player interaction)
    public void SetBlock(int x, int y, int z, BlockType newType)
    {
        if (!IsInBounds(x, y, z)) return;

        BlockType currentType = worldGrid[x, y, z];
        if (currentType == newType) return;

        // Destroy existing block (if any)
        if (blocksGrid[x, y, z] != null)
        {
            Destroy(blocksGrid[x, y, z]);
            blocksGrid[x, y, z] = null;
        }

        // Instantiate new block (if not air)
        if (newType != BlockType.Air)
        {
            GameObject prefab = GetPrefabForType(newType);
            Vector3 position = new Vector3(x, y, z);
            Transform parent = (newType == BlockType.Stone) ? stoneParent : surfaceParent;
            GameObject block = Instantiate(prefab, position, Quaternion.identity, parent);
            blocksGrid[x, y, z] = block;
        }

        worldGrid[x, y, z] = newType;
    }

    // Helper to get prefab for a block type
    private GameObject GetPrefabForType(BlockType type)
    {
        switch (type)
        {
            case BlockType.Stone: return stonePrefab;
            case BlockType.Grass: return grassPrefab;
            case BlockType.Dirt: return dirtPrefab;
            default: return null;
        }
    }

    private bool IsInBounds(int x, int y, int z)
    {
        return x >= 0 && x < worldGrid.GetLength(0) &&
               y >= 0 && y < worldGrid.GetLength(1) &&
               z >= 0 && z < worldGrid.GetLength(2);
    }

    // Check if a block is completely surrounded by non-air blocks
    void IsBlockSurrounded()
    {
        for (int x = 0; x < worldGrid.GetLength(0); x++)
        {
            for (int y = 0; y < worldGrid.GetLength(1); y++)
            {
                for (int z = 0; z < worldGrid.GetLength(2); z++)
                {
                    bool hasNeighborAbove = CheckNeighbor(x, y + 1, z);
                    bool hasNeighborBelow = CheckNeighbor(x, y - 1, z);
                    bool hasNeighborLeft = CheckNeighbor(x - 1, y, z);
                    bool hasNeighborRight = CheckNeighbor(x + 1, y, z);
                    bool hasNeighborFront = CheckNeighbor(x, y, z + 1);
                    bool hasNeighborBack = CheckNeighbor(x, y, z - 1);
                    if (worldGrid[x, y, z] != BlockType.Air && (hasNeighborAbove && hasNeighborBelow && hasNeighborLeft && hasNeighborRight && hasNeighborFront && hasNeighborBack) == true)
                    {
                        //MeshRenderer _renderer = blocksGrid[x, y, z].GetComponent<MeshRenderer>();
                        //_renderer.enabled = false;
                        blocksGrid[x, y, z].SetActive(false);
                        Debug.Log("Block should be hidden"+x);
                        Debug.Log(y);
                        Debug.Log(z);
                    }
                }
            }
        }

    }

    // Check if a neighbor exists and is non-air
    bool CheckNeighbor(int x, int y, int z)
    {
        // Boundary check
        if (x < 0 || x >= worldGrid.GetLength(0) ||
            y < 0 || y >= worldGrid.GetLength(1) ||
            z < 0 || z >= worldGrid.GetLength(2))
        {
            return false; // Treat out-of-bounds as "air"
        }

        return worldGrid[x, y, z] != BlockType.Air;
    }


    /*void CombineGrassMeshes()
    {
        MeshCombiner meshCombiner = surfaceParent.gameObject.AddComponent<MeshCombiner>();
        meshCombiner.CombineMeshes();

        // Assign material to the combined mesh
        MeshRenderer renderer = surfaceParent.GetComponent<MeshRenderer>();
        renderer.material = grassPrefab.GetComponent<Renderer>().sharedMaterial;
    }
    void CombineStoneMeshes()
    {
        MeshCombiner meshCombiner = stoneParent.gameObject.AddComponent<MeshCombiner>();
        meshCombiner.CombineMeshes();

        // Assign material to the combined mesh
        MeshRenderer renderer = stoneParent.GetComponent<MeshRenderer>();
        renderer.material = stonePrefab.GetComponent<Renderer>().sharedMaterial;
    }
    
    void CombineMeshes()
    {
        CombineGrassMeshes();
        CombineStoneMeshes();
    }
    */
}