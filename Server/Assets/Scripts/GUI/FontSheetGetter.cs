#region License

// // FontSheetGetter.cs
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

using System;
using System.Xml;
using UnityEngine;

public class FontSheetGetter : MonoBehaviour
{
    // Use this for initialization

    private static Texture2D keyTexture;
    private readonly Texture2D fontTextureSheet = Resources.Load("FontSpriteSheet01") as Texture2D;

    private void Start()
    {
        /*
	StringBuilder output = new StringBuilder();

	string xmlString =
	    @"<bookstore>
	        <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
	            <title>The Autobiography of Benjamin Franklin</title>
	            <author>
	                <first-playerName>Benjamin</first-playerName>
	                <last-playerName>Franklin</last-playerName>
	            </author>
	            <price>8.99</price>
	        </book>
	    </bookstore>";

// Create an XmlReader
	using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
	{
	    reader.ReadToFollowing("book");
	    reader.MoveToFirstAttribute();
	    string genre = reader.Value;
	    output.AppendLine("The genre value: " + genre);
	
	    reader.ReadToFollowing("title");
	    output.AppendLine("Content of the title element: " + reader.ReadElementContentAsString());
	}

	OutputTextBlock.Text = output.ToString();
	
	 * parser=new XMLParser();
 * var node=parser.Parse("<example><value type=\"String\">Foobar</value><value type=\"Int\">3</value></example>");
	*/
    }

    public Texture2D KeyTableLookup(int keyDown)
    {
        int width, height;
        int locX, locY;
        var doc = new XmlDocument();
        doc.Load(Application.dataPath + "/Resources/Metrics.xml");
        string keyIdent = String.Format("character key=\"{0}\"", keyDown);
        Debug.Log(keyIdent);
        var reader = new XmlNodeReader(doc);
        reader.ReadStartElement("file");
        reader.ReadStartElement(keyIdent);
        reader.ReadStartElement("x");
        locX = Convert.ToInt32(reader.ReadString());
        reader.ReadEndElement();
        reader.ReadStartElement("y");
        locY = Convert.ToInt32(reader.ReadString());
        reader.ReadEndElement();
        reader.ReadStartElement("width");
        width = Convert.ToInt32(reader.ReadString());
        reader.ReadEndElement();
        reader.ReadStartElement("height");
        height = Convert.ToInt32(reader.ReadString());
        reader.ReadEndElement();
        reader.ReadEndElement();
        reader.ReadEndElement();
        keyTexture = new Texture2D(width, height);
        keyTexture.SetPixels(locX, locY, width, height, fontTextureSheet.GetPixels(locX, locY, width, height));
        return keyTexture;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}