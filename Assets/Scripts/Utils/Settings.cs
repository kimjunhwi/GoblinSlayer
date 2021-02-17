using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace junkaki {

    /// <summary>
    /// Save/Load settings from PlayerPrefs
    /// </summary>
    public static class Settings {

        /// <summary>
        /// Increase and save a value and return it
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IncreaseAndSave(string key, int value) {
            int totalValue =　LoadInt(key, 0) + value;
            PlayerPrefs.SetInt(key, totalValue);
            PlayerPrefs.Save();
            return totalValue;
        }

        /// <summary>
        /// Saves an String data.
        /// </summary>
        public static void Save(string key, string value) {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Saves an Int data.
        /// </summary>
        public static void Save(string key, int value) {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Saves an Bool data.
        /// </summary>
        public static void Save(string key, bool value) {
            PlayerPrefs.SetInt(key, value == true ? 1 : 0);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Saves an Float data.
        /// </summary>
        public static void Save(string key, float value) {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Saves an Long data.
        /// </summary>
        public static void Save(string key, long value) {
            PlayerPrefs.SetString(key, value.ToString());
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static string Load(string key, string defaultValue) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetString(key);
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static int Load(string key, int defaultValue) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetInt(key);
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static float Load(string key, float defaultValue) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetFloat(key);
        }

        /// <summary>
        /// Get an Long data. If not exist the value, create it with default value
        /// </summary>
        public static long Load(string key, long defaultValue) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return Convert.ToInt64(PlayerPrefs.GetString(key));
        }

        /// <summary>
        /// Get an Bool data. If not exist the value, create it with default value
        /// </summary>
        public static bool Load(string key, bool defaultValue) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue == true ? 1 : 0);
                return defaultValue;
            }
            return PlayerPrefs.GetInt(key) == 1;
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static string LoadString(string key, string defaultValue = "") {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetString(key);
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static int LoadInt(string key, int defaultValue = 0) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetInt(key);
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static bool LoadBool(string key, bool defaultValue = false) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetInt(key) == 1;
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static float LoadFloat(string key, float defaultValue = 0.0f) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return PlayerPrefs.GetFloat(key);
        }

        /// <summary>
        /// Get an String data. If not exist the value, create it with default value
        /// </summary>
        public static long LoadLong(string key, long defaultValue = 0) {
            if (!PlayerPrefs.HasKey(key)) {
                Save(key, defaultValue);
                return defaultValue;
            }
            return Convert.ToInt64(PlayerPrefs.GetString(key));
        }

        /// <summary>
        /// Save an Serializable Object to an file in persisten data path.
        /// </summary>
        /// <returns><c>true</c>, if data was saved, <c>false</c> otherwise.</returns>
        /// <param name="serializableObject">Serializable object.</param>
        /// <param name="filename">Filename.</param>
        /// <remarks><paramref name="filename" /> is relative to <see cref="Application.persistentDataPath" /></remarks>
        public static bool SaveObjectToFile(object serializableObject, string filename) {
            try {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + filename);
                Debug.Log("-[HNCORE] Settings Save: Saving object to file. Filename: " + Application.persistentDataPath + Path.DirectorySeparatorChar + filename);

                binaryFormatter.Serialize(file, serializableObject);
                file.Close();
                return true;
            } catch (Exception e) {
                Debug.LogError("-[HNCORE] Settings Save: Exception Saving object to file. Filename: " + filename + ". Exception: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Load an persistent data file to an Serializable Object.
        /// Return a <see langword="object"/>, need force object convertion to correct type.
        /// </summary>
        /// <returns>An Object or <see langword="null"/> if error or not file found.</returns>
        /// <param name="filename">Filename.</param>
        /// <remarks><paramref name="filename" /> is relative to <see cref="Application.persistentDataPath" /></remarks>
        public static object LoadObjectFromFile(string filename) {
            if (!File.Exists(Application.persistentDataPath + "/" + filename)) {
                return null;
            }

            try {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + filename, FileMode.Open);
                Debug.Log("-[HNCORE] Settings Load: Loading object from file. Filename: " + Application.persistentDataPath + Path.DirectorySeparatorChar + filename);
                var loadedObject = binaryFormatter.Deserialize(file);
                file.Close();
                return loadedObject;
            } catch (Exception e) {
                Debug.LogError("-[HNCORE] Settings Load: Exception Loading object from file. Filename: " + filename + ". Exception: " + e.Message);
                return null;
            }
        }

    }
}