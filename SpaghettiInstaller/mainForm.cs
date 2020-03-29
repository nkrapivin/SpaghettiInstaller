using Microsoft.Win32; // for Steam detection ONLY!
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static SpaghettiInstaller.localizationClass; // for Localization.

/*
 * Hewwo!
 * This is basically the core of "SpaghettiInstaller"
 * Have fun!
 * 
 * Made by nik the neko originally for Depa the dog.
 */

namespace SpaghettiInstaller
{
    public partial class mainForm : Form
    {
        public int currentPage = 0; // greeting page is 0.
        public string gamePath = ""; // path for Undertale.
        public string steamPath = ""; // this will be filled with Steam's installation path (and later, UNDERTALE's path)
        public bool haveSteam = false;
        public bool gameNotInstalled = false; // a small variable used to check piracy stuff...
        public bool userChoseSteamCopy = false; // if user decided to use Steam copy of UNDERTALE.
        public const string dataFileName = "data.win"; // game.droid/game.unx for other platforms.

        public const string dataWantedSHA256 = "B718F8223A5BB31979FFEED10BE6140C857B882FC0D0462B89D6287AE38C81C7"; // Undertale v1.08 Steam(!)
        public const string dataGOGSHA256 = "946C738C6D6182F683431320A0E32470BC3DEB50F3E8887E61998DF0C4E3ABC8"; // Undertale v1.08 GOG(!) "setup_undertale_1.08_(18328).exe"

        public static string tempFolder = Path.GetTempPath();
        public static string patcherPath = AppDomain.CurrentDomain.BaseDirectory; // directory where patcher is located.

        public const bool usesOptionalFiles = true; // use "Optional Files" or not?
        public static string optionalFilesPath = patcherPath + "Optional Files" + Path.DirectorySeparatorChar; // Optional Files directory.
        public static string patchFile = patcherPath + "UndertaleTogether.xdelta"; // pleaz change.

        public mainForm()
        {
            InitializeComponent();
            sanityCheck();
            steamCheck();

            applyLocalizationStrings();
        }

        private void applyLocalizationStrings()
        {
            Text = getLocalizationString("mainForm.Text");
            titleLabel.Text = getLocalizationString("titleLabel.Text");
            quitButton.Text = getLocalizationString("quitButton.Text");
            prevButton.Text = getLocalizationString("prevButton.Text");
            nextButton.Text = getLocalizationString("nextButton.Text");
            thankyouLabel.Text = getLocalizationString("thankyouLabel.Text");
            instructionLabel.Text = getLocalizationString("instructionLabel.Text");
        }

        private string getLocalizationString(string id)
        {
            var locale = "{" + ci + "}";
            if (!localizationDictionary.ContainsKey(locale))
            {
                locale = "{en-US}";
                Console.WriteLine("Cannot find current locale.");
            }

            // Overwrite localization in case of a wrong one... (pt-* for example, it can be pt-BR or can not.)
            if (File.Exists(patcherPath + "loc.txt"))
                locale = "{en-US}";

            return localizationDictionary[id + localizationDictionary[locale]];
        }

        private string FindGOGUT()
        {
            var gogpath1 = @"GOG Games\UNDERTALE\";
            var gogpath2 = @"GOG Games\UNDERTALE\Undertale\";
            var drv = DriveInfo.GetDrives();
            foreach (DriveInfo dInfo in drv)
            {
                try
                {
                    //dInfo.Name = C:\ D:\ E:\ F:\
                    if (File.Exists(dInfo.Name + gogpath1 + dataFileName)) return dInfo.Name + gogpath1;
                    if (File.Exists(dInfo.Name + gogpath2 + dataFileName)) return dInfo.Name + gogpath2;
                }
                catch { } // do nothing.
            }
            return string.Empty; // return an empty string if couldn't find.
        }

