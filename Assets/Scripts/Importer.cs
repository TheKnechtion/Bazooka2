using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
public class Importer
{
    StreamReader streamReader;


    string currentLineInTxtFile;
    List<string> allLinesInTxtFile = new List<string>();

    //method to import data
    public List<string> Import(string path)
    {
        //clear the list containing txt file names
        allLinesInTxtFile.Clear();

        try
        {
            //Sets a stream reader to a specified text file stored in scripts
            using (streamReader = new StreamReader(Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\" + path))
            {
                //skips the first line explanatory data in the txt file
                streamReader.ReadLine();

                //while stream reader is not null...
                while ((currentLineInTxtFile = streamReader.ReadLine()) != null)
                {
                    //...add current line in txt file to list of strings
                    allLinesInTxtFile.Add(currentLineInTxtFile);
                }

                //close the stream reader
                streamReader.Close();
            }

        }
        catch
        {

        }
        //returns the txt file data to enemy database
        return allLinesInTxtFile;
    }


}
