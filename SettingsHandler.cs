﻿using System.IO;

namespace GenericSettingsLoader
{
    public class SettingsHandler<T> where T : new()
    {
        // This is the main object loaded from settingsfile
        private T mLoadedSettingsObject;

        // And this is accessed through this readonly property
        public T Settings { get { return mLoadedSettingsObject; } }

        
        // This is the filename of the settingsfile
        string mSettingsFileName;

        public SettingsHandler(string filename)
        {
            mSettingsFileName = filename;

            LoadOrCreate();
        }

        private void LoadOrCreate()
        {
            // If we do not have a existing settingsfile we create a default setting
            if (!File.Exists(mSettingsFileName))
            {
                mLoadedSettingsObject = new T();

                // Create path if it does not already exists
                string path = Path.GetDirectoryName(mSettingsFileName);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // Save the new object
                ObjectLoader<T>.SaveFile(mSettingsFileName, mLoadedSettingsObject);
            }
            else
            {
                mLoadedSettingsObject = ObjectLoader<T>.LoadFile(mSettingsFileName);
            }
        }

        public void Commit()
        {
            ObjectLoader<T>.SaveFile(mSettingsFileName, mLoadedSettingsObject);
        }
    }
}