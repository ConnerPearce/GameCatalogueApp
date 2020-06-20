using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Pages.Home;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameCatalogueApp.Classes.StorageManager
{


    public class Storage
    {
        // read a text file from the app's local folder
        public static async Task<string> ReadTextFileAsync(string _filename, HomePage.ErrorHandling errorHandling)
        {
            // declare an empty variable to be filled later
            string result = null;

            try
            {
                // get hold of the file system
                IFolder rootFolder = FileSystem.Current.LocalStorage;

                // create a folder, if one does not exist already
                IFolder folder = await rootFolder.CreateFolderAsync("GameCatalogueStorage", CreationCollisionOption.OpenIfExists);

                // create a file, overwriting any existing file
                IFile file = await rootFolder.CreateFileAsync(_filename, CreationCollisionOption.OpenIfExists);

                // populate the file with some text
                result = await file.ReadAllTextAsync();
            }
            catch (Exception ex)
            {
                errorHandling(ex.Message);
            }

            // return the contents of the file
            return result;
        }

        // write a text file to the app's local folder
        public static async Task<string> WriteTextFileAsync(string _filename, string _content, HomePage.ErrorHandling errorHandling)
        {
            // declare an empty variable to be filled later
            string result = null;
            try
            {
                // get hold of the file system
                IFolder rootFolder = FileSystem.Current.LocalStorage;

                // create a folder, if one does not exist already
                IFolder folder = await rootFolder.CreateFolderAsync("GameCatalogueStorage", CreationCollisionOption.OpenIfExists);

                // create a file, overwriting any existing file
                IFile file = await rootFolder.CreateFileAsync(_filename, CreationCollisionOption.ReplaceExisting);

                // populate the file with some text
                await file.WriteAllTextAsync(_content);

                result = _content;
            }
            // if there was a problem
            catch (Exception ex)
            {
                errorHandling(ex.Message);
            }

            // return the contents of the file
            return result;
        }
    }
}
