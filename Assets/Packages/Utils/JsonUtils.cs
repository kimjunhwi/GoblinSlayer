using System;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;

namespace junkaki {
    public static class JsonUtils<T> {

        /// <summary>
        /// Save the specified data to json format.
        /// </summary>
        /// <returns>The result. True = Successful, False = Error.</returns>
        /// <param name="filename">Filename.</param>
        /// <param name="data">Data.</param>
        /// <remarks><paramref name="filename" /> is relative to <see cref="Application.persistentDataPath" /></remarks>
        public static bool Save(string filename, T data) {
            // Check for no valid file
            if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + filename)) {
                Debug.LogWarning("-[HNCORE] JsonUtils: Warning reading file. Filename exist: " + Application.persistentDataPath + Path.DirectorySeparatorChar + filename);
            }

            try {
                StreamWriter stream = new StreamWriter(new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + filename, FileMode.Create));
                string jsonData = JsonUtility.ToJson(data);
                stream.Write(jsonData);
                stream.Close();

                return true;
            } catch (Exception e) {
                Debug.LogError("-[HNCORE] JsonUtils: Error reading file. Filename: " + Application.persistentDataPath + Path.DirectorySeparatorChar + filename + ". Error: " + e.Message);
                data = default(T);
                return false;
            }
        }

        /// <summary>
        /// Load data from resource folder.
        /// </summary>
        /// <returns><c>true</c>, if from resource was loaded, <c>false</c> otherwise.</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="data">Data.</param>
        /// <remarks><paramref name="resource" /> is relative to resources folder. You must not use file extensions</remarks>
        public static bool LoadFromResource(string resource, out T data) {
            try {
                TextAsset jsonTextFile = Resources.Load(resource) as TextAsset;
                data = JsonUtility.FromJson<T>(jsonTextFile.text);
            } catch (Exception e) {
                Debug.LogError("-[HNCORE] JsonUtils: Error reading Resource file. Resource file: " + resource + ". Error: " + e.Message);
                data = default(T);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load the specified filename with json format to data.
        /// Application.persistentDataPath
        /// </summary>
        /// <returns>The result. True = Successful, False = Error.</returns>
        /// <param name="filename">Filename.</param>
        /// <param name="data">Data.</param>
        /// <remarks><paramref name="filename" /> is relative to <see cref="Application.persistentDataPath" /></remarks>
        public static bool Load(string filename, out T data) {
            // Check for no valid file
            if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + filename)) {
                Debug.LogError("-[HNCORE] JsonUtils: Error reading file. Filename: " + Application.persistentDataPath + Path.DirectorySeparatorChar + filename + ". No exist file.");
                data = default(T);
                return false;
            }

            try {
                StreamReader stream = new StreamReader(new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + filename, FileMode.Open));
                data = JsonUtility.FromJson<T>(stream.ReadToEnd());
                stream.Close();

                return true;
            } catch (Exception e) {
                Debug.LogError("-[HNCORE] JsonUtils: Error reading file. Filename: " + filename + ". Error: " + e.Message);
                data = default(T);
                return false;
            }
        }

    }
}
