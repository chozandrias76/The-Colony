#region License

// // World.cs
// //  
// //  Author:
// //        <colin.p.swensonh@gmail.com>
// // 
// //  Copyright (c) 2013 swensonhcp
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class World
{
    public delegate float ToolFunction(ref GridVertex g, Vector3 dlt, float editRadius, float er2, float targDelta);

    private static int MAX_VERTS = 65000;
    private static int MAX_TRIS = 666000;
    private static int chunkDim = 10;

    private static readonly Random rand = new Random();


    private static readonly int cacheRowSize = (chunkDim + 3);
    private static readonly int ox = (cacheRowSize*cacheRowSize);
    private static readonly int oy = cacheRowSize;
    private static int oz = 1;

    private static readonly int chunkVertexCount = (chunkDim + 3)*(chunkDim + 3)*(chunkDim + 3);

    private static readonly int[] normalNeighbors =
    {
        caddr(0, 0, -1), caddr(0, 0, 1),
        caddr(0, -1, 0), caddr(0, 1, 0),
        caddr(-1, 0, 0), caddr(1, 0, 0)
    };

    private static readonly int[] cacheNeighbors =
    {
        caddr(0, 0, 0), caddr(1, 0, 0), caddr(1, 1, 0), caddr(0, 1, 0),
        caddr(0, 0, 1), caddr(1, 0, 1), caddr(1, 1, 1), caddr(0, 1, 1)
    };

    private readonly MCubes.GRIDCELL cell = new MCubes.GRIDCELL();
    private readonly List<Region> deadRegions = new List<Region>();

    //GridVertex[]	chunkVertices=new GridVertex[chunkVertexCount];

    private readonly Dictionary<string, Region> loadedRegions = new Dictionary<string, Region>();

    private readonly int[] rgnCubeIndices =
    {
        0, 1, 1, 2, 2, 3, 3, 0,
        4, 5, 5, 6, 6, 7, 7, 4,
        0, 4, 1, 5, 2, 6, 3, 7
    };

    private readonly Vector3[] rgnCubeVerts =
    {
        new Vector3(0, 0, 0),
        new Vector3(chunkDim, 0, 0),
        new Vector3(chunkDim, chunkDim, 0),
        new Vector3(0, chunkDim, 0),
        new Vector3(0, 0, chunkDim),
        new Vector3(chunkDim, 0, chunkDim),
        new Vector3(chunkDim, chunkDim, chunkDim),
        new Vector3(0, chunkDim, chunkDim)
    };

    private readonly PangoWorld root;

    private readonly Dictionary<Region, bool> visibleMap = new Dictionary<Region, bool>();
    private int curIndex;
    private int curVert;
    private int depthSweep = 10;
    public List<Region> geometryQueue = new List<Region>();
    private int[] indices = new int[MAX_TRIS]; //[chunkDim*chunkDim * 6];
    public List<Region> loadingQueue = new List<Region>();
    private float matNoiseRes = 0.1f;
    private float noiseRes = 0.03f;
    private Vector3[] normals = new Vector3[MAX_VERTS]; //(chunkDim+1)*(chunkDim+1)];
    private int planarSweep = 2;
    private List<Region> regionCollisions = new List<Region>();
    public string regionName;
    private int[] tmpMax = new int[3];
    private int[] tmpMin = new int[3];
    private Vector2[] uvs = new Vector2[MAX_VERTS]; //[(chunkDim+1)*(chunkDim+1)];
    private Vector3[] vertices = new Vector3[MAX_VERTS]; //(chunkDim+1)*(chunkDim+1)];
    private int viewRX;
    private int viewRY;
    private int viewRZ;
    private int[] viewRgnCoord = new int[3];

    public World(PangoWorld _root)
    {
//		UnityEngine.Object[]	rezzes=Resources.FindObjectsOfTypeAll(typeof( Mesh));
//		UnityEngine.Object[]	meshes=GameObject.FindObjectsOfTypeIncludingAssets(typeof( Mesh));

        root = _root;
    }

    public static float frand()
    {
        return (float) rand.NextDouble();
    }

    public static float fmax(float a, float b)
    {
        return a > b ? a : b;
    }

    public static float fmin(float a, float b)
    {
        return a < b ? a : b;
    }

    private static float sin(float val)
    {
        return (float) Math.Sin(val);
    }

    private static float cos(float val)
    {
        return (float) Math.Cos(val);
    }

    private void capVolumeDomain(int rx, int ry, int rz, ref MCubes.GRIDCELL cell)
    {
        if (rx == 0) cell.val[0] = cell.val[3] = cell.val[4] = cell.val[7] = 0.0;
        else if (rx == chunkDim - 1) cell.val[1] = cell.val[2] = cell.val[5] = cell.val[6] = 0.0;
        if (ry == 0) cell.val[0] = cell.val[1] = cell.val[4] = cell.val[5] = 0.0;
        else if (ry == chunkDim - 1) cell.val[2] = cell.val[3] = cell.val[6] = cell.val[7] = 0.0;
        if (rz == 0) cell.val[0] = cell.val[1] = cell.val[2] = cell.val[3] = 0.0;
        else if (rz == chunkDim - 1) cell.val[4] = cell.val[5] = cell.val[6] = cell.val[7] = 0.0;
    }


    private float evaluateFieldOriginal(ref float nx, ref float ny, ref float nz)
    {
        float densityVariance =
            1.0f;
        densityVariance = ((float) SimplexNoise.noise(nx*densityVariance, ny*densityVariance, nz*densityVariance) - 0.5f)*
                          -0.5f;
        float noiseScl = 0.2f;
        //((float)SimplexNoise.noise (nx*densityVarianceScale, ny*densityVarianceScale, nz*densityVarianceScale)-0.5f)*-0.5f;
        float nv = (float) SimplexNoise.noise(nx, ny, nz)*densityVariance;
        var nv2 = (float) SimplexNoise.noise(nx*noiseScl, ny*noiseScl, nz*noiseScl);
        //gv = ny;		//Add ground
        nv += (ny*-3.4f);
        //Add noise
        nv = nv + (nv2*0.5f);
        return fmin(fmax(nv, 0.0f), 1.0f);
    }

    private float evaluateFieldNiceTerrain(ref float nx, ref float ny, ref float nz)
    {
        float densityVariance = 1.0f;
        //((float)SimplexNoise.noise (nx*densityVarianceScale, ny*densityVarianceScale, nz*densityVarianceScale)-0.5f)*-0.5f;
        float nv = (float) SimplexNoise.noise(nx, ny, nz)*densityVariance;
        var nv2 = (float) SimplexNoise.noise(nx*6.1f, ny*6.1f, nz*6.1f);
        //gv = ny;		//Add ground
        nv += (ny*-1.4f);
        //Add noise
        nv = nv + (nv2*0.1f);
        return fmin(fmax(nv, 0.0f), 1.0f);
    }

    private float evaluateField(ref float nx, ref float ny, ref float nz)
    {
        float densityVariance = 1.0f;
        //((float)SimplexNoise.noise (nx*densityVarianceScale, ny*densityVarianceScale, nz*densityVarianceScale)-0.5f)*-0.5f;
        float nv = (float) SimplexNoise.noise(nx, ny, nz)*densityVariance;
        var nv2 = (float) SimplexNoise.noise(nx*6.1f, ny*6.1f, nz*6.1f);
        //gv = ny;		//Add ground
        nv += (ny*-1.4f);
        //Add noise
        nv = nv + (nv2*0.1f);
        return fmin(fmax(nv, 0.0f), 1.0f);
    }

    private Vector3[] v3subarray(ref Vector3[] v, int len)
    {
        var gen = new Vector3[len];
        Array.Copy(v, gen, len);
        return gen;
    }

    private Vector2[] v2subarray(ref Vector2[] v, int len)
    {
        var gen = new Vector2[len];
        Array.Copy(v, gen, len);
        return gen;
    }

    private int[] vIntsubarray(ref int[] v, int len)
    {
        var gen = new int[len];
        Array.Copy(v, gen, len);
        return gen;
    }

    private Vector3 vecD3(double dx, double dy, double dz)
    {
        return new Vector3((float) dx, (float) dy, (float) dz);
    }

    private float snoise(float x, float y, float z)
    {
        return (float) SimplexNoise.noise(x, y, z);
    }

    private void initializeField(int x, int y, int z, ref GridVertex[] chunkVertices)
    {
        int vtop = 0;
        //Build the procedural field for this chunk...
        for (int rx = -1; rx <= chunkDim + 1; rx++)
            for (int ry = -1; ry <= chunkDim + 1; ry++)
                for (int rz = -1; rz <= chunkDim + 1; rz++)
                {
                    chunkVertices[vtop].position.Set(rx, ry, rz);

                    float nx = (x + rx)*noiseRes;
                    float ny = (y + ry)*noiseRes;
                    float nz = (z + rz)*noiseRes;

                    //	float d= 
                    chunkVertices[vtop].density = evaluateField(ref nx, ref ny, ref nz);

                    float n2sz = matNoiseRes;
                    chunkVertices[vtop].material.Set( //0.7f,
                        (snoise(nx*n2sz, ny*n2sz, 0.0f)*0.7f) +
                        (snoise(ny*3.0f, nx*3.0f, 0.0f)*0.3f)
                        //(snoise (nx*n2sz, ny*n2sz, 0.0f)*0.7f)+
                        //(snoise (ny*(n2sz*2), nx*(n2sz*2), 0.0f)*0.3f)
                        ,
                        snoise(ny*n2sz, nx*n2sz, 0.0f));

                    vtop++;
                }
    }

    private static int caddr(int cx, int cy, int cz)
    {
        return (cx*cacheRowSize*cacheRowSize) + (cy*cacheRowSize) + cz;
    }

    private void generateNormalField(ref GridVertex[] chunkVertices)
    {
        int cbase = ox + oy + oz;

        //Compute normals on volume verts...	
        for (int rx = 0; rx <= chunkDim; rx++)
        {
            int cplane = cbase;
            for (int ry = 0; ry <= chunkDim; ry++)
            {
                int cidx = cplane;
                for (int rz = 0; rz <= chunkDim; rz++)
                {
                    float nmlx = chunkVertices[cidx + normalNeighbors[0]].density -
                                 chunkVertices[cidx + normalNeighbors[1]].density;
                    float nmly = chunkVertices[cidx + normalNeighbors[2]].density -
                                 chunkVertices[cidx + normalNeighbors[3]].density;
                    float nmlz = chunkVertices[cidx + normalNeighbors[4]].density -
                                 chunkVertices[cidx + normalNeighbors[5]].density;
                    chunkVertices[cidx].normal = new Vector3(nmlx, nmly, nmlz);
                    chunkVertices[cidx].normal.Normalize();
                    cidx += oz;
                }
                cplane += oy;
            }
            cbase += ox;
        }
    }


    private static float fabs(float f)
    {
        return f < 0.0f ? -1.0f*f : f;
    }

    public static float cubeTool(ref GridVertex g, Vector3 dlt, float editRadius, float er2, float targDelta)
    {
        //Cube tool...
        float d = g.density;
        if (g.material.x > 0.4f &&
            fabs(dlt.x) < editRadius &&
            fabs(dlt.y) < editRadius &&
            fabs(dlt.z) < editRadius)
        {
            d = d + targDelta;
            d = dclamp(d + targDelta);
            g.density = d;
        }
        return d;
    }

    public static float sphereTool(ref GridVertex g, Vector3 dlt, float editRadius, float er2, float targDelta)
    {
        //Sphere tool...
        float d = g.density;
        if (g.material.x > 0.4f &&
            dlt.sqrMagnitude < er2)
        {
            // add/sub a sphere from the  density field
            //if(dlt.y<0.2f)	//Comment this in to get flat top sphere drawing
            d = dclamp(d + ((1.0f - (dlt.magnitude/editRadius))*targDelta)); //Effect this element...
            g.density = d;
        }
        return d;
    }

    private float modifyRegion(Region rgn, Vector3 origin, float editRadius, float targDelta, ToolFunction tool)
    {
        int cbase = 0;
        //ox+oy+oz;
        //cbase*=2;
        float densityChange = 0.0f;
        float er2 = editRadius*editRadius;
        for (int rx = 0; rx <= chunkDim + 2; rx++)
        {
            int cplane = cbase;
            for (int ry = 0; ry <= chunkDim + 2; ry++)
            {
                int cidx = cplane;
                for (int rz = 0; rz <= chunkDim + 2; rz++)
                {
                    Vector3 pos = rgn.data[cidx].position + rgn.origin;
                    Vector3 dlt = pos - origin;
                    float d = rgn.data[cidx].density;
                    densityChange += tool(ref rgn.data[cidx], dlt, editRadius, er2, targDelta) - d;

                    //if(d>0.5f)
                    //	rgn.data[cidx].material.x=1.5f;

                    cidx += oz;
                }
                cplane += oy;
            }
            cbase += ox;
        }
        return densityChange;
    }


    private void generateMeshGeometry(ref GridVertex[] chunkVertices, ref Mesh mesh, Vector3 originWorld, bool firstTime)
    {
        curVert = 0;
        curIndex = 0;
        int cbase = ox + oy + oz;

        //Loop through the density volume and call marching cubes on each cube... 
        for (int rx = 0; rx < chunkDim; rx++)
        {
            int cplane = cbase;
            for (int ry = 0; ry < chunkDim; ry++)
            {
                int cidx = cplane;
                for (int rz = 0; rz < chunkDim; rz++)
                {
                    for (int ax = 0; ax < 8; ax++)
                    {
                        GridVertex gv = chunkVertices[cidx + cacheNeighbors[ax]];
                        cell.p[ax] = gv.position;
                        cell.n[ax] = gv.normal;
                        cell.t[ax] = gv.material;
                        cell.val[ax] = gv.density;
                    }
                    int triBase = curIndex;
                    MCubes.Polygonise(cell, 0.5, ref vertices, ref normals, ref uvs, ref indices, ref curVert,
                        ref curIndex);

                    if (firstTime)
                    {
                        if (triBase < curIndex)
                        {
                            //If a triangle is being emitted, check its normal.. if its suitably flat, do random chance of object spawn...
                            GridVertex gv = chunkVertices[cidx + cacheNeighbors[0]];
                            if (gv.normal.y > 0.819f)
                            {
                                //Flatish surface... potentially emit something..
                                if (gv.normal.y < 0.82f)
                                {
                                    /*
									GameObject go=(GameObject)GameObject.Instantiate(Resources.Load("Objs/TurretBase"));//CreatePrimitive (PrimitiveType.Sphere);
									go.transform.position=gv.position+originWorld;
									go.GetComponent<Turret>().enabled=true;
                                     */
                                }
                            }
                        }
                    }
                    cidx += oz;
                }
                cplane += oy;
            }
            cbase += ox;
        }

        //Make a mesh to hold this chunk and assign the verts/tris
        mesh.Clear(true);
        mesh.vertices = v3subarray(ref vertices, curVert);
        mesh.uv = v2subarray(ref uvs, curVert);
        mesh.normals = v3subarray(ref normals, curVert);
        mesh.triangles = vIntsubarray(ref indices, curIndex);
        //mesh.RecalculateNormals ();
    }

    /*
	Mesh generateChunk2 (int x, int y, int z)
	{
		
		curVert = 0;
		curIndex = 0;
		
		for (int rx=0; rx<chunkDim; rx++)
			for (int ry=0; ry<chunkDim; ry++)
				for (int rz=0; rz<chunkDim; rz++) {
			
					cell.p [0] = new Vector3 (rx, ry, rz);
					cell.p [1] = new Vector3 (rx + 1, ry, rz);
					cell.p [2] = new Vector3 (rx + 1, ry + 1, rz);
					cell.p [3] = new Vector3 (rx, ry + 1, rz);
					cell.p [4] = new Vector3 (rx, ry, rz + 1);
					cell.p [5] = new Vector3 (rx + 1, ry, rz + 1);
					cell.p [6] = new Vector3 (rx + 1, ry + 1, rz + 1);
					cell.p [7] = new Vector3 (rx, ry + 1, rz + 1);

					for (int t=0; t<8; t++) {
						float nx = (cell.p [t].x + x) * noiseRes;
						float ny = (cell.p [t].y + y) * noiseRes;
						float nz = (cell.p [t].z + z) * noiseRes;
				
						cell.val [t] = evaluateField(ref nx,ref ny,ref nz);
				
						cell.n[t] = new Vector3(0,1,0);//evaluateFieldNormal(ref nx,ref ny,ref nz);
						//Following code caps the volume at the edge of the chunk...
						//capVolumeDomain(rx,ry,rz,ref cell);
					}
					MCubes.Polygonise2 (cell, 0.5, ref vertices, ref normals, ref uvs, ref indices, ref curVert, ref curIndex);
				}
		
		
		//Make a mesh to hold this chunk and assign the verts/tris
		Mesh mesh = new Mesh ();
		
		mesh.vertices = v3subarray(ref vertices,curVert);
		mesh.uv = v2subarray(ref uvs,curVert);
		mesh.triangles = vIntsubarray(ref indices,curIndex);
		mesh.normals = v3subarray(ref normals,curVert);

		//mesh.RecalculateNormals ();
		
		return mesh;
	}
	*/


    private void buildRegionData(GameObject archetype, ref Region chunk)
    {
        int[] ccoord = chunk.coord;
        int x = ccoord[0];

        int y = ccoord[1];

        int z = ccoord[2];
        //z = (int)((5 / 4) * (Mathf.Log10(Mathf.Tan((1 / 4) * Mathf.PI + (2 / 5) * z))));
        //Generate the mesh from simplex noise terrain algorithm...

        int rgnx = x*chunkDim;
        int rgny = y*chunkDim;
        int rgnz = z*chunkDim;

        if (chunk.data == null)
        {
            //Generate the field data for this chunk for the first time...
            chunk.data = new GridVertex[chunkVertexCount];
            initializeField(rgnx, rgny, rgnz, ref chunk.data);
        }
    }

    private void generateMesh(Region chunk, GameObject archetype)
    {
        bool virginRegion = false;
        if (chunk.obj == null)
        {
            chunk.obj = new GameObject(chunk.key);
            chunk.obj.transform.position =
                chunk.origin = new Vector3(chunk.coord[0]*chunkDim, chunk.coord[1]*chunkDim, chunk.coord[2]*chunkDim);
            chunk.obj.AddComponent<MeshRenderer>();
            chunk.obj.AddComponent<MeshFilter>();
            chunk.obj.AddComponent<MeshCollider>();
            chunk.obj.tag = "Finish";
            virginRegion = true;
        }

        generateNormalField(ref chunk.data);
        var mesh = new Mesh();
        generateMeshGeometry(ref chunk.data, ref mesh, chunk.origin, virginRegion);
        chunk.obj.GetComponent<MeshFilter>().mesh = mesh;
        chunk.obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        //chunk.AddComponent(chunk.GetComponent<"Material">().);
        //Material mat = new Material (Shader.Find ("Diffuse"));
        chunk.obj.GetComponent<MeshRenderer>().sharedMaterial = archetype.GetComponent<MeshRenderer>().sharedMaterial;
    }

//	void	buildRegionMesh(GameObject archetype,ref Region chunk)
//	{
//		generateMesh(chunk,archetype,ref chunk.data);
//	}

    public string getRegionName(int x, int y, int z)
    {
        return "chunk " + "X: " + x + " Y: " + y + " Z: " + z;
    }

    public Region getRegion(GameObject archetype, int x, int y, int z)
    {
        //Gets the chunk located at chunk coordinat x,y,z
        string ckName = getRegionName(x, y, z);
        Region chunk;
        if (loadedRegions.ContainsKey(ckName))
        {
            chunk = loadedRegions[ckName];
            if (chunk.data == null)
            {
                if (chunk.inLoadQueue == false)
                {
                    loadingQueue.Add(chunk);
                    chunk.inLoadQueue = true;
                }
            }
            else if (chunk.obj == null)
            {
                if (chunk.inGeomQueue == false)
                {
                    geometryQueue.Add(chunk);
                    chunk.inGeomQueue = true;
                }
            }
        }
        else
        {
            chunk = new Region(ckName);
            chunk.coord = new[] {x, y, z};
            chunk.inLoadQueue = true;
            loadingQueue.Add(chunk);
            loadedRegions[ckName] = chunk;
        }
        return chunk;
    }


    public bool isRegionLoaded(string name)
    {
        return loadedRegions.ContainsKey(name);
    }

    public void updateGenerator()
    {
        if (geometryQueue.Count > 0)
        {
            Region topObj = geometryQueue[geometryQueue.Count - 1];
            geometryQueue.RemoveRange(geometryQueue.Count - 1, 1);
            generateMesh(topObj, root.gameObject);
            topObj.inGeomQueue = false;
        }
        if (loadingQueue.Count > 0)
        {
            Region topObj = loadingQueue[loadingQueue.Count - 1];
            loadingQueue.RemoveRange(loadingQueue.Count - 1, 1);
            if (topObj.data == null)
            {
                //Look for file here...
                buildRegionData(root.gameObject, ref topObj);
            }
            topObj.inLoadQueue = false;
            if (topObj.inGeomQueue == false)
            {
                topObj.inGeomQueue = true;
                geometryQueue.Add(topObj);
            }
        }
    }

    /*
	void destroyDeadRegions(){
		foreach(Region rg in deadRegions){
			loadingQueue.Remove(rg);
			geometryQueue.Remove(rg);
			rg.inGeomQueue=rg.inLoadQueue=false;
//			loadedRegions.Remove(rg.key);
			//if(rg.obj!=null)
			{
				//MeshCollider 	mc=rg.obj.GetComponent<MeshCollider>();
				//MeshFilter 		mf=rg.obj.GetComponent<MeshFilter>();
				//mc.sharedMesh = null;
				//Destroy(mf.mesh);
				//mf.mesh = null;
				PangoWorld.Destroy(rg.obj);
				rg.obj=null;
			}
		}
		if(deadRegions.Count>0){
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
		deadRegions.Clear();
	}
     */

    public void updateCache()
    {
        //Debug.Log ("Generating Terrain....");

        regionName = getRegionName(viewRX, viewRY, viewRZ);
        visibleMap.Clear();
        for (int tx = -planarSweep; tx <= planarSweep; tx++)
            for (int ty = -depthSweep; ty <= depthSweep; ty++)
                for (int tz = -planarSweep; tz <= planarSweep; tz++)
                {
                    Region chunk = getRegion(root.gameObject, tx + viewRX, ty + viewRY, tz + viewRZ);
                    visibleMap[chunk] = true;
                    chunk.isVisible = true;
                }
        //	GetComponent<MeshFilter> ().mesh = mesh;
        //	GetComponent<MeshCollider>().sharedMesh = mesh;
        //Debug.Log("Terrain finished.");

        foreach (Region rg in loadedRegions.Values)
        {
            if (visibleMap.ContainsKey(rg) == false)
            {
                deadRegions.Add(rg);
                rg.isVisible = false;
            }
        }
        //destroyDeadRegions();
    }

    private static float dclamp(float density)
    {
        //return density>0.5f?1.0f:0.0f;
        return fmin(1.0f, fmax(0.0f, density));
    }

    public void getRgnCoord(Vector3 pos, ref int[] coord)
    {
        coord[0] = (int) Math.Floor(pos.x/chunkDim);
        coord[1] = (int) Math.Floor(pos.y/chunkDim);
        coord[2] = (int) Math.Floor(pos.z/chunkDim);
    }

    public float editWorld(GameObject archetype, Vector3 point, float editRadius, float editValue,
        ToolFunction tool)
    {
        regionCollisions.Clear();

        getCollidingRegions(point, editRadius, ref regionCollisions);
        bool validEdit = true;
        foreach (Region r in regionCollisions)
        {
            if (r.data == null)
            {
                Debug.Log("Chunk not loaded, edit cancelled..");
                validEdit = false;
                break;
            }
        }
        float sumDensity = 0.0f;
        if (validEdit)
        {
            foreach (Region rgn in regionCollisions)
            {
                sumDensity += modifyRegion(rgn, point, editRadius, editValue, tool);
                generateMesh(rgn, archetype);
                //hilightRegion(rgn,Color.blue,1.0f);
            }
        }
        //Region chunk=getChunk (this.gameObject, viewRX, viewRY, viewRZ);
        //if(chunk!=null && chunk.data!=null){
        //	modifyChunk(ref chunk.data,ref chunk.origin);
        //	generateMesh(ref chunk,this.gameObject,ref chunk.data);
        //	Debug.Log ("Rebuilt chunk:"+chunk.key);
        //}
        return sumDensity;
    }


    private void getCollidingRegions(Vector3 pmin, Vector3 pmax, ref List<Region> colliders)
    {
        getRgnCoord(pmin, ref tmpMin);
        getRgnCoord(pmax, ref tmpMax);
        for (int tx = tmpMin[0]; tx <= tmpMax[0]; tx++)
            for (int ty = tmpMin[1]; ty <= tmpMax[1]; ty++)
                for (int tz = tmpMin[2]; tz <= tmpMax[2]; tz++)
                    colliders.Add(getRegion(root.gameObject, tx, ty, tz));
    }


    private void getCollidingRegions(Vector3 pos, float radius, ref List<Region> colliders)
    {
        radius += 1.0f; //Pad every search to get 
        var vrad = new Vector3(radius, radius, radius);
        Vector3 pmin = pos - vrad;
        Vector3 pmax = pos + vrad;
        getCollidingRegions(pmin, pmax, ref colliders);
    }

    private void hilightRegion(Vector3 pos, Color color, float duration)
    {
        getRgnCoord(pos, ref viewRgnCoord);
        Vector3 rgnOrg = new Vector3(viewRgnCoord[0], viewRgnCoord[1], viewRgnCoord[2])*chunkDim;
        for (int t = 0; t < 24; t += 2)
        {
            Debug.DrawLine(rgnOrg + rgnCubeVerts[rgnCubeIndices[t]], rgnOrg + rgnCubeVerts[rgnCubeIndices[t + 1]], color,
                duration);
        }
    }

    private void hilightRegion(Region rgn, Color color, float duration)
    {
        Vector3 rgnOrg = new Vector3(rgn.coord[0], rgn.coord[1], rgn.coord[2])*chunkDim;
        for (int t = 0; t < 24; t += 2)
        {
            Debug.DrawLine(rgnOrg + rgnCubeVerts[rgnCubeIndices[t]], rgnOrg + rgnCubeVerts[rgnCubeIndices[t + 1]], color,
                duration);
        }
    }

    public void primeCache()
    {
        regionName = getRegionName(viewRX, viewRY, viewRZ);
        updateCache();
        while ((loadingQueue.Count > 0) || (geometryQueue.Count > 0))
            updateGenerator();
    }

    public void update()
    {
        Vector3 cacheOrigin = (root.viewerObject != null) ? root.viewerObject.transform.position : Vector3.zero;
        getRgnCoord(cacheOrigin, ref viewRgnCoord);

        if ((viewRgnCoord[0] != viewRX) || (viewRgnCoord[1] != viewRY) || (viewRgnCoord[2] != viewRZ))
        {
            viewRX = viewRgnCoord[0];
            viewRY = viewRgnCoord[1];
            viewRZ = viewRgnCoord[2];
            //Debug.Log ("RgnChange:"+viewRX+":"+viewRY+":"+viewRZ);
            //hilightRegion (viewerObject.transform.position,Color.red,0.5f);
            updateCache();
        }
    }

    public struct GridVertex
    {
        public float density;
        public Vector2 material;
        public Vector3 normal;
        public Vector3 position;
    }

    public class Region
    {
        public int[] coord;
        public GridVertex[] data;
        public bool inGeomQueue;
        public bool inLoadQueue;
        public bool isVisible;
        public string key;
        public GameObject obj;
        public Vector3 origin;

        public Region(string _key)
        {
            key = _key;
        }
    };

    /*
	protected static GameObject[] getAllEditorAssets()
    {
	    List<GameObject> tempObjects = new List<GameObject>();
	    DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
	    FileInfo[] goFileInfo = directory.GetFiles("*.prefab", SearchOption.AllDirectories);
	    uint i = 0; 
		uint goFileInfoLength = goFileInfo.length;
	    FileInfo tempGoFileInfo; 
		String tempFilePath;
		int assetIndex;
	    GameObject tempGO;
	    for(i = 0; i < goFileInfoLength; i++)
	    {
	        tempGoFileInfo = goFileInfo[i] as FileInfo;
	        if(tempGoFileInfo == null) continue;            
	        tempFilePath = tempGoFileInfo.FullName;
	
	        assetIndex = tempFilePath.IndexOf("Assets/");
	        //assetIndex = tempFilePath.IndexOf("Assets\\");
	        if (assetIndex < 0) assetIndex = 0;         
	        tempFilePath = tempFilePath.Substring(assetIndex, tempFilePath.length - assetIndex);
	        //tempFilePath = tempFilePath.Replace('\\', '/');
	        tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, GameObject) as GameObject;
	        if(tempGO == null) continue;
	        tempObjects.push(tempGO);
	    }
	    return tempObjects.ToBuiltin(GameObject) as GameObject[];
	}
	*/
}