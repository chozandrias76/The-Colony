using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

public class FontSheetGetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
	StringBuilder output = new StringBuilder();

	string xmlString =
	    @"<bookstore>
	        <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
	            <title>The Autobiography of Benjamin Franklin</title>
	            <author>
	                <first-name>Benjamin</first-name>
	                <last-name>Franklin</last-name>
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
	static Texture2D keyTexture;
	private Texture2D fontTextureSheet = Resources.Load("FontSpriteSheet01") as Texture2D;
	public Texture2D KeyTableLookup(int keyDown)
	{
		int width,height;
		int locX,locY;
		XmlDocument doc = new XmlDocument();
		doc.Load(Application.dataPath+"/Resources/Metrics.xml");
		string keyIdent = System.String.Format("character key=\"{0}\"", keyDown);
		Debug.Log(keyIdent);
		XmlNodeReader reader = new XmlNodeReader(doc);
		reader.ReadStartElement("file");
		reader.ReadStartElement(keyIdent);
		reader.ReadStartElement("x");
		locX = System.Convert.ToInt32(reader.ReadString());
		reader.ReadEndElement();
		reader.ReadStartElement("y");
		locY = System.Convert.ToInt32(reader.ReadString());
		reader.ReadEndElement();
		reader.ReadStartElement("width");
		width = System.Convert.ToInt32(reader.ReadString());
		reader.ReadEndElement();
		reader.ReadStartElement("height");
		height = System.Convert.ToInt32(reader.ReadString());
		reader.ReadEndElement();
		reader.ReadEndElement();
		reader.ReadEndElement();
		keyTexture = new Texture2D(width,height);
		keyTexture.SetPixels(locX,locY,width,height,fontTextureSheet.GetPixels(locX,locY,width,height));
		return keyTexture;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
}