        private void BackupGame(string datapath)
        {
            //back up the original file as data.win.bau (bau?????????????)
            if (!userChoseSteamCopy) // because Steam users can use "Verify Integrity of Game Files" button...
            {
                try
                {
                    File.Copy(datapath, datapath + ".bau", true);
                }
                catch
                {
                    MessageBox.Show(
                        getLocalizationString("backupErrorQuestion"),
                        getLocalizationString("errorTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }
        }

        private bool UACCheck(string path)
        {
            try
            {
                File.WriteAllText(path + "check.txt", "bau");
                File.Delete(path + "check.txt");
            }
            catch { return false; }
            return true;
        }

        // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private bool handleOptionalFiles()
        {
            Focus();
            var ret = MessageBox.Show(
                    getLocalizationString("optionalFilesQuestion"),
                    getLocalizationString("questionTitle"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
            Focus();
            if (ret != DialogResult.Yes) return true;

            if (!Directory.Exists(optionalFilesPath)) return false;

            DirectoryCopy(optionalFilesPath, gamePath, true);
            return true;
        }

        private bool PatchGOG(string path, string xdeltapath)
        {
            var steamPatch = patcherPath + "GOGToSteam.xdelta";
            // due to cool reasons I can't embed this file with the patcher,
            // please put it manually okay?

            if (!File.Exists(steamPatch))
            {
                MessageBox.Show(
                    getLocalizationString("cannotFindGOGSteam"),
                    getLocalizationString("errorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            ToggleButtons(false);

            // xdelta3.exe written okay, can continue...
            var xdeltaArgs = @"-d -s """ + path + @""" """ + steamPatch + @""" """ + path + @".gog"""; // only use absolute paths.
            var xdeltaprocinfo = new ProcessStartInfo();
            xdeltaprocinfo.FileName = xdeltapath;
            xdeltaprocinfo.Arguments = xdeltaArgs;
            //xdeltaprocinfo.Verb = "runas";
            var xdeltaProc = Process.Start(xdeltaprocinfo);
            xdeltaProc.WaitForExit();
            xdeltaProc.Close();
            xdeltaProc.Dispose();

            File.Copy(path + ".gog", path, true);
            File.Delete(path + ".gog");

            ToggleButtons(true);

            return true;
        }

        private void PiracyCheck()
        {
            try { File.Delete(patchFile); } catch { }
            // we "try" to remove the patch file without crashing.

            Clipboard.SetText("https://bit.ly/33LVPOT");

            MessageBox.Show(
                getLocalizationString("bauPiracyMsg"),
                getLocalizationString("errorTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            Environment.Exit(-1); // force quit due to an "error".
        }

        private void steamCheck()
        {
            var ret2 = 0;
            var ret = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", string.Empty).ToString();
            int.TryParse(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam\Apps\391540", "Installed", 0).ToString(), out ret2);
            if (ret == string.Empty)
            {
                haveSteam = false; // nope, no Steam in the first place.
                return;
            }
            else
            {
                ret += @"/steamapps/common/Undertale/";
                if (File.Exists(ret + dataFileName)) // found UNDERTALE from the 1st attempt!
                {
                    haveSteam = true;
                    steamPath = ret;
                    return;
                }
                else // hmm... perhaps an external installation folder?
                {
                    if (ret2 > 0)
                    {
                        haveSteam = true;
                        steamPath = "$ASK_FOR_ANOTHER_FOLDER!";
                        return;
                    }
                    else // UNDERTALE is not installed, or it doesn't exist in Steam's library.
                    {
                        gameNotInstalled = true;
                    }
                }
            }
        }

        private void ToggleButtons(bool value)
        {
            nextButton.Enabled = value;
            prevButton.Enabled = value;
        }

        private void sanityCheck()
        {
            if (!File.Exists(patchFile))
            {
                MessageBox.Show(
                    getLocalizationString("cannotFindModPatch"),
                    getLocalizationString("errorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(-1); // force quit.
            }

            // Add some other stuff here you consider important...
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            // :(
            var result = MessageBox.Show(
                getLocalizationString("quitQuestion"),
                getLocalizationString("quitQuestionTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );
            if (result == DialogResult.Yes) Application.Exit(); // safe quit.
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            handlePage(false);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            handlePage(true);
        }

        private void handlePage(bool next) // next or previous?
        {
            if (currentPage == 0 && !next) return; // trying to go back from a greeting page, excuse me?
            else
            {
                if (currentPage == 0 && next)
                {
                    currentPage++;
                    instructionLabel.Text = getLocalizationString("instructionLabel.Text3");
                    return;
                }
                if (currentPage == 1 && !next)
                {
                    currentPage--;
                    instructionLabel.Text = getLocalizationString("instructionLabel.Text2"); // bau.
                    return;
                }
                if (currentPage == 1 && next)
                {
                    // thy main...

                    var askResult = askForGameFolder();
                    if (!askResult)
                    {
                        if (gamePath != "AAAA")
                        MessageBox.Show(
                            getLocalizationString("dataNotFoundError"),
                            getLocalizationString("errorTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    var dataFilePath = gamePath + dataFileName;

                    var xdeltaPath = tempFolder + "xdelta3.exe";
                    try
                    {
                        File.WriteAllBytes(xdeltaPath, Properties.Resources.xdelta3);
                    }
                    catch
                    {
                        MessageBox.Show(
                            getLocalizationString("xdeltaError"),
                            getLocalizationString("errorTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    if (!UACCheck(dataFilePath))
                    {
                        MessageBox.Show(
                            getLocalizationString("runasAdmin"),
                            getLocalizationString("errorTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    var supposedChecksum = GetSHA256(dataFilePath);
                    var backedup = false;

                    if (supposedChecksum == dataGOGSHA256)
                    {
                        MessageBox.Show(
                            getLocalizationString("gogPatch"),
                            getLocalizationString("steamCannotFindFolder"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        BackupGame(dataFilePath);
                        backedup = true;
                        if (!PatchGOG(dataFilePath, xdeltaPath)) return;
                        supposedChecksum = GetSHA256(dataFilePath); // update our checksum.
                    }

                    if (supposedChecksum == dataWantedSHA256 && (!haveSteam || (haveSteam && gameNotInstalled)))
                    {
                        //excuse me, you chose a Steam(!) copy of undertale, and you don't have Steam?
                        //let PiracyCheck handle the rest
                        PiracyCheck();
                    }
                    else if (supposedChecksum == dataWantedSHA256 && haveSteam && !gameNotInstalled && !backedup)
                    {
                        userChoseSteamCopy = true;
                    }

                    ToggleButtons(false);

                    if (!backedup) BackupGame(dataFilePath);

                    if (supposedChecksum == dataWantedSHA256) // all is fine.
                    {

                        // xdelta3.exe written okay, can continue...
                        var xdeltaArgs = @"-d -s """ + dataFilePath + @""" """ + patchFile + @""" """ + dataFilePath + @".new"""; // only use absolute paths.
                        var xdeltaprocinfo = new ProcessStartInfo();
                        xdeltaprocinfo.FileName = xdeltaPath;
                        xdeltaprocinfo.Arguments = xdeltaArgs;
                        //xdeltaprocinfo.Verb = "runas";
                        var xdeltaProc = Process.Start(xdeltaprocinfo);
                        //var xdelta = Process.Start(xdeltaPath, xdeltaArgs);
                        xdeltaProc.WaitForExit();
                        var patchingSuccess = File.Exists(dataFilePath + ".new");
                        xdeltaProc.Close();
                        xdeltaProc.Dispose();

                        if (usesOptionalFiles)
                        {
                            if (!handleOptionalFiles())
                            {
                                MessageBox.Show(
                                    getLocalizationString("cannotCopyOptionalFiles"),
                                    getLocalizationString("errorTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }

                        ToggleButtons(true);

                        if (!patchingSuccess)
                        {
                            MessageBox.Show(
                                getLocalizationString("patchingFailed"),
                                getLocalizationString("errorTitle"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            File.Copy(dataFilePath + ".new", dataFilePath, true);
                            File.Delete(dataFilePath + ".new");
                            displayFinalPage();
                            return;
                        }
                    }
                    else
                    {
                        ToggleButtons(true);
                        MessageBox.Show(
                            getLocalizationString("wrongGameVersion"),
                            getLocalizationString("errorTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                        // Do something here damn it!
                        // ....
                    }
                }
                if (currentPage == 2)
                {
                    try
                    { 
                        File.Delete(tempFolder + "xdelta3.exe"); // try to remove xdelta3.exe as we don't need it anymore.
                    }
                    catch
                    {
                        //Do nothing.
                    }
                    Application.Exit(); // safe quit.
                }
            }
        }

        private void displayFinalPage()
        {
            currentPage++;
            finalDoodle.Visible = true;
            thankyouLabel.Visible = true;

            var thankyoustring = userChoseSteamCopy ? getLocalizationString("revertModSteam") : getLocalizationString("revertModGOG");

            thankyouLabel.Text = thankyouLabel.Text.Replace("$REPLACETEXT$", thankyoustring);
            nextButton.Text = "Finish";

            instructionLabel.Visible = false;
            prevButton.Enabled = false; // there's no going back...
        }

        private string GetSHA256(string path)
        {
            // Initialize a SHA256 hash object.
            using (SHA256 mySHA256 = SHA256.Create())
            {
                try
                {
                    // Create a fileStream for the file.
                    var fileStream = new FileStream(path, FileMode.Open);
                    // Be sure it's positioned to the beginning of the stream.
                    fileStream.Position = 0;
                    // Compute the hash of the fileStream.
                    byte[] hashValue = mySHA256.ComputeHash(fileStream);
                    // Close the file.
                    fileStream.Close();
                    fileStream.Dispose();

                    var sBuilder = new StringBuilder();
                    for (var i = 0; i < hashValue.Length; i++)
                        sBuilder.Append(hashValue[i].ToString("X2"));

                    hashValue = new byte[0]; // trigger Garbage Collector to dump the old array.
                    return sBuilder.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        getLocalizationString("veryBadError") + e.ToString(),
                        getLocalizationString("errorTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return string.Empty;
                }
            }
        }

        private bool askForGameFolder()
        {
            var gogpath = FindGOGUT();

            if (gogpath != string.Empty)
            {
                var ret = MessageBox.Show(
                        getLocalizationString("gogQuestion"),
                        getLocalizationString("questionTitle"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                if (ret == DialogResult.Yes)
                {
                    gamePath = gogpath;
                    userChoseSteamCopy = false;
                    return true;
                }
            }

            if (haveSteam && steamPath == "$ASK_FOR_ANOTHER_FOLDER!")
            {
                MessageBox.Show(
                    getLocalizationString("cannotDetectSteamPath"),
                    getLocalizationString("steamCannotFindFolder"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (haveSteam && steamPath != "$ASK_FOR_ANOTHER_FOLDER!" && steamPath != string.Empty)
            {
                var ret = MessageBox.Show(
                    getLocalizationString("steamQuestion"),
                    getLocalizationString("questionTitle"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (ret == DialogResult.Yes)
                {
                    gamePath = steamPath;
                    userChoseSteamCopy = true;
                    return true;
                }
            }

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = getLocalizationString("folderBrowserDialog");
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    gamePath = fbd.SelectedPath + Path.DirectorySeparatorChar;

                    if (File.Exists(gamePath + dataFileName))
                    {
                        return true;
                        //the function ends here, returning to handlePage.
                    }
                    else
                    {
                        return false;
                    }
                }
                gamePath = "AAAA";
                return false;
            }
        }
    }
}
