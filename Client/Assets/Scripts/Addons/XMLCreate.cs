using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLCreate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	using (XmlWriter writer = XmlWriter.Create("fontmetrics.xml"))
		{
		    writer.WriteStartDocument();
		    writer.WriteStartElement("fontMetrics");
			writer.WriteAttributeString("file", "FontSpriteSheet02.png");
			int x = 0;
			int locX;
			int y = 15;
			int locY;
			int width = 64;
			int height = 64;
		    for(int i = 0; i < 256; i++)
		    {
				locX = x * width;
				locY = y * height;
			writer.WriteStartElement("character");
			writer.WriteAttributeString("key", System.Convert.ToString(i));
			writer.WriteElementString("x", System.Convert.ToString((locX)));
			writer.WriteElementString("y",System.Convert.ToString(locY));
			writer.WriteElementString("width",System.Convert.ToString(width));
			writer.WriteElementString("height",System.Convert.ToString(height));
			writer.WriteEndElement();
				if(x >= 15)
				{
					x = 0;
					y--;
				}
				else if(x < 15)
					x++;
		    }
		
		    writer.WriteEndElement();
		    writer.WriteEndDocument();
			writer.Close();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
