using System.IO;
using UnityEditor;
using Utilities;

namespace EditorExtensions.Utilities
{
    public static class IoUtilities
    {
        public static void SaveToFile(string path, ISerializable model)
        {
            using (var fileStream = File.OpenWrite(path))
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                    model.Serialize(writer);
                }
            }
        }
        
        public static T LoadFromFile<T>(string path, uint? signature = null) where T : class, ISerializable, new()
        {
            if (!File.Exists(path))
                return null;

            if (!path.EndsWith(".bytes"))
            {
                EditorUtility.DisplayDialog("Invalid extension", "Selected file is not binary asset file.", "OK");
                return null;
            }

            return LoadFromBinaryAsset<T>(path, signature);
        }
        
        private static T LoadFromBinaryAsset<T>(string path, uint? signature = null) where T : class, ISerializable, new()
        {
            if (!File.Exists(path) || !path.EndsWith(".bytes"))
            {
                return null;
            }

            using (var fileStream = File.OpenRead(path))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    if (signature != null && !IsSignatureMatch(reader, signature.Value))
                    {
                        return null;
                    }

                    var model = new T();
                    model.Deserialize(reader);
                    return model;
                }
            }
        }
        
        public static bool IsSignatureMatch(BinaryReader reader, uint signature)
        {
            var position = reader.BaseStream.Position;

            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            var result = signature == reader.ReadUInt32();

            reader.BaseStream.Seek(position, SeekOrigin.Begin);

            return result;
        }
    }
}