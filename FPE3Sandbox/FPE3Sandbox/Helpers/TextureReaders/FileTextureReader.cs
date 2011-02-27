using System;
using FarseerPhysics.Common;
using FarseerPhysicsBaseFramework.Helpers.FileReaders;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Helpers.TextureReaders
{
    class FileTextureReader:ITextureReader
    {
        private string filePath;
        private IFileReader fileReader;

        public FileTextureReader(string filePath)
        {
            this.filePath = filePath;
            fileReader = new FileReaderWP7();
        }

        public Vertices GetVertices()
        {
            var vertices = new Vertices();
            foreach (var line in fileReader.Read(filePath))
            {
                var parts = line.Replace(")", "").Replace("(", "").Split(',');
                float x = Convert.ToSingle(parts[0]), y = Convert.ToSingle(parts[1]);
                
                vertices.Add(new Vector2(x, y));
            }
            return vertices;
        }
    }
}
