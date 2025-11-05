using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject terrainChunk;

    public Transform player;

    int gridDist = 16;

    public int seed;

    public static Dictionary<ChunkPos, TerrainChunk> chunks = new Dictionary<ChunkPos, TerrainChunk>();

    FastNoise noise = new FastNoise();

    public int Render_Distance = 12;

    List<TerrainChunk> pooledChunks = new List<TerrainChunk>();

    List<ChunkPos> toGenerate = new List<ChunkPos>();

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(0, 10000);
        LoadChunks(true);
    }


    private void Update()
    {
        LoadChunks();
    }



    void BuildChunk(int xPos, int zPos)
    {
        TerrainChunk chunk;
        if(pooledChunks.Count > 0)
        {
            chunk = pooledChunks[0];
            chunk.gameObject.SetActive(true);
            pooledChunks.RemoveAt(0);
            chunk.transform.position = new Vector3(xPos, 0, zPos);
        }
        else
        {
            GameObject chunkGO = Instantiate(terrainChunk, new Vector3(xPos, 0, zPos), Quaternion.identity);
            chunk = chunkGO.GetComponent<TerrainChunk>();
        }
        

        for(int x = 0; x < TerrainChunk.chunkWidth+2; x++)
            for(int z = 0; z < TerrainChunk.chunkWidth+2; z++)
                for(int y = 0; y < TerrainChunk.chunkHeight; y++)
                {
                    //if(Mathf.PerlinNoise((xPos + x-1) * .1f, (zPos + z-1) * .1f) * 10 + y < TerrainChunk.chunkHeight * .5f)
                    chunk.blocks[x, y, z] = GetBlockType(xPos+x-1, y, zPos+z-1);
                }


        //GenerateTrees(chunk.blocks, xPos, zPos);

        chunk.BuildMesh();


        WaterChunk wat = chunk.transform.GetComponentInChildren<WaterChunk>();
        wat.SetLocs(chunk.blocks);
        wat.BuildMesh();
        


        chunks.Add(new ChunkPos(xPos, zPos), chunk);
    }


    //get the block type at a specific coordinate
    BlockType GetBlockType(int x, int y, int z)
    {
        /*if(y < 33)
            return BlockType.Dirt;
        else
            return BlockType.Air;*/



        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex((seed+x)*.8f, (seed + z) *.8f)*14;
        float simplex2 = noise.GetSimplex((seed + x) * 3.5f, (seed + z) * 3.5f) * 10*(noise.GetSimplex((seed + x) *2f, (seed + z) *2f)+.5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = TerrainChunk.chunkHeight * .5f + heightMap;


        //stone layer heightmap
        float simplexStone1 = noise.GetSimplex((seed + x) * 1f, (seed + z) * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex((seed + x) * 5f, (seed + z) * 5f)+.5f) * 20 * (noise.GetSimplex((seed + x) * .3f, (seed + z) * .3f) + .5f);

        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = TerrainChunk.chunkHeight * .25f + stoneHeightMap - 1;


        BlockType blockType = BlockType.Air;

        //under the surface, dirt block
        if(y <= baseLandHeight)
        {
            blockType = BlockType.Dirt;

            //just on the surface, use a grass type
            if(y > baseLandHeight - 1 && y > WaterChunk.waterHeight-1)
                blockType = BlockType.Grass;

            if(y <= baseStoneHeight)
                blockType = BlockType.Stone;
        }

        return blockType;
    }


    ChunkPos curChunk = new ChunkPos(-1,-1);
    void LoadChunks(bool instant = false)
    {
        //the current chunk the player is in
        int curChunkPosX = Mathf.FloorToInt(player.position.x/16)*16;
        int curChunkPosZ = Mathf.FloorToInt(player.position.z/16)*16;

        //entered a new chunk
        if(curChunk.x != curChunkPosX || curChunk.z != curChunkPosZ)
        {
            curChunk.x = curChunkPosX;
            curChunk.z = curChunkPosZ;


            for(int i = curChunkPosX - gridDist * Render_Distance; i <= curChunkPosX + gridDist * Render_Distance; i += 16)
                for(int j = curChunkPosZ - gridDist * Render_Distance; j <= curChunkPosZ + gridDist * Render_Distance; j += 16)
                {
                    ChunkPos cp = new ChunkPos(i, j);

                    if(!chunks.ContainsKey(cp) && !toGenerate.Contains(cp))
                    {
                        if(instant)
                            BuildChunk(i, j);
                        else
                            toGenerate.Add(cp);
                    }
                     

                }

            //remove chunks that are too far away
            List<ChunkPos> toDestroy = new List<ChunkPos>();
            //unload chunks
            foreach(KeyValuePair<ChunkPos, TerrainChunk> c in chunks)
            {
                ChunkPos cp = c.Key;
                if(Mathf.Abs(curChunkPosX - cp.x) > 16 * (Render_Distance + 3) || 
                    Mathf.Abs(curChunkPosZ - cp.z) > 16 * (Render_Distance + 3))
                {
                    toDestroy.Add(c.Key);
                }
            }

            //remove any up for generation
            foreach(ChunkPos cp in toGenerate)
            {
                if(Mathf.Abs(curChunkPosX - cp.x) > 16 * (Render_Distance + 1) ||
                    Mathf.Abs(curChunkPosZ - cp.z) > 16 * (Render_Distance + 1))
                    toGenerate.Remove(cp);
            }

            foreach(ChunkPos cp in toDestroy)
            {
                chunks[cp].gameObject.SetActive(false);
                pooledChunks.Add(chunks[cp]);
                chunks.Remove(cp);
            }

            StartCoroutine(DelayBuildChunks());
        }




    }


    IEnumerator DelayBuildChunks()
    {
        while(toGenerate.Count > 0)
        {
            BuildChunk(toGenerate[0].x, toGenerate[0].z);
            toGenerate.RemoveAt(0);

            yield return new WaitForSeconds(.2f);

        }

    }


}


public struct ChunkPos
{
    public int x, z;
    public ChunkPos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}