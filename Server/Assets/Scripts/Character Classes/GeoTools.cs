#region License

// // GeoTools.cs
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

public class GeoTools
{
    /*
	int 		MAX_VERTS = 65000;
	int 		MAX_TRIS = 666000;
	int 		curVert = 0;
	int 		curIndex = 0;
	Vector3[] 	normals;
	Vector3[] 	vertices;
	Vector2[] 	uvs;
	int[] 		indices;
	int 		chunkDim = 16;
	float		genScaleY = 10.3f;
	float 		noiseRes = 0.06f;
	
	void emitCube (float x, float y, float z)
	{
		int firstVert = curVert;
		
		vertices [curVert++] = new Vector3 (x, y, z);
		vertices [curVert++] = new Vector3 (x + 1, y, z);
		vertices [curVert++] = new Vector3 (x + 1, y + 1, z);
		vertices [curVert++] = new Vector3 (x, y + 1, z);
		
		vertices [curVert++] = new Vector3 (x, y, z + 1);
		vertices [curVert++] = new Vector3 (x + 1, y, z + 1);
		vertices [curVert++] = new Vector3 (x + 1, y + 1, z + 1);
		vertices [curVert++] = new Vector3 (x, y + 1, z + 1);
		
		int uvert = firstVert;
		uvs [uvert++] = new Vector2 (x, z);
		uvs [uvert++] = new Vector2 (x + 1, z);
		uvs [uvert++] = new Vector2 (x + 1, z);
		uvs [uvert++] = new Vector2 (x, z);
		uvs [uvert++] = new Vector2 (x, z + 1);
		uvs [uvert++] = new Vector2 (x + 1, z + 1);
		uvs [uvert++] = new Vector2 (x + 1, z + 1);
		uvs [uvert++] = new Vector2 (x, z + 1);
		
		indices [curIndex++] = firstVert + 0;
		indices [curIndex++] = firstVert + 3;
		indices [curIndex++] = firstVert + 2;
		indices [curIndex++] = firstVert + 2;
		indices [curIndex++] = firstVert + 1;
		indices [curIndex++] = firstVert + 0;

		indices [curIndex++] = firstVert + 0 + 4;
		indices [curIndex++] = firstVert + 1 + 4;
		indices [curIndex++] = firstVert + 2 + 4;
		indices [curIndex++] = firstVert + 2 + 4;
		indices [curIndex++] = firstVert + 3 + 4;
		indices [curIndex++] = firstVert + 0 + 4;
		
		
		indices [curIndex++] = firstVert + 0;
		indices [curIndex++] = firstVert + 1;
		indices [curIndex++] = firstVert + 1 + 4;
		indices [curIndex++] = firstVert + 1 + 4;
		indices [curIndex++] = firstVert + 0 + 4;
		indices [curIndex++] = firstVert + 0;
		
		indices [curIndex++] = firstVert + 1;
		indices [curIndex++] = firstVert + 2;
		indices [curIndex++] = firstVert + 2 + 4;
		indices [curIndex++] = firstVert + 2 + 4;
		indices [curIndex++] = firstVert + 1 + 4;
		indices [curIndex++] = firstVert + 1;
		
		indices [curIndex++] = firstVert + 2;
		indices [curIndex++] = firstVert + 3;
		indices [curIndex++] = firstVert + 3 + 4;
		indices [curIndex++] = firstVert + 3 + 4;
		indices [curIndex++] = firstVert + 2 + 4;
		indices [curIndex++] = firstVert + 2;
		
		indices [curIndex++] = firstVert + 3;
		indices [curIndex++] = firstVert + 0;
		indices [curIndex++] = firstVert + 0 + 4;
		indices [curIndex++] = firstVert + 0 + 4;
		indices [curIndex++] = firstVert + 3 + 4;
		indices [curIndex++] = firstVert + 3;
	}

	
	void genFunction (ref Vector3 vec)
	{
		vec.y = (float)SimplexNoise.noise (vec.x * 0.01f, 0.0f, vec.z * 0.01f) * genScaleY;
//		vec.y = (sin(vec.x*0.1f)+(cos(vec.z*0.12f)*2.0f)+cos(vec.x*0.3f)*sin(vec.z*0.133f)*2.0f)*genScaleY;
	}
	void emitGrid (float gx, float gy, float gz, int chunkDim)
	{
		int firstVert = curVert;
		for (int x=0; x<(chunkDim+1); x++) {
			for (int z=0; z<(chunkDim+1); z++) {
				vertices [curVert] = new Vector3 (x + gx * 1.0f, 0, z + gz * 1.0f);
				genFunction (ref vertices [curVert]);
				uvs [curVert++] = new Vector2 (x, z);
			}
		}
		for (int x=0; x<chunkDim; x++) {
			for (int z=0; z<chunkDim; z++) {
				int cornerVert = firstVert + (z * (chunkDim + 1)) + x;
				indices [curIndex++] = cornerVert + 0;
				indices [curIndex++] = cornerVert + 1;
				indices [curIndex++] = cornerVert + chunkDim + 1;
		
				indices [curIndex++] = cornerVert + 1;
				indices [curIndex++] = cornerVert + chunkDim + 2;
				indices [curIndex++] = cornerVert + chunkDim + 1;
			}
		}
	}
	
	void buildCubesOnNoise(){
		
		for(int rx=0;rx<chunkDim;rx++)
		for(int ry=0;ry<chunkDim;ry++)
		for(int rz=0;rz<chunkDim;rz++){
			if(((SimplexNoise.noise(noiseRes*rx,noiseRes*ry,noiseRes*rz)>0.5) &&
				(SimplexNoise.noise(noiseRes*rx,noiseRes*ry,noiseRes*(rz+1))<=0.5))||
					((SimplexNoise.noise(noiseRes*(rx+1),noiseRes*ry,noiseRes*rz)>0.5) &&
				(SimplexNoise.noise(noiseRes*(rx+1),noiseRes*ry,noiseRes*rz)<=0.5))||
					((SimplexNoise.noise(noiseRes*rx,noiseRes*(ry+1),noiseRes*rz)>0.5) &&
				(SimplexNoise.noise(noiseRes*rx,noiseRes*(ry+1),noiseRes*rz)<=0.5))||
					((SimplexNoise.noise(noiseRes*rx,noiseRes*ry,noiseRes*rz)<0.5) &&
				(SimplexNoise.noise(noiseRes*rx,noiseRes*ry,noiseRes*(rz+1))>=0.5))||
					((SimplexNoise.noise(noiseRes*(rx+1),noiseRes*ry,noiseRes*rz)<0.5) &&
				(SimplexNoise.noise(noiseRes*(rx+1),noiseRes*ry,noiseRes*rz)>=0.5))||
					((SimplexNoise.noise(noiseRes*rx,noiseRes*(ry+1),noiseRes*rz)<0.5) &&
				(SimplexNoise.noise(noiseRes*rx,noiseRes*(ry+1),noiseRes*rz)>=0.5)||
					((SimplexNoise.noise(noiseRes*rx,noiseRes*ry,noiseRes*rz)<0.5) &&
				(SimplexNoise.noise(noiseRes*(rx+1),noiseRes*(ry+1),noiseRes*rz+1))>=0.5)))
			//if(SimplexNoise.noise(noiseRes*rx,noiseRes*ry,noiseRes*rz)>0.5)
				emitCube(rx,ry,rz);
		}
	}	
	*/
}