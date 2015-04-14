using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    /// <summary>
    /// Denna klass skapar och ritar ut terrängen.
    /// </summary>
    static class Terrain
    {
        #region Fields
        static private VertexBuffer vertexBuffer;
        static private IndexBuffer indexBuffer;
        static private GraphicsDevice device;
        static private Texture2D terrainTexture;
        static private Effect effect;
        static private float textureScale, terrainSizeMultiplier;
        static private float[,] heights;
        #endregion

        #region LoadContent
        /// <summary>
        /// Denna metod laddar in height map och texturer till terräng och 
        /// anropar de metoder som skapar terrängen.
        /// </summary>
        /// <param name="graphicsDevice">Spelets Graphic Device från Game1.</param>
        /// <param name="content">Spelets Content Manager från Game1.</param>
        /// <param name="texScale"></param>
        /// <param name="terrainWidth">Bredden utav height mappen.</param>
        /// <param name="terrainHeight">Höjden utav height mappen.</param>
        /// <param name="heightScale">Skalan i vilken terrängens höjd kan röra sig inom.</param>
        /// <param name="terrainSizeMultp"></param>
        public static void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, float texScale, int terrainWidth, int terrainHeight, float heightScale, float terrainSizeMultp)
        {
            device = graphicsDevice;
            terrainTexture = content.Load<Texture2D>(@"Textures\grass");
            textureScale = texScale;
            terrainSizeMultiplier = terrainSizeMultp;

            effect = content.Load<Effect>(@"Effects/Terrain");

            ReadHeightMap(content.Load<Texture2D>(@"Textures\hmap512"), terrainWidth, terrainHeight, heightScale);
            BuildVertexBuffer(terrainWidth, terrainHeight, heightScale);
            BuildIndexBuffer(terrainWidth, terrainHeight);
        }
        #endregion

        #region Height Map
        /// <summary>
        /// Denna metod avläser height mappen och lägger höjdvärdena från denna i en array.
        /// </summary>
        /// <param name="heightMap">Height mappen som ska avläsas.</param>
        /// <param name="terrainWidth">Bredden utav height mappen.</param>
        /// <param name="terrainHeight">Höjden utav height mappen.</param>
        /// <param name="heightScale">Skalan i vilken terrängens höjd kan röra sig inom.</param>
        private static void ReadHeightMap(Texture2D heightMap, int terrainWidth, int terrainHeight, float heightScale)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            heights = new float[terrainWidth, terrainHeight];
            Color[] heightMapData = new Color[heightMap.Width * heightMap.Height];
            heightMap.GetData(heightMapData);
            for (int x = 0; x < terrainWidth; x++)
                for (int z = 0; z < terrainHeight; z++)
                {
                    byte height = heightMapData[x + z * terrainWidth].R;
                    heights[x, z] = (float)height / 255f;
                    max = MathHelper.Max(max, heights[x, z]);
                    min = MathHelper.Min(min, heights[x, z]);
                }
            float range = (max - min);
            for (int x = 0; x < terrainWidth; x++)
                for (int z = 0; z < terrainHeight; z++)
                {
                    heights[x, z] =
                    ((heights[x, z] - min) / range) * heightScale;
                }
        }
        #endregion

        #region Vertex Buffer
        /// <summary>
        /// Metoden skapar terrängen.
        /// </summary>
        /// <param name="width">Antalet vertex längs terrängens ena sida.</param>
        /// <param name="height">Antalet vertex längs terrängens andra sida.</param>
        /// <param name="heightScale"></param>
        private static void BuildVertexBuffer(int width, int height, float heightScale)
        {
            VertexPositionNormalTexture[] vertices =
            new VertexPositionNormalTexture[width * height];
            for (int x = 0; x < width; x++)
                for (int z = 0; z < height; z++)
                {
                    vertices[x + (z * width)].Position = new Vector3(x * terrainSizeMultiplier, heights[x, z] * terrainSizeMultiplier / 2, z * terrainSizeMultiplier);
                    vertices[x + (z * width)].TextureCoordinate =
                        new Vector2((float)x / textureScale, (float)z / textureScale);
                }
            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture),
                vertices.Length, BufferUsage.WriteOnly);

            vertexBuffer.SetData(vertices);
        }
        #endregion

        #region Index Buffer
        /// <summary>
        /// Metoden skapar ett index för kvadrater i terrängen.
        /// </summary>
        /// <param name="width">Antalet kvadrater längs terrängens ena sida.</param>
        /// <param name="height">Antalet kvadrater längs terrängens andra sida.</param>
        private static void BuildIndexBuffer(int width, int height)
        {
            int indexCount = (width - 1) * (height - 1) * 6;
            int[] indices = new int[indexCount];
            int counter = 0;
            for (int z = 0; z < height - 1; z++)
                for (int x = 0; x < height - 1; x++)
                {
                    int upperLeft = (int)(x + (z * width));
                    int upperRight = (int)(upperLeft + 1);
                    int lowerLeft = (int)(upperLeft + width);
                    int lowerRight = (int)(upperLeft + width + 1);
                    indices[counter++] = upperLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;
                    indices[counter++] = upperLeft;
                    indices[counter++] = upperRight;
                    indices[counter++] = lowerRight;
                }
            indexBuffer = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits,
            indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);
        }
        #endregion

        #region Draw
        /// <summary>
        /// Metoden ritar ut terrängen.
        /// </summary>
        /// <param name="camera">Spelarens kamera.</param>
        public static void Draw(Camera camera)
        {
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["terrainTexture1"].SetValue(terrainTexture);
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(vertexBuffer);
                device.Indices = indexBuffer;
                device.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                vertexBuffer.VertexCount,
                0,
                indexBuffer.IndexCount / 3);
            }
        }
        #endregion
    }
}